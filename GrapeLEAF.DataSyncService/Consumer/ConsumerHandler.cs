using GrapeLEAF.DataSyncService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrapeLEAF.DataSyncService
{
    public class ConsumerHandler
    {
        private readonly IConsumerClientFactory _consumerClientFactory;
        private readonly MessageServiceOptions _messageServiceOptions;
        private readonly CancellationTokenSource _cts;

        private readonly TimeSpan _pollingDelay = TimeSpan.FromSeconds(1);

        private readonly List<ConsumerExecutorDescriptor> _consumerExecutorDescriptor;

        public ConsumerHandler(IConsumerClientFactory consumerClientFactory,
            MessageServiceOptions messageServiceOptions)
        {
            _cts = new CancellationTokenSource();

            _consumerClientFactory = consumerClientFactory;

            _messageServiceOptions = messageServiceOptions;

            _consumerExecutorDescriptor = _messageServiceOptions._queueServiceMapping.ToDescriptors();
        }

        private Task _compositeTask;

        public void Start()
        {
            var queueGroup = _consumerExecutorDescriptor.GroupBy(d => d.Attribute.Name);

            foreach (var qg in queueGroup)
            {
                Task.Factory.StartNew(() =>
                {
                    using (var client = _consumerClientFactory.Create(qg.Key, qg.First().Attribute.ExchangeName))
                    {
                        HandleMessage(client);

                        foreach (var qgt in qg)
                        {
                            client.Subscribe(qgt.Attribute.ExchangeName, qgt.Attribute.Group);
                        }

                        client.Listening(_pollingDelay, _cts.Token);
                    }
                }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            _compositeTask = Task.CompletedTask;
        }

        private void HandleMessage(IConsumerClient client)
        {
            client.MessageReceieved += (sender, message) =>
            {
                try
                {
                    var receive = message.Content.ToModel<PublishedMessage>();
                    try
                    {
                        DoMessage(message, receive);
                    }
                    catch(Exception e)
                    {
                        receive.consumerNum++;

                        if (receive.consumerNum <= receive.maxNum)
                            client.Reject(true);
                        else
                            client.Commit();
                    }
                }
                catch(Exception e)
                {
                    //log here..
                    client.Commit();
                }
            };
        }

        private MessageResult DoMessage(Message message, PublishedMessage receive)
        {
            var queueExector = _consumerExecutorDescriptor.SingleOrDefault(qd => qd.Attribute.Name == message.Name);

            if (queueExector != null)
            {
                object reflect = Activator.CreateInstance(queueExector.ImplTypeInfo);

                var methodInfo = queueExector.MethodInfo;

                var parameters = methodInfo.GetParameters();

                List<object> objParams = new List<object>();

                objParams = receive.msgBeanJson.ToModel(objParams);

                for (int i = 0; i < parameters.Length; i++)
                {
                    objParams[i] = objParams[i].ToJson().ToModel(parameters[i].ParameterType);
                }

                var result = methodInfo.Invoke(reflect, objParams.ToArray()) as MessageResult;

                return result;
            }
            return MessageResult.Fail(0);
        }
    }
}
