using System.Threading.Tasks;

namespace Mango.Sevices.OrderAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
