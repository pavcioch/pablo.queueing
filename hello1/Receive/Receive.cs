﻿using Newtonsoft.Json;
using pablo.queueing.common;
using pablo.queueing.common.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace Receive
{
    class Receive
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

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
                    queue: "hello",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine("Press any key to terminate ...");
                Console.ReadKey();
                    
            }
            
        }
    }
}
