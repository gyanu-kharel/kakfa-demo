using System.Text.Json;
using Bogus;
using Confluent.Kafka;
using Shared;

string topic = "UserSignUp";

var producerConfig = new ProducerConfig()
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<Null, string>(producerConfig)
    .Build();

var message = new Message<Null, string>();


for (int i = 0; i < 50; i++)
{
    var faker = new Faker();
    var user = new UserSignUpMessage(faker.Person.FullName, faker.Person.Email);
    message.Value = JsonSerializer.Serialize(user);

    try
    {
        var producerResult = await producer.ProduceAsync(topic, message);
        Console.WriteLine($"{producerResult.Message.Value}");
    }
    catch (ProduceException<Null, string> exc)
    {
        Console.WriteLine($"Delivery failed: {exc.Error.Reason}");
    }
}