using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly ICustomerService _service;

    public UpdateCustomerCommandHandler(ICustomerService service)
    {
        _service = service;
    }

    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        await _service.UpdateCustomerAsync(new Customer(command.CustomerId, command.FirstName, command.LastName, command.PhoneNumber,
            command.Email, command.BankAccount, command.DateOfBirth));
    }
}