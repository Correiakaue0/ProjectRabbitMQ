using Producer.Application.Interfaces.Customer;
using Producer.Arguments.Response;
using Producer.Arguments.Settings;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Producer.Application.Services.Customer;

public class CustomerService : ICustomerService
{
    private readonly string _filePath;
    private readonly ConnectionFactory _connectionFactory;

    public CustomerService(IOptions<RabbitMqSettings> rabbitSettings)
    {
        var rootPath = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName.Replace("\\Producer", "\\SharedData");
        _filePath = Path.Combine(rootPath!, "customer.json");

        var settings = rabbitSettings.Value;

        _connectionFactory = new ConnectionFactory()
        {
            HostName = settings.HostName,
            UserName = settings.UserName,
            Password = settings.Password
        };
    }

    public List<CustomerResponse> Get()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<CustomerResponse>>(json)!;
    }

    public async Task UpdateCustomerStatusRabbitMQ(int id, string status)
    {
        ValidateCustomer(id);

        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "update-status-customer", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var message = new
        {
            Id = id,
            Status = status
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(exchange: "", routingKey: "update-status-customer", body: body);
    }

    private void ValidateCustomer(int id)
    {
        var customer = (from i in Get() where i.Id == id select i).FirstOrDefault();
        if (customer is null) throw new Exception("Id de cliente invalido.");
    }
}