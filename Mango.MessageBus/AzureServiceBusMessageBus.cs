using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        //put it in appsetting or somewhere else
        private readonly string connectionString = "Endpoint=sb://mangorestauranttry.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RTwzOC07tNS2eUBZeB07vABv7yMNdzMMIg+HI4poysY=";
        public async Task PublishMessage(BaseMessage baseMessage, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicName);
            var message = JsonConvert.SerializeObject(baseMessage);
            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(message))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);

            await client.DisposeAsync();
        }
    }
}
