using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Factories;
using Mc2.CrudTest.Presentation.Shared.Validators.Abstract;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class UpdateCustomerCommandHandler(ICustomerService service, ICustomerFactory customerFactory) : IRequestHandler<UpdateCustomerCommand>
{
    public async Task Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        
        await service.UpdateCustomerAsync(
            customerFactory.CreateCustomer(command.FirstName, command.LastName, command.PhoneNumber,
            command.Email, command.BankAccount, command.DateOfBirth), command.id);
    }
}