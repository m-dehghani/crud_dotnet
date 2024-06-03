using Asp.Versioning;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
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


    // for testing
    public CustomerController GetInstance()
    {
        return this;
    }


    [ApiVersion(1.0)]
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(string id)
    {
        GetCustomerByIdQuery getCustomerByIdQuery = new (Guid.Parse(id)); 
        var result = await RequestHandler.HandleQuery(getCustomerByIdQuery, _mediator, Log);
        return result;
    }

    [ApiVersion(1.0)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        GetAllCustomersQuery getAllCustomersQuery = new GetAllCustomersQuery(); 
        var result = await RequestHandler.HandleQuery(getAllCustomersQuery, _mediator, Log);
        return result;
    }

    [ApiVersion(1.0)]
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(ViewModels.CustomerViewModel newCustomer)
    {
        CreateCustomerCommand command = new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName,
            newCustomer.PhoneNumber, newCustomer.Email, newCustomer.BankAccount, newCustomer.DateOfBirth);
         return await RequestHandler.HandleCommand(command, _mediator, Log);
    }

    [ApiVersion(1.0)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UdateCustomer(string id, CustomerUpdateViewModel updatedCustomer)
    {
        var customerUpdateCmd = new UpdateCustomerCommand(Guid.Parse(id), updatedCustomer.FirstName,
            updatedCustomer.LastName,
            updatedCustomer.PhoneNumber, updatedCustomer.Email, updatedCustomer.BankAccount,
            updatedCustomer.DateOfBirth);
        return await RequestHandler.HandleCommand(customerUpdateCmd, _mediator, Log);
    }

    [ApiVersion(1.0)]
    [HttpDelete("{id}")]
    public async Task DeleteCustomer(string id)
    {
        DeleteCustomerCommand customerDeleteCmd = new (Guid.Parse(id));
        await RequestHandler.HandleCommand(customerDeleteCmd, _mediator, Log);
    }
}
