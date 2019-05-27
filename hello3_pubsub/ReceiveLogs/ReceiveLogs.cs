using Newtonsoft.Json;
using pablo.queueing.common;
using pablo.queueing.common.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace ReceiveLogs
{
    class ReceiveLogs
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                var queueName = ParseArgs();
                if (queueName == null)
                {
                    queueName = channel.QueueDeclare().QueueName;
                }
                else
                {
                    channel.QueueDeclare(queue: queueName);
                }

                channel.QueueBind(queue: queueName, exchange: "logs", routingKey: ""); //fanout - routingKey is ignored anyway



                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (sender, e) =>
                {
                    var messageJson = Encoding.UTF8.GetString(e.Body);
                    var bodyObj = JsonConvert.DeserializeObject<MessageBase>(messageJson);

                    ConsoleLogger.Log($"Received '{messageJson}'");
                    Thread.Sleep(5000);
                    ConsoleLogger.Log($"Processed '{messageJson}'");
                    Console.WriteLine("-----------------------------------------------");
                };

                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine("Press any key to terminate ...");
                Console.ReadKey();

                string ParseArgs()
                {
                    if(args.Length == 0)
                    {
                        return null;
                    }

                    return args[0].Trim();
                }

            }

        }
    }
}
