using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class CreateCustomerEventHandler : IRequestHandler<CreateCustomerCommand>
{
private readonly IEventRepository _eventStore;
private readonly ICustomerService _service;
public CreateCustomerEventHandler(IEventRepository eventStore, ICustomerService service)
{
    _eventStore = eventStore;
    _service = service;
}

public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
{
    //var @event = new CustomerCreatedEvent(command.CustomerId, command.FirstName, command.LastName, command.PhoneNumber, command.Email, command.BankAccount, DateTime.Parse(command.DateOfBirth));

    await _service.CreateCustomerAsync(new Customer(command.CustomerId, command.FirstName, command.LastName, command.PhoneNumber,
        command.Email, command.BankAccount, command.DateOfBirth));

}
}