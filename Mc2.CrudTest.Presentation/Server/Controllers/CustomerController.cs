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
[Route("[controller]/V1")]
public class CustomerController : Controller
{
    private readonly IMediator _mediator;
    private static readonly ILogger Log = Serilog.Log.ForContext<CustomerController>();
    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

   
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(string id)
    {
        GetCustomerByIdQuery getCustomerByIdQuery = new (Guid.Parse(id)); 
        IActionResult? result = await RequestHandler.HandleQuery(getCustomerByIdQuery, _mediator, Log);
        return result;
    }

    [HttpGet("{id:Guid}/History")]
    public async Task<IActionResult> GetHistory(string id)
    {
        GetCustomerHistoryByIdQuery getCustomerHistoryByIdQuery = new(Guid.Parse(id));
        IActionResult? result = await RequestHandler.HandleQuery(getCustomerHistoryByIdQuery, _mediator, Log);
        return result;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return await RequestHandler.HandleQuery(new GetAllCustomersQuery(), _mediator, Log);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand command)
    {
         return await RequestHandler.HandleCommand(command, _mediator, Log);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(string id, UpdateCustomerCommand customerUpdateCmd)
    {
        if (new Guid(id) != customerUpdateCmd.Id) { return new BadRequestResult(); }
        return await RequestHandler.HandleCommand(customerUpdateCmd, _mediator, Log);
    }

    [HttpDelete("{id}")]
    public async Task DeleteCustomer(string id)
    {
        await RequestHandler.HandleCommand(new DeleteCustomerCommand(Guid.Parse(id)), _mediator, Log);
    }
}
