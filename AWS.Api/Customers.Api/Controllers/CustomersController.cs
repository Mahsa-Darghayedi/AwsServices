using Customers.Application.Domain.Contracts.Services;
using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using Customers.Application.Domain.DTOs.Requests.CustomerUpdate;
using Customers.Application.Domain.DTOs.Responses;
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

    [HttpGet("GetCustomer/{id:int}")]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var dto = await _customerService.GetAsync(id);
        if (dto == null)
            return NotFound();
        return Ok(dto);
    }
    [HttpGet("GetAllCustomers")]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _customerService.GetAllAsync();
        if (result?.Count == 0)
            return NoContent();

        return Ok(result);
    }
    [HttpPut("UpdateCustomer")]
    public async Task<IActionResult> Update([FromBody] CustomerUpdateDto dto)
    {
        var customer = await _customerService.GetAsync(dto.Id);
        if (customer is null)
            return NotFound();

        return Ok(await _customerService.UpdateCustomer(dto));
    }

    [HttpDelete("DeleteCustomer/{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var model = await _customerService.GetAsync(id);
        if (model == null)
            return NotFound();

        return Ok(await _customerService.DeleteAsync(id));
    }

}
