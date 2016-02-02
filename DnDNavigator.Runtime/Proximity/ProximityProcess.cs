
using DnDNavigator.Runtime.Model.DnDEntry;
using Microsoft.Phone.Controls;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using System;
using System.IO;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using DnDNavigator.Runtime.DataAccess;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using DnDNavigator.Runtime.Model.Entry;

namespace DnDNavigator.Runtime.Proximity
{
    public class ProximityProcess : INotifyPropertyChanged
    {
        #region Private members
        private StreamSocket streamSocket = null;
        private string storedRemoteHostRawName = string.Empty;
        private string storedRemoteServiceName = string.Empty;
        private List<Type> knownTypes;
        #endregion

        #region Properties
        public ProximityDevice Device { get; private set; }
        public bool IsSupported { get; private set; }
        public bool IsHost { get; private set; }
        public string Information { get; private set; }
        public BaseEntry Entry { get; set; }
        public bool HasReceivedData { get; private set; }
        public bool HasCompletedSendingData { get; private set; }
        #endregion

        public ProximityProcess()
        {
            //init the list of types to expect for shared data
            knownTypes = new List<Type>()
            {
                typeof(Class),
                typeof(Domain),
                typeof(Equipment),
                typeof(Feat),
                typeof(Item),
                typeof(Monster),
                typeof(Power),
                typeof(Skill),
                typeof(Spell)
            };
        }

        public void UpdateInformation(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    Information = Information + "\n" + message;
                    OnPropertyChanged("Information");
                });
        }

        public void ClearInformation()
        {
            Information = string.Empty;
        }

        public void InitializeProximity(bool isHost)
        {
            Device = ProximityDevice.GetDefault();

            if(Device == null)
            {
                throw new InvalidOperationException("Device does not have NFC capability.  Maybe it just needs to be turned on.");
            }

            IsHost = isHost;
            ClearInformation(); //start over
            PeerFinder.TriggeredConnectionStateChanged += OnTriggeredConnectionStateChanged;
            Share(); //start the share process 
        }

        public async Task RetryConnection(StreamSocket socket)
        {
            PeerFinder.Stop();
            Share();
            HostName newRemoteHostName = new HostName(storedRemoteHostRawName);
            await socket.ConnectAsync(newRemoteHostName, storedRemoteServiceName);
        }

        /// <summary>
        /// Fired when a published message is received.  Converts the message binary data to a byte[] and then 
        /// deserializes it to an IEntry which is then stored is isolatedstorage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns>IEntry</returns>
        public void MessageReceived(ProximityDevice sender, ProximityMessage message)
        {
            using (MemoryStream memoryStream = new MemoryStream(message.Data.ToArray()))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(BaseEntry), knownTypes);

                var entry = serializer.ReadObject(memoryStream) as BaseEntry;

                if (entry != null)
                {
                    IsolatedStorage.TempEntry = entry;
                    HasReceivedData = true;
                    OnPropertyChanged("HasReceivedData");
                }
                else
                {
                    UpdateInformation("There was an error receiving the entry.");
                }
            }
        }

        /// <summary>
        /// Sends the actual data over the connected device.
        /// </summary>
        public void SendData()
        {
            long msgID = 0L;

            if (Entry != null)
            {
                DataContractSerializer serializer = new DataContractSerializer(Entry.GetType().BaseType, knownTypes);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    serializer.WriteObject(memoryStream, Entry);
                    using (DataWriter dataWriter = new DataWriter())
                    {
                        dataWriter.WriteBytes(memoryStream.ToArray());

                        try
                        {
                            msgID = Device.PublishBinaryMessage("Windows.DnD35", dataWriter.DetachBuffer());
                            StopProximityProcess();
                            HasCompletedSendingData = true;
                        }
                        catch (Exception)
                        {
                            UpdateInformation("There was an issue sharing the entry");
                            StopProximityProcess();
                        }
                    }
                }
            }
            else
            {
                UpdateInformation("The entry was invalid.  Stopping share process.");
                StopProximityProcess();
            }
        }

        public void StopProximityProcess()
        {
            PeerFinder.Stop();
        }

        public void Share()
        {
            PeerFinder.DisplayName = "DnD Reference";

            if (IsHost)
            {
                InitializeEntry();
                UpdateInformation("Attempting to share Entry...");
            }
            UpdateInformation("Looking for Peer...");
            PeerFinder.Start();
        }

        /// <summary>
        /// Gets the stored entry from Isolated Storage if there is one.  If 'Entry' is already set, just returns.
        /// </summary>
        private void InitializeEntry()
        {
            if (Entry == null)
            {
                Entry = IsolatedStorage.TempEntry;
            }
            else
            {
                return;
            }
        }

        private void OnTriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
        {
            switch (args.State)
            {
                //listening as host
                case TriggeredConnectState.Listening:
                    {
                        UpdateInformation("Waiting...\n");
                        break;
                    }
                // Proximity gesture is complete and user can pull their devices away. Remaining work is to 
                // establish the connection using a different transport, like TCP/IP or Bluetooth
                case TriggeredConnectState.PeerFound:
                    {
                        UpdateInformation("Compatible device found!\n");
                        break;
                    }
                // Connecting as a client
                case TriggeredConnectState.Connecting:
                    {
                        UpdateInformation("Connecting...\n");
                        break;
                    }
                // Connection completed, retrieve the socket over which to communicate
                case TriggeredConnectState.Completed:
                    {
                        //PeerFinder.Stop();
                        UpdateInformation("Connection complete!  Transferring item now...  \nWhen your friend sees the item on their device you may press the Back button and end the share.");
                        streamSocket = args.Socket;
                        storedRemoteHostRawName = args.Socket.Information.RemoteHostName.RawName;
                        storedRemoteServiceName = args.Socket.Information.RemoteServiceName;

                        //If the device is the host, send over data
                        if (IsHost)
                        {
                            //now share the actual data
                            SendData();
                        }
                        else //otherwise, don't push any data and wait to receive it
                        {
                            long ID = Device.SubscribeForMessage("Windows.DnD35", MessageReceived);
#if DEBUG
                            Debug.WriteLine("message ID: " + ID);
#endif
                        }
                        UpdateInformation("Transfer completed.  You can go back to your Entry now by pressing the Back button.");
                        break;
                    }
                case TriggeredConnectState.Canceled:
                    {
                        break;
                    }
                //Connection was unsuccessful
                case TriggeredConnectState.Failed:
                    {
                        UpdateInformation("Failed to connect to another device. \n");
                        //if the user wants to, try again
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {

                            CustomMessageBox messageBox = new CustomMessageBox()
                            {
                                Caption = "NFC Error.",
                                Message = "There was an issue connecting to another Device.  Try again?",
                                LeftButtonContent = "Yes",
                                RightButtonContent = "Nope",
                            };
                            messageBox.Dismissed += (s1, e1) =>
                            {
                                switch (e1.Result)
                                {
                                    case CustomMessageBoxResult.LeftButton:
                                        {
                                            if (streamSocket != null)
                                            {
                                                RetryConnection(streamSocket);
                                            }
                                            else
                                            {
                                                Share();
                                            }
                                            break;
                                        }
                                    case CustomMessageBoxResult.RightButton:
                                        {
                                            PeerFinder.Stop();
                                            this.Dispose();
                                            break;
                                        }
                                    case CustomMessageBoxResult.None:
                                        {
                                            PeerFinder.Stop();
                                            this.Dispose();
                                            break;
                                        }
                                    default:
                                        {
                                            PeerFinder.Stop();
                                            this.Dispose();
                                            break;
                                        }
                                }
                            };
                        });
                        break;
                    }
            }
        }

        public void Dispose()
        {
            Device = null;
            streamSocket = null;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}
