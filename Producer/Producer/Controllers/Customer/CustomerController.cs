using Microsoft.AspNetCore.Mvc;
using Producer.Application.Interfaces.Customer;
using Producer.Arguments.Response;

namespace Producer.Controllers.Customer;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpGet]
    public IEnumerable<CustomerResponse> Get()
    {
        return _customerService.Get();
    }

    [HttpPut("UpdateCustomerStatus/{id}")]
    public async Task UpdateCustomerStatusRabbitMQ(int id, [FromBody] string status)
    {
        await _customerService.UpdateCustomerStatusRabbitMQ(id, status);
    }
}