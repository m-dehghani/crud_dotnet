using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Entities;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class UpdateCustomerCommandHandler(ICustomerService service) : IRequestHandler<UpdateCustomerCommand>
{
    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        await service.UpdateCustomerAsync(new Customer(command.FirstName, command.LastName, command.PhoneNumber,
            command.Email, command.BankAccount, command.DateOfBirth), command.Id);
    }
}