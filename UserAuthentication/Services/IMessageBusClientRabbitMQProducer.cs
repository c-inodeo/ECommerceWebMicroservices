using UserNotificationMessages.Helpers;

namespace UserAuthentication.Services
{
    public interface IMessageBusClientRabbitMQProducer
    {
        void SendMessage(UserNotifModel message);
    }
}
