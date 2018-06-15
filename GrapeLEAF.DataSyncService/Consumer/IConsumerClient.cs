using System;
using System.Threading;

namespace GrapeLEAF.DataSyncService
{
    public interface IConsumerClient : IDisposable
    {
        void Subscribe(string topic);

        void Subscribe(string topic, string group);

        void Subscribe(string topic, int partition);

        void Listening(TimeSpan timeout, CancellationToken cancellationToken);

        void Commit();

        void Reject(bool requeue);

        event EventHandler<Message> MessageReceieved;
    }
}
