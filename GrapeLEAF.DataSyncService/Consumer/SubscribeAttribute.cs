using System;

namespace GrapeLEAF.DataSyncService
{
    public class SubscribeAttribute : Attribute
    {
        public SubscribeAttribute(string name, string exchangeName = null, string group = null, int retries = 0)
        {
            Name = name;
            ExchangeName = exchangeName;
            Group = group;
            Retries = retries;
        }

        public string Name { get; }

        public string ExchangeName { get; set; }

        public string Group { get; set; }

        public int Retries { get; set; }
    }
}
