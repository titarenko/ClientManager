using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParserThunderbird : IMailMessageParser
    {
        const string emailMatch = @"\b([A-Z0-9._%-]+)@([A-Z0-9.-]+\.[A-Z]{2,6})\b";
        const string tableMatch = @"\w:\s*\t.*\r\n";

        public string GetSubject(string subject)
        {
            int startPos = 0;

            if (subject.ToLower().StartsWith("fwd:"))
                startPos = 5;
            if (subject.ToLower().StartsWith("fw:"))
                startPos = 4;

            return subject.Substring(startPos);
        }


        public string GetBody(MailMessage mailMessage)
        {
            var tableRegex = new Regex(tableMatch, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var tableStrings = tableRegex.Matches(mailMessage.Body);
            if (tableStrings.Count>0)
            {
                var lastTableString = tableStrings[tableStrings.Count - 1].Value;
                return mailMessage.Body.Substring(
                    mailMessage.Body.IndexOf(lastTableString, StringComparison.Ordinal) + lastTableString.Length);
            }
            return mailMessage.Body;
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
    }
}
