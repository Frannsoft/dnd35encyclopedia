
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using DnDNavigator.Runtime.Model.DnDEntry;

namespace DnDNavigator.Runtime.Model.Menu
{
    public class MenuOptionsService : IMenuOptionsService
    {
        private const string HOME_MENU_OPTION_PATH = "HomeMenuOptions.json";

        /// <summary>
        /// Gets the data from the home menu options json file.  If the file doesn't exist,
        /// the file is created from a serialized list of MenuOption objects.  The file is then read.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MenuOption>> RefreshHomeMenu()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile jsonFile = null;
            string json = string.Empty;
            var options = new List<MenuOption>();

            bool doesOptionsFileExist = false;
            try
            {
                jsonFile = await localFolder.GetFileAsync(HOME_MENU_OPTION_PATH);
                doesOptionsFileExist = true;

            }
            catch (FileNotFoundException)
            {
                doesOptionsFileExist = false;
            }

            if (!doesOptionsFileExist)
            {
                await CreateHomeMenuOptions(localFolder);
                jsonFile = await localFolder.GetFileAsync(HOME_MENU_OPTION_PATH);
            }           

            using (IRandomAccessStream stream = await jsonFile.OpenReadAsync())
            {
                //read the stream
                using (DataReader reader = new DataReader(stream))
                {
                    //get size
                    uint length = (uint)stream.Size;
                    await reader.LoadAsync(length);

                    //read data
                    json = reader.ReadString(length);

                    //deserialize to .net type
                    options = JsonConvert.DeserializeObject<List<MenuOption>>(json);
                }
            }
            return options;
        }

        /// <summary>
        /// Creates the home menu options file.  This is a json serialized list of MenuOption objects.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private async Task CreateHomeMenuOptions(StorageFolder folder)
        {
            //create the file
            List<MenuOption> defaultOptions = new List<MenuOption>(){
                new MenuOption("Search", "Find what you're looking for - improved!", EntryType.Types.Search),
                new MenuOption("Classes", "What can your Class(es) do?", EntryType.Types.Class),
                new MenuOption("Domains", "Domain specific information.", EntryType.Types.Domain),
                new MenuOption("Equipment", "The Armory.", EntryType.Types.Equipment),
                new MenuOption("Feats", "Feats of strength, courage, wisdom and more.", EntryType.Types.Feat),
                new MenuOption("Items", "Items acquired on your adventure.", EntryType.Types.Item),
                new MenuOption("Monsters", "The creatures of DND 3.5.", EntryType.Types.Monster),
                new MenuOption("Powers", "Powers mighty to behold.", EntryType.Types.Power),
                new MenuOption("Skills", "What makes you a critical part of the team?", EntryType.Types.Skill),
                new MenuOption("Spells", "Potent (and some not so potent) spells.", EntryType.Types.Spell),
                };

            string optionJson = JsonConvert.SerializeObject(defaultOptions);

            StorageFile file = await folder.CreateFileAsync(HOME_MENU_OPTION_PATH, CreationCollisionOption.ReplaceExisting);

            //open the file
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                //write the string to the file
                using (DataWriter writer = new DataWriter(stream))
                {
                    writer.WriteString(optionJson);
                    await writer.StoreAsync();
                }
            }
        }
    }
}
