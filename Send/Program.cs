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
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var closeApp = false;
            Console.WriteLine("----------Sender----------");
            Console.WriteLine("Message format: [Emotion].[Animal] [message]");
            var message = "";

            do
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(message))
                    {
                        channel.ExchangeDeclare(exchange: "topic_example",
                            type: "topic");

                        var messageArray = message.Split(Convert.ToChar(" "));
                        var spaceLocation = message.IndexOf(" ");

                        message = message.Substring(spaceLocation, message.Length - spaceLocation);

                        var routingKey = messageArray[0];

                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "topic_example",
                            routingKey: routingKey,
                            basicProperties: null,
                            body: body);
                        Console.WriteLine(" Sent... {0}", message);
                    }
                }


                message = Console.ReadLine();
                if (string.IsNullOrEmpty(message))
                {
                    closeApp = true;
                }

            } while (!closeApp);

            Environment.Exit(0);
        }
    }
}
