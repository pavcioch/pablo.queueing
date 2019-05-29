using Newtonsoft.Json;
using pablo.queueing.common;
using pablo.queueing.common.Model;
using RabbitMQ.Client;
using System;
using System.Text;


namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "direct_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                    );

                channel.ExchangeDeclare(
                    exchange: "direct_exchange",
                    type: "direct"
                    );



            }
        }
    }
}
