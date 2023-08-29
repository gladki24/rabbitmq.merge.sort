// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;

var connectionFactory = new ConnectionFactory
{
    HostName = "127.0.0.1",
    Port = 5552,
    Password = "algo",
    UserName = "algo"
};

using var brokerConnection = connectionFactory.CreateConnection();
using var channel = brokerConnection.CreateModel();

channel.QueueDeclare(
    queue: "hello_world_broker_persistence",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

while (true)
{
    Console.Write("write message: ");
    var userInput = Console.ReadLine();

    if (userInput is null)
    {
        continue;
    }

    if (userInput.Equals(".exit"))
    {
        return 0;
    }

    var body = Encoding.UTF8.GetBytes(userInput);

    var properties = channel.CreateBasicProperties();
    properties.Persistent = true;
    
    channel.BasicPublish(
        exchange: string.Empty,
        routingKey: "hello_world_broker_persistence",
        basicProperties: properties,
        body: body);
}


