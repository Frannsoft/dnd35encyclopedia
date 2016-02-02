using Microsoft.Phone.Tasks;
using System.Xml.Linq;

namespace DnDNavigator.Runtime.Feedback
{

    /// <summary>
    /// Compose an email for feedback to send to ME.
    /// </summary>
    public class FeedbackEmail
    {
        public FeedbackEmail()
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "DnD 3.5 Encyclopedia Feedback - Version: " + XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            emailComposeTask.Body = "";
            emailComposeTask.To = "FrannSoftDev@outlook.com";
            emailComposeTask.Show();
        }

        public FeedbackEmail(string data)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();
            emailComposeTask.Subject = "DnD 3.5 Encyclopedia Error Feedback - Version: " + XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
            emailComposeTask.Body = "What happened: " + data + "Feel free to add any additional information you feel is necessary.";
            emailComposeTask.To = "FrannSoftDev@outlook.com";
            emailComposeTask.Show();
        }
    }
}
