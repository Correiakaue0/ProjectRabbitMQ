using Consumer.Response;
using Microsoft.Extensions.Options;
using Producer.Arguments.Settings;
using RabbitMQ.Client;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Consumer.Services;

public class RabbitMqService
{
    private readonly string _filePath;

    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqService(IOptions<RabbitMqSettings> rabbitSettings)
    {
        var rootPath = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent!.Parent!.Parent!.FullName.Replace("\\Consumer", "\\SharedData");
        _filePath = Path.Combine(rootPath!, "customer.json");

        var settings = rabbitSettings.Value;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password
        };
    }

    public async Task ReadMessages()
    {
        Console.WriteLine("Iniciando Consumer com while(true)...");

        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "update-status-customer",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        Console.WriteLine("Aguardando mensagens...");

        while (true)
        {
            var result = await channel.BasicGetAsync(queue: "update-status-customer", autoAck: false);

            if (result != null)
            {
                var body = result.Body.ToArray();
                var messageObject = JsonSerializer.Deserialize<CustomerResponseRabbit>(Encoding.UTF8.GetString(body))!;

                Console.WriteLine($"Mensagem recebida: {messageObject}");

                await SetStatusCustomer(messageObject);

                await Task.Delay(500);
                await channel.BasicAckAsync(result.DeliveryTag, false);
            }

            await Task.Delay(1000);
        }
    }

    private async Task SetStatusCustomer(CustomerResponseRabbit messageObject)
    {
        var json = await File.ReadAllTextAsync(_filePath);
        var listCustomer = JsonSerializer.Deserialize<List<CustomerResponse>>(json)!;

        var customer = listCustomer.FirstOrDefault(x => x.Id == messageObject.Id);
        if (customer != null)
            customer.Status = messageObject.Status;
        
        var jsonUpdated = JsonSerializer.Serialize(listCustomer, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });

        await File.WriteAllTextAsync(_filePath, jsonUpdated);
    }
}