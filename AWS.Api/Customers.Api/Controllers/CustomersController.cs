using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;

using Microsoft.AspNetCore.Mvc;

namespace Customers.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("CreateCustomer")]
    public async Task<IActionResult> Create([FromBody] CustomerRequestDto request)
    {
        return Ok(await _customerService.CreateAsync(request));
    }

    [HttpGet("GetCustomer")]
    public async Task<IActionResult> Get(int id)
    {
      var dto = await _customerService.GetAsync(id);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }
}
