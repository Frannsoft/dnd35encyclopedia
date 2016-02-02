using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DnD_3._5_SRD_Navigator8.Constants
{
    /// <summary>
    /// Contains all constants used when dealing with the user's Isolated Storage.
    /// </summary>
    internal static class IsolatedStorage
    {
        public const string ISO_PREVIOUS_SEARCHES = "PreviousSearches";
        public const string ISO_FAVORITES = "Favorites";
        public const string ISO_DB_VERSION = "DBVersion";
        public const string ISO_EXCEPTION = "LastException";

        /// <summary>
        /// Whether or not to show a message box to the user stating that bluetooth should be enabled for the best NFC experience.  0 = show it, 1 = do not show it.
        /// </summary>
        public const string ISO_SHOW_MSGBOX_PROXIMITY_BLUETOOTH = "NFCMessageBox";

        /// <summary>
        /// The key used to check if the user needs to view the recent update message.  Not going to be used right now.
        /// </summary>
        //public const string ISO_UPDATE_MESSAGE = "UpdateMessage";

        /// <summary>
        /// The actual text to show the user who needs to see it.  Not going to be used right now.
        /// </summary>
        //public const string ISO_UPDATE_MESSAGE_TEXT = "12/29/13 - Changelog: \n - Can add an item in Browse to Favorites by Long Pressing the item in the Browse List! \n - Fixed some issues with Items in database \n - Fixed bug in history list. \n NOTE: Application will soon have a WP8 specific version (instead of being built on WP7 platform) in order to take advantage of new WP8 features such as NFC.  The WP7 version will still receive bug fixes when necessary.  More info will be provided in the next update.  \nThank you for using the D&D 3.5 Encyclopedia!"; //message text to show.

        /// <summary>
        /// Used to show only the most recent update message and only if the user needs to see it.  Not going to be used right now.
        /// </summary>
        //public const string ISO_UPDATE_MESSAGE_VERSION = "1";
    }
}
