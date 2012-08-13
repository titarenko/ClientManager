using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParserThunderbird : IMailMessageParser
    {
        const string emailMatch = @"\b([A-Z0-9._%-]+)@([A-Z0-9.-]+\.[A-Z]{2,6})\b";

        public string GetSubject(string subject)
        {
            int startPos = 0;

            if (subject.ToLower().StartsWith("fwd:"))
                startPos = 5;
            if (subject.ToLower().StartsWith("fw:"))
                startPos = 4;

            return subject.Substring(startPos);
        }

        public ICollection<MailAddress> GetReceivers(MailMessage mailMessage)
        {
            var emailRegex = new Regex(emailMatch, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var emails = emailRegex.Matches(mailMessage.Body);

            var receivers = new List<MailAddress>();

            // Should we return addresses in CC section as receivers?
            for (int i = 1; i < emails.Count; i++)
            {
                receivers.Add(new MailAddress(emails[i].Value));
            }

            return receivers;

            //var stringBuilderForMailAddress = new StringBuilder();
            //var bodyInLower = mailMessage.Body.ToLower();
            ////find in end of body to: 
            //var indexOfTo = bodyInLower.IndexOf("\nto: ", System.StringComparison.Ordinal);

            //var indexOfEndOfLine = bodyInLower.IndexOf("\n", indexOfTo+1, System.StringComparison.Ordinal);

            //var currentIndex = indexOfTo;

            //var receivers = new List<MailAddress>();

            //while (currentIndex<indexOfEndOfLine && currentIndex!=-1)
            //{
            //    //find symbol @ in "to: ........ @...."
            //    var indexOfAt = bodyInLower.IndexOf("@", currentIndex, indexOfEndOfLine-currentIndex, System.StringComparison.Ordinal);

            //    if (indexOfAt==-1)
            //        break;

            //    stringBuilderForMailAddress.Append("@");

            //    //append to mail address everything that lefter of @
            //    for (var i = indexOfAt - 1; i > 0; i--)
            //    {
            //        if (Char.IsLetterOrDigit(bodyInLower[i]))
            //        {
            //            stringBuilderForMailAddress.Insert(0, bodyInLower[i]);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }

            //    //append to mail address everything that righter of @
            //    for (var i = indexOfAt + 1; i < bodyInLower.Length; i++)
            //    {
            //        if (Char.IsLetterOrDigit(bodyInLower[i]) || bodyInLower[i] == '.')
            //        {
            //            stringBuilderForMailAddress.Append(bodyInLower[i]);
            //        }
            //        else
            //        {
            //            currentIndex = i;
            //            break;
            //        }
            //    }

            //    var mailAddress = new MailAddress(stringBuilderForMailAddress.ToString());
            //    receivers.Add(mailAddress);
            //    stringBuilderForMailAddress.Clear();
            //}

            ////return list of mail addresses
            //return receivers;
        }

        public MailAddress GetSender(MailMessage mailMessage)
        {
            var mailRegex = new Regex(emailMatch, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var matchesEmails = mailRegex.Matches(mailMessage.Body);
            try
            {
                var sender = matchesEmails[0].Value;
                return new MailAddress(sender);
            }
            catch (Exception)
            {
                throw new ApplicationException("Forwarded message has illegal format");
            }
            
        }   

        public bool IsForwarded(MailMessage mailMessage)
        {
            return mailMessage.Subject.ToLower().Trim().StartsWith("fwd:") || mailMessage.Subject.ToLower().StartsWith("fw:");
        }
    }
}
