using System.Net;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mc2.CrudTest.Presentation.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : Controller
{
    private ICustomerService _customerService;
    private IMediator _mediator;
    public CustomerController(ICustomerService customerService, IMediator mediator)
    {
        _customerService = customerService;
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var customer = await _customerService.GetCustomer(id);
            return StatusCode(200, customer);
        }
        catch (Exception ex)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateCustomer(CustomerViewModel newCustomer)
    {
        ActionResult result;
        try
        {
            var cmd = new CreateCustomerCommand(newCustomer.FirstName, newCustomer.LastName, newCustomer.PhoneNumber,
                newCustomer.Email, newCustomer.BankAccount, DateTime.Parse(newCustomer.DateOfBirth));

            await _mediator.Publish(cmd);

            result = new OkResult();
        }
        catch (ArgumentException aex)
        {
            result = BadRequest(aex.Message);
        }
        catch (Exception ex)
        {
            // LogException(e);
            result = StatusCode(500);
        }

        return result;
    }

    [HttpPut]
    public async Task<IActionResult> UdateCustomer(CustomerUpdateViewModel customer)
    {
        ActionResult result;
        try
        {
            var cmd = new UpdateCustomerCommand(customer.Id, customer.FirstName, customer.LastName,
                customer.PhoneNumber,
                customer.Email, customer.BankAccount, DateTime.Parse(customer.DateOfBirth));

            await _mediator.Publish(cmd);
            result = new OkResult();
        }
        catch (ArgumentException aex)
        {
            result = BadRequest(aex.Message);
        }
        catch (Exception ex)
        {
            // LogException(e);
            result = StatusCode(500);
        }

        return result;

    }

    [HttpDelete]
    public async Task DeleteCustomer(Guid id)
    {
        var cmd = new DeleteCustomerCommand(id);
        
        await _mediator.Publish(cmd);
        
    }
}