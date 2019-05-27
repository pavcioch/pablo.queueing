using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using pablo.queueing.common;
using Newtonsoft.Json;
using pablo.queueing.common.Model;

namespace Worker
{
    class Worker
    {
        static void Main(string[] args)
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

                channel.QueueBind("task_queue", "task_exchange", "task");

                channel.BasicQos(
                    prefetchSize: 0,
                    prefetchCount: 1,
                    global: false);

                ConsoleLogger.Log(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var messageJson = Encoding.UTF8.GetString(ea.Body);
                    var bodyObj = JsonConvert.DeserializeObject<MessageAck>(messageJson);

                    ConsoleLogger.Log($"Received '{messageJson}'");

                    int dots = bodyObj.Payload.Split('.').Length - 1;
                    Thread.Sleep(dots * 1000);

                    ConsoleLogger.Log($"Processed '{messageJson}'");

                    if (bodyObj.Acknowledge)
                    {
                        //manual ack - look at 'autoAck: false' in BasicConsume
                        channel.BasicAck(
                            deliveryTag: ea.DeliveryTag,
                            multiple: false);

                        ConsoleLogger.Log($"Acknowledged '{messageJson}'");
                    }

                    Console.WriteLine("-----------------------------------------------");
                };

                channel.BasicConsume(                    
                    queue: "task_queue", 
                    autoAck: false,
                    consumer: consumer
                );

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
