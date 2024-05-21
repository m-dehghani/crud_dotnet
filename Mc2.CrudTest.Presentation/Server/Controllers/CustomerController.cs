using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Mc2.CrudTest.Presentation.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : Controller
{
    private ICustomerService _customerService;
    private IMediator _mediator;
    private static readonly ILogger Log = Serilog.Log.ForContext<CustomerController>();
    public CustomerController(ICustomerService customerService, IMediator mediator)
    {
        _customerService = customerService;
        _mediator = mediator;
    }
    
    [HttpGet]
    public   Task<IActionResult> Get(GetCustomerByIdQuery  getCustomerByIdQuery)
    =>  RequestHandler.HandleQuery<Customer>(getCustomerByIdQuery, _mediator, Log);
    

    [HttpPost]
    public Task<IActionResult> CreateCustomer(CreateCustomerCommand newCustomerCmd)
        => RequestHandler.HandleCommand(newCustomerCmd, _mediator, Log);
   

    [HttpPut]
    public Task<IActionResult> UdateCustomer(UpdateCustomerCommand customerUpdateCmd)
        => RequestHandler.HandleCommand(customerUpdateCmd, _mediator, Log);
    
    

    [HttpDelete]
    public Task DeleteCustomer(DeleteCustomerCommand customerDeleteCmd)
        => RequestHandler.HandleCommand(customerDeleteCmd, _mediator, Log);
}
