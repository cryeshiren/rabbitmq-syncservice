namespace GrapeLEAF.DataSyncService
{
    public interface IConsumerClientFactory
    {
        IConsumerClient Create(string queueName, string exchangeName = null);
    }
}
