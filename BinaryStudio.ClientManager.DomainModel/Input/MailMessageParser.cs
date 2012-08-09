using System;
using System.Net.Mail;
using System.Text;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParser
    {
        private MailMessage mailMessage;

        public MailMessageParser(MailMessage mailMessage)
        {
            this.mailMessage = mailMessage;
        }

        public MailAddress GetSenderFromForwardedMail()
        {
            this.mailMessage = mailMessage;
            var stringBuilderForMailAddress = new StringBuilder();
            var bodyInLower = mailMessage.Body.ToLower();
            //find in end of body from: 
            var indexOfFrom=bodyInLower.LastIndexOf("from: ", System.StringComparison.Ordinal);
            //find symbol @ in "from: ........ @...."
            var indexOfAt=bodyInLower.IndexOf("@", indexOfFrom, System.StringComparison.Ordinal);

            stringBuilderForMailAddress.Append("@");
            
            //append to mail address everything that lefter of @
            for (var i=indexOfAt-1;i>0;i--)
            {
                if(Char.IsLetterOrDigit(bodyInLower[i]))
                {
                    stringBuilderForMailAddress.Insert(0,bodyInLower[i]);
                }
                else
                {
                    break;
                }
            }

            //append to mail address everything that righter of @
            for (var i = indexOfAt+1; i < bodyInLower.Length; i++)
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

        public bool IsForwardedMail()
        {
            return mailMessage.Subject.ToLower().StartsWith("fwd:") || mailMessage.Subject.ToLower().StartsWith("fw:");
        }
    }
}
