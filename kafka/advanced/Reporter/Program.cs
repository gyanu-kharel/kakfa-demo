using System.Text.Json;
using Confluent.Kafka;
using Shared.Constants;
using Shared.Models;

var consumerConfig = new ConsumerConfig()
{
    BootstrapServers = "localhost:9092",
    GroupId = Groups.PaymentReportingGroup,
    AutoOffsetReset = AutoOffsetReset.Earliest
};


var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();

consumer.Subscribe(Topics.NewPaymentProcessed);

while (true)
{
    try
    {
        var consumerResult = consumer.Consume();
        var payment = JsonSerializer.Deserialize<Payment>(consumerResult.Message.Value);
        
        // send the invoice to the user
        Console.Write($"Sending invoice to the user: {payment?.UserId}");
    }
    catch (ConsumeException exc)
    {
        Console.WriteLine($"Error while consuming: {exc.Error.Reason}");
    }
}