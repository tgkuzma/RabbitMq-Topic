using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Send
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var message = "";
            var closeApp = false;
            Console.WriteLine("----------Sender----------");
            Console.WriteLine("Write a message to send...");
            Console.WriteLine("...or [Enter] to exit");

            do
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_example",
                        type: "topic");

                    var routingKey = GetRoutingKeyFromMessage(message);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_example",
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);
                    Console.WriteLine(" Sent... {0}", message);
                }


                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    closeApp = true;
                }

            } while (!closeApp);

            Environment.Exit(0);
        }

        private static string GetRoutingKeyFromMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
