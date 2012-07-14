using System;

namespace BinaryStudio.ClientManager.DomainModel.Infrastructure
{
    public class Clock
    {
        public static DateTime? FreezedTime { get; set; }

        public static DateTime Now
        {
            get { return FreezedTime ?? DateTime.Now; }
        }
    }
}