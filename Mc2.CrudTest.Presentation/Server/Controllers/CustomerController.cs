using Asp.Versioning;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Mc2.CrudTest.Presentation.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : Controller
{
    private IMediator _mediator;
    private static readonly ILogger Log = Serilog.Log.ForContext<CustomerController>();
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [ApiVersion(1.0)]
    [HttpGet("V1/{id:Guid}")]
    public async Task<IActionResult> Get(string id)
    {
        GetCustomerByIdQuery getCustomerByIdQuery = new (Guid.Parse(id)); 
        var result = await RequestHandler.HandleQuery(getCustomerByIdQuery, _mediator, Log);
        return result;
    }

    [ApiVersion(1.0)]
    [HttpGet("V1/{id:Guid}/History")]
    public async Task<IActionResult> GetHistory(GetCustomerHistoryByIdQuery getCustomerHistoryByIdQuery)
    {
        //GetCustomerHistoryByIdQuery getCustomerHistoryByIdQuery = new(Guid.Parse(id));
        var result = await RequestHandler.HandleQuery(getCustomerHistoryByIdQuery, _mediator, Log);
        return result;
    }

    [ApiVersion(1.0)]
    [HttpGet("V1")]
    public async Task<IActionResult> GetAll()
    {
        return await RequestHandler.HandleQuery(new GetAllCustomersQuery(), _mediator, Log);
    }

    [ApiVersion(1.0)]
    [HttpPost("V1")]
    public async Task<IActionResult> CreateCustomer(CustomerViewModel newCustomer)
    {
        CreateCustomerCommand command = new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName,
            newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth);
         return await RequestHandler.HandleCommand(command, _mediator, Log);
    }

    [ApiVersion(1.0)]
    [HttpPut("V1/{id}")]
    public async Task<IActionResult> UdateCustomer(string id, CustomerUpdateViewModel updatedCustomer)
    {
        var customerUpdateCmd = new UpdateCustomerCommand(Guid.Parse(id), updatedCustomer.FirstName,
            updatedCustomer.LastName,
            updatedCustomer.PhoneNumber, updatedCustomer.Email, updatedCustomer.BankAccount,
            updatedCustomer.DateOfBirth);
        return await RequestHandler.HandleCommand(customerUpdateCmd, _mediator, Log);
    }

    [ApiVersion(1.0)]
    [HttpDelete("V1/{id}")]
    public async Task DeleteCustomer(string id)
    {
        await RequestHandler.HandleCommand(new DeleteCustomerCommand(Guid.Parse(id)), _mediator, Log);
    }
}
