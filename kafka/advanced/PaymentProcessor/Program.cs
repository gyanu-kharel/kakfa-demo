using System.Text.Json;
using Confluent.Kafka;
using PaymentProcessor;
using Shared.Constants;
using Shared.Models;

var producerConfig = new ProducerConfig()
{
    BootstrapServers = "localhost:9092"
};

var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

var message = new Message<Null, string>();

var payments = Iterator.GetPayments();

foreach (var payment in payments)
{
    message.Value = JsonSerializer.Serialize(payment);

    var producerResult = await producer.ProduceAsync(Topics.NewPaymentProcessed, message);
    
    Console.WriteLine(producerResult.Status);
}