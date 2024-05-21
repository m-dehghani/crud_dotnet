using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
{
    private readonly IEventRepository _eventStore;

    public UpdateCustomerCommandHandler(IEventRepository eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var @event = new CustomerUpdatedEvent(command.CustomerId, command.FirstName, command.LastName, command.Email, command.PhoneNumber, command.BankAccount, command.DateOfBirth);
        await _eventStore.SaveEventAsync(@event, () => {});
     }
}