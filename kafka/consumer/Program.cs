using System.Text.Json;
using Confluent.Kafka;
using Shared;

string topic = "UserSignUp";
string group = "AuthGroup";

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = group,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using var consumer = new ConsumerBuilder<Null, string>(consumerConfig)
    .Build();

consumer.Subscribe(topic);

try
{
    while (true)
    {
        try
        {
            var consumerResult = consumer.Consume();
            var user = JsonSerializer.Deserialize<UserSignUpMessage>(consumerResult.Message.Value);

            Console.WriteLine($"Received user {user.Email}");
        }
        catch (ConsumeException exc)
        {
            Console.WriteLine($"Error while consuming: {exc.Error.Reason}");
            throw;
        }
    }
}
catch (Exception exc)
{
    Console.WriteLine($"Internal Error: {exc.Message}");
}
finally
{
    consumer.Close();
}



