using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
{
    private readonly ICustomerService _service;
    public CreateCustomerCommandHandler(ICustomerService service)
    {
        _service = service;
    }

    public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
       
            await _service.CreateCustomerAsync(new Customer(command.FirstName, command.LastName, command.PhoneNumber,
                command.Email, command.BankAccount, command.DateOfBirth));
        
        
    }
}