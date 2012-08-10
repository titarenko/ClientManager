using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParser
    {
        public string GetBody(string body)
        {
            var ccStart = body.IndexOf("\ncc", StringComparison.OrdinalIgnoreCase);
            var ccEnd = body.IndexOf('\n', ccStart + 1);

            return body.Substring(ccEnd + 1).Trim();
        }

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
            var stringBuilderForMailAddress = new StringBuilder();
            var bodyInLower = mailMessage.Body.ToLower();
            //find in end of body to: 
            var indexOfTo = bodyInLower.IndexOf("\nto: ", System.StringComparison.Ordinal);

            var indexOfEndOfLine = bodyInLower.IndexOf("\n", indexOfTo+1, System.StringComparison.Ordinal);

            var currentIndex = indexOfTo;

            var receivers = new List<MailAddress>();
            
            while (currentIndex<indexOfEndOfLine && currentIndex!=-1)
            {
                //find symbol @ in "to: ........ @...."
                var indexOfAt = bodyInLower.IndexOf("@", currentIndex, indexOfEndOfLine-currentIndex, System.StringComparison.Ordinal);

                if (indexOfAt==-1)
                    break;

                stringBuilderForMailAddress.Append("@");

                //append to mail address everything that lefter of @
                for (var i = indexOfAt - 1; i > 0; i--)
                {
                    if (Char.IsLetterOrDigit(bodyInLower[i]))
                    {
                        stringBuilderForMailAddress.Insert(0, bodyInLower[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                //append to mail address everything that righter of @
                for (var i = indexOfAt + 1; i < bodyInLower.Length; i++)
                {
                    if (Char.IsLetterOrDigit(bodyInLower[i]) || bodyInLower[i] == '.')
                    {
                        stringBuilderForMailAddress.Append(bodyInLower[i]);
                    }
                    else
                    {
                        currentIndex = i;
                        break;
                    }
                }

                var mailAddress = new MailAddress(stringBuilderForMailAddress.ToString());
                receivers.Add(mailAddress);
                stringBuilderForMailAddress.Clear();
            }
            
            //return list of mail addresses
            return receivers;
        }

        public MailAddress GetSenderFromForwardedMail(MailMessage mailMessage)
        {
            //const string emailPattern = @"(([^<>()[\]\\.,;:\s@\""]+"
            //                            + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
            //                            + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
            //                            + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
            //                            + @"[a-zA-Z]{2,}))";

            //const string fromPattern = @"From:(\b\w*\b){0,3}(<" + emailPattern + ">)|" + emailPattern;

            //var regexFrom = new Regex(fromPattern);
            //var match = regexFrom.Match(mailMessage.Body);
            //var regexAddress = new Regex(emailPattern);
            //return new MailAddress(regexAddress.Match(match.Value).Value);

            var stringBuilderForMailAddress = new StringBuilder();
            var bodyInLower = mailMessage.Body.ToLower();
            //find in end of body from: 
            var indexOfFrom = bodyInLower.LastIndexOf("from: ", System.StringComparison.Ordinal);
            //find symbol @ in "from: ........ @...."
            var indexOfAt = bodyInLower.IndexOf("@", indexOfFrom, System.StringComparison.Ordinal);

            stringBuilderForMailAddress.Append("@");

            //append to mail address everything that lefter of @
            for (var i = indexOfAt - 1; i > 0; i--)
            {
                if (Char.IsLetterOrDigit(bodyInLower[i]))
                {
                    stringBuilderForMailAddress.Insert(0, bodyInLower[i]);
                }
                else
                {
                    break;
                }
            }

            //append to mail address everything that righter of @
            for (var i = indexOfAt + 1; i < bodyInLower.Length; i++)
            {
                if (Char.IsLetterOrDigit(bodyInLower[i]) || bodyInLower[i] == '.')
                {
                    stringBuilderForMailAddress.Append(bodyInLower[i]);
                }
                else
                {
                    break;
                }
            }

            //return mail address
            var mailAddress = new MailAddress(stringBuilderForMailAddress.ToString());

            return mailAddress;
        }

        public bool IsForwardedMail(MailMessage mailMessage)
        {
            return mailMessage.Subject.ToLower().StartsWith("fwd:") || mailMessage.Subject.ToLower().StartsWith("fw:");
        }
    }
}
