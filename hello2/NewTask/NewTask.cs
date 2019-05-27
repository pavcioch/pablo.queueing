using Newtonsoft.Json;
using pablo.queueing.common;
using pablo.queueing.common.Model;
using RabbitMQ.Client;
using System;
using System.Text;

namespace NewTask
{
    class NewTask
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "task_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.ExchangeDeclare(
                    exchange: "task_exchange",
                    type: "direct"
                );
                
                var parsedArgs = ParseArgs();
                var messageObject = new MessageAck
                {
                    Payload = parsedArgs.Payload,
                    Acknowledge = parsedArgs.Acknowledge
                };
                var messageJson = JsonConvert.SerializeObject(messageObject);

                var body = Encoding.UTF8.GetBytes(messageJson);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                ConsoleLogger.Log($"start BasicPublish '{messageJson}'");

                channel.BasicPublish(
                    exchange: "task_exchange",
                    routingKey: "task",
                    basicProperties: properties,
                    body: body);

                ConsoleLogger.Log($"end BasicPublish '{messageJson}'");
            }

            (string Payload, bool Acknowledge) ParseArgs()
            {
                if (args.Length != 2)
                {
                    throw new Exception($"Expected exactly 2 args, number of args={args.Length}");
                }

                return (
                    Payload: args[0].Trim(),
                    Acknowledge: bool.Parse(args[1])
                );


            }
        }

    
    }
}
