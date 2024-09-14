using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers;

public class GetAllCustomerQueryHandler: IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerViewModel>>
{
    private readonly ICustomerService  _readService;

    public GetAllCustomerQueryHandler(ICustomerService readService)
    {
        _readService = readService;
    }
    public async Task<IEnumerable<CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return (await _readService.GetAllCustomers()).Select(customer => new CustomerViewModel(customer.Id,customer.History.ToArray() ,customer.FirstName, customer.LastName, customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value, customer.DateOfBirth?.Value.ToString()));
    }
}