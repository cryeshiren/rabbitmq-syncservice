using System;

namespace GrapeLEAF.DataSyncService
{
    public class PublishedMessage
    {
        public PublishedMessage()
        {
            time = DateTime.Now;
        }

        public string mid { get; set; }

        public string exchangeName { get; set; }

        public string queueName { get; set; }

        public string msgBeanJson { get; set; }

        public int consumerNum { get; set; } = 0;

        public int maxNum { get; set; }

        public DateTime timeout { get; set; }

        public DateTime time { get; set; }
    }
}