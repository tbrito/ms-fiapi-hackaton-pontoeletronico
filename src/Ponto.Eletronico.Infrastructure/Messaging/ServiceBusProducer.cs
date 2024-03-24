using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ponto.Eletronico.Core.Interfaces;
using System.Text;

namespace Ponto.Eletronico.Infrastructure.Messaging
{
    public class ServiceBusProducer : IServiceBusProducer
    {
        private readonly ILogger<ServiceBusProducer> logger;
        private readonly ServiceBusSettings settings;

        public ServiceBusProducer(
            IOptionsMonitor<ServiceBusSettings> options,
            ILogger<ServiceBusProducer> logger)
        {
            this.logger = logger;
            settings = options.CurrentValue;
        }

        public async Task SendAsync<T>(T message, string topicName)
        {
            Message messageServiceBus = PrepareMessage<T>(message, "adc");

            ITopicClient topicClient = Connect(topicName);

            try
            {
                await topicClient.SendAsync(messageServiceBus);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error ao tentar enviar mensagem para topico {topicName}", ex);
                throw;
            }
        }

        private ITopicClient Connect(string topicName)
        {
            if (settings == null)
            {
                throw new ArgumentException("ServiceBusSettings está nulo");
            }

            if (string.IsNullOrEmpty(settings.ConnectionString))
            {
                throw new ArgumentException("ServiceBusSettings.ConnectionString está nulo");
            }


            ServiceBusConnection connection = new ServiceBusConnection(settings.ConnectionString);
            return new TopicClient(connection, topicName, null);
        }

        private Message PrepareMessage<T>(T message, string label)
        {
            Message messageServiceBus;

            if (message != null)
            {
                var body = Serialize(message);
                messageServiceBus = new Message(body);
            }
            else
            {
                messageServiceBus = new Message();
            }

            messageServiceBus.CorrelationId = Guid.NewGuid().ToString();
            messageServiceBus.MessageId = Guid.NewGuid().ToString();

            if (!string.IsNullOrWhiteSpace(label))
            {
                messageServiceBus.Label = label;
            }

            return messageServiceBus;
        }

        private byte[] Serialize<T>(T message)
        {
            var str = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(str);
        }
    }
}