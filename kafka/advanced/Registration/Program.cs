// Set up producer
// With topic "NewUserRegistration"

using System.Text.Json;
using Confluent.Kafka;
using Registration;
using Shared.Constants;

var producerConfig = new ProducerConfig()
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

var message = new Message<Null, string>();

var users = Iterator.GetUsers();

foreach (var user in users)
{
    message.Value = JsonSerializer.Serialize(user);
    var producerResult = await producer.ProduceAsync(Topics.NewUserRegistration, message);
    
    Console.WriteLine(producerResult.Status);
}


