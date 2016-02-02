using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Phone.Tasks;

namespace DnD_3._5_SRD_Navigator7.Share
{
    /// <summary>
    /// This will not be used until either livemailmessage is purchased or Microsoft provides the additional functionality needed to programmatically assign attachments to an email.
    /// </summary>
    internal class EmailHelper
    {

        private string itemName = string.Empty;
        private string fullHtml = string.Empty;

        public EmailHelper()
        {   
        }

        /// <summary>
        /// Compose an email for sending.
        /// </summary>
        public void sendEmailNow(string _itemName, string _fullHtml)
        {
            if(validate(_itemName, _fullHtml)){
                try
                {
                    EmailComposeTask composeEmail = new EmailComposeTask();
                    composeEmail.Subject = itemName;// Name of Selected Item
                    composeEmail.Body = fullHtml;//html page of full_text for item

                    composeEmail.Show();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error while configuring email: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Make sure the passed in values are good.
        /// </summary>
        /// <returns></returns>
        private bool validate(string _itemName, string _fullHtml)
        {
            try
            {
                if (_itemName.Length > 0 && _fullHtml.Length > 0)
                {
                    itemName = _itemName;
                    fullHtml = _fullHtml;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error while checking provided email properties: " + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
        }
    }
}
