using System.Threading.Tasks;

namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(BaseMessage baseMessage,string topicName);

    }
}
