using System.Text.Json;
using Confluent.Kafka;
using Shared.Constants;
using Shared.Models;

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = Groups.RegistrationWelcomeGroup,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

consumer.Subscribe(Topics.NewUserRegistration);

while (true)
{
    try
    {
        var consumerResult = consumer.Consume();
        var user = JsonSerializer.Deserialize<User>(consumerResult.Message.Value);
        
        // send welcome email to this user
        Console.WriteLine($"Send welcome email to the user: {user?.Email}");
    }
    catch (ConsumeException exc)
    {
        Console.WriteLine($"Error while consuming: {exc.Error.Reason}");
    }
}