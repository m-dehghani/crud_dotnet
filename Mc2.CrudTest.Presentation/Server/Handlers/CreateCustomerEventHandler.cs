using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

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