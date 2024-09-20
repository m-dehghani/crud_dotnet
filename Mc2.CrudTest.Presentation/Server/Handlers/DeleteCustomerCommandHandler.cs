using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class DeleteCustomerCommandHandler(IEventRepository eventStore) : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        CustomerDeletedEvent? @event = new(command.Id);
        await eventStore.SaveEventAsync(@event, () => {});
       
    }
    
}