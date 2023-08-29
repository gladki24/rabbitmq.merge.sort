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
    queue: "hello_world_broker_persistence",
    durable: true, // messages are persist even if rabbitmq service will be stopped
    exclusive: false,
    autoDelete: false,
    arguments: null);

Console.WriteLine("Waiting for messages from broker: ");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    
    
    // simulate long-run task
    Thread.Sleep(1000);
    Console.WriteLine($"Received message: {message}");
    
    // confirm message has been handled by ack(nowledgement) message send back
    // ack allow to notify rabbitmq about task has been processed sucessfully 
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(
    queue: "hello_world_broker_persistence",
    autoAck: false,
    consumer: consumer);
    
Console.WriteLine("Press [enter] to exit");
Console.ReadLine();