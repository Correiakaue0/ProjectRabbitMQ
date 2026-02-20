using Producer.Arguments.Response;

namespace Producer.Application.Interfaces.Customer;

public interface ICustomerService
{
    List<CustomerResponse> Get();
    Task UpdateCustomerStatusRabbitMQ(int id, string status);
}