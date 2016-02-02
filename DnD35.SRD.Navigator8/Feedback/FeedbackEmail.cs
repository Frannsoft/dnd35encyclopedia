using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD_3._5_SRD_Navigator8.Feedback
{

    /// <summary>
    /// Compose an email for feedback to send to ME.
    /// </summary>
   internal class FeedbackEmail
    {
       public FeedbackEmail()
       {
           EmailComposeTask emailComposeTask = new EmailComposeTask();
           emailComposeTask.Subject = "DnD 3.5 Encyclopedia Error Feedback - Version: " + System.Xml.Linq.XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
           emailComposeTask.Body = "";
           emailComposeTask.To = "FrannSoftDev@outlook.com";
           emailComposeTask.Show();
       }

       public FeedbackEmail(string data)
       {
           EmailComposeTask emailComposeTask = new EmailComposeTask();
           emailComposeTask.Subject = "DnD 3.5 Encyclopedia Error Feedback - Version: " + System.Xml.Linq.XDocument.Load("WMAppManifest.xml").Root.Element("App").Attribute("Version").Value;
           emailComposeTask.Body = "What happened: " + data + "Feel free to add any additional information you feel is necessary.";
           emailComposeTask.To = "FrannSoftDev@outlook.com";
           emailComposeTask.Show();
       }
    }
}
