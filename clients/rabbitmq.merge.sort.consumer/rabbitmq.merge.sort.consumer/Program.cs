// See https://aka.ms/new-console-template for more information

using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

Console.WriteLine("Hello, World!");

var connectionFactory = new ConnectionFactory
{
    HostName = "127.0.0.1",
    Port = 5552,
    Password = "algo",
    UserName = "algo",
};

using var connection = connectionFactory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(
    queue: "hello_world_broker",
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null);
    
Console.WriteLine("Waiting for messages from broker: ");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");
};

channel.BasicConsume(
    queue: "hello_world_broker",
    autoAck: true,
    consumer: consumer);
    
Console.Write("Press [enter] to exit");
Console.ReadLine();