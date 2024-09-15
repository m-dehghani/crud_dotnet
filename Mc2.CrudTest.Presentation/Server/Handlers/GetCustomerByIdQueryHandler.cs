using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class GetCustomerByIdQueryHandler(ICustomerService readService)
    : IRequestHandler<GetCustomerByIdQuery, CustomerViewModel>
{
    public async Task<CustomerViewModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await readService.GetCustomer(request.Id);
        return new CustomerViewModel(customer.Id,customer.History.ToArray(), customer.FirstName, customer.LastName, customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value, customer.DateOfBirth?.Value.ToString());
    }
}