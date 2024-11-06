namespace UserAuthentication.Services
{
    public interface IMessageBusClientRabbitMQProducer
    {
        void SendMessage(string message);
    }
}
