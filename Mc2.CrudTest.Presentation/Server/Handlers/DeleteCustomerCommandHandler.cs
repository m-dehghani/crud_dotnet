using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Events;
using MediatR;
namespace Mc2.CrudTest.Presentation.Handlers;

public class DeleteCustomerCommandHandler: IRequestHandler<DeleteCustomerCommand>
{
    private readonly ICustomerService _service;

    public DeleteCustomerCommandHandler(ICustomerService service)
    {
        _service = service;
    }
    
    public async Task Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
    {
        await _service.DeleteCustomerAsync(command.Id);
    }
    
}