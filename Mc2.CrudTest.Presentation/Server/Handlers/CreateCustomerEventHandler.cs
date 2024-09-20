using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Commands;
using Mc2.CrudTest.Presentation.Shared.Factories;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>
{
    private readonly ICustomerService _service;
    private readonly ICustomerFactory _customerFactory;
    
    public CreateCustomerCommandHandler(ICustomerService service, ICustomerFactory customerFactory)
    {
        _service = service;
        
        _customerFactory = customerFactory;
    }

    public async Task Handle(CreateCustomerCommand? command, CancellationToken cancellationToken)
    {
        if (command != null)
        {
            await _service.CreateCustomerAsync(

               _customerFactory.CreateCustomer(command?.FirstName, command?.LastName, command?.PhoneNumber,

                    command?.Email, command?.BankAccount, command?.DateOfBirth));
        }
    }
}