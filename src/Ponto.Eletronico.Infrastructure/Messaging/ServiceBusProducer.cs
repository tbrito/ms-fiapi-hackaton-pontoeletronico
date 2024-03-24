using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ponto.Eletronico.Core.Interfaces;
using System.Configuration;
using System.Text;

namespace Ponto.Eletronico.Infrastructure.Messaging
{
    public class ServiceBusProducer : IServiceBusProducer
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ServiceBusProducer> logger;
        
        public ServiceBusProducer(
            IConfiguration configuration,
            ILogger<ServiceBusProducer> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
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
            var connectionString = this.configuration.GetConnectionString("ServiceBus");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("ServiceBusSettings.ConnectionString está nulo");
            }


            ServiceBusConnection connection = new ServiceBusConnection(connectionString);
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