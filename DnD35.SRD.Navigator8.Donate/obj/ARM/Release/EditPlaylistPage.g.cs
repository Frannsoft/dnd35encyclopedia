﻿#pragma checksum "E:\Development\WindowsPhoneProjects\DnD35.SRD.Navigator\DnD35.SRD.Navigator8.Donate\..\DnD35.SRD.Navigator8\EditPlaylistPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "11AC1157FA5247F64B00B5BCB4158865"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace DnD35.SRD.Navigator8 {
    
    
    public partial class EditPlaylistPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton OKAppBarButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton CancelAppBarButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton SelectionAppBarButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton DeleteSelectedPlaylistItemsButton;
        
        internal Microsoft.Phone.Controls.PhoneTextBox PlaylistNamePhoneTextBox;
        
        internal Microsoft.Phone.Controls.LongListMultiSelector PlaylistItemsList;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/DnD35.SRD.Navigator8.Donate;component/EditPlaylistPage.xaml", System.UriKind.Relative));
            this.OKAppBarButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("OKAppBarButton")));
            this.CancelAppBarButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("CancelAppBarButton")));
            this.SelectionAppBarButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("SelectionAppBarButton")));
            this.DeleteSelectedPlaylistItemsButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("DeleteSelectedPlaylistItemsButton")));
            this.PlaylistNamePhoneTextBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("PlaylistNamePhoneTextBox")));
            this.PlaylistItemsList = ((Microsoft.Phone.Controls.LongListMultiSelector)(this.FindName("PlaylistItemsList")));
        }
    }
}

