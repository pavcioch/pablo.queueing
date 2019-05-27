﻿using System;
using System.Text;
using Newtonsoft.Json;
using pablo.queueing.common;
using pablo.queueing.common.Model;
using RabbitMQ.Client;

namespace Send
{
    class Send
    {
        static void Main(string[] args)
        {
            const string DEFAULT_MESSAGE = "Hello!";

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null );

                var parsedArgs = ParseArgs();
                var messageObject = new MessageBase
                {
                    Payload = parsedArgs
                };

                var messageJson = JsonConvert.SerializeObject(messageObject);

                var body = Encoding.UTF8.GetBytes(messageJson);

                ConsoleLogger.Log($"start BasicPublish '{messageJson}'");

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

                ConsoleLogger.Log($"end BasicPublish '{messageJson}'");
            }


            string ParseArgs()
            {
                return args.Length == 0 || string.IsNullOrWhiteSpace(args[0]) ? DEFAULT_MESSAGE : args[0].Trim();                    
            }
        }
    }
}
