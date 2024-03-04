using System.Text.Json;
using Confluent.Kafka;
using Shared.Constants;
using Shared.Models;

var registrationConsumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = Groups.RegistrationLoggingGroup,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var paymentConsumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = Groups.PaymentLoggingGroup,
    AutoOffsetReset = AutoOffsetReset.Earliest
};

var registrationConsumer = new ConsumerBuilder<Ignore, string>(registrationConsumerConfig).Build();
var paymentConsumer = new ConsumerBuilder<Ignore, string>(paymentConsumerConfig).Build();

registrationConsumer.Subscribe(Topics.NewUserRegistration);
paymentConsumer.Subscribe(Topics.NewPaymentProcessed);

while (true)
{
    try
    {
        var registrationConsumerResult = registrationConsumer.Consume();
        var user = JsonSerializer.Deserialize<User>(registrationConsumerResult.Message.Value);
        
        // persist this as an event to the database
        var registrationEvent = new EventLog()
        {
            Id = Guid.NewGuid(),
            Data = registrationConsumerResult.Message.Value,
            EventType = EventType.NewUserRegistered
        };
        Console.WriteLine($"Logging user registration: {user?.Email}");
        

        var paymentConsumerResult = paymentConsumer.Consume();
        var payment = JsonSerializer.Deserialize<Payment>(paymentConsumerResult.Message.Value);
        
        // persist this as an event to the database
        var paymentEvent = new EventLog()
        {
            Id = Guid.NewGuid(),
            Data = paymentConsumerResult.Message.Value,
            EventType = EventType.NewPaymentProcessed
        };
        Console.WriteLine($"Logging payment process: {payment?.Id}");

    }
    catch (ConsumeException exc)
    {
        Console.WriteLine($"Error while consuming: {exc.Error.Reason}");
    }
}