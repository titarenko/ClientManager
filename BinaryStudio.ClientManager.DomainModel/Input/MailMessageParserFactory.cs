﻿using System;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    public class MailMessageParserFactory : IMailMessageParserFactory
    {
        public IMailMessageParser GetMailMessageParser(string userAgent)
        {
            if (userAgent.ToLower().Contains("thunderbird"))
                return new MailMessageParserThunderbird();
            //default
            return new MailMessageParserThunderbird();
        }
    }
}
