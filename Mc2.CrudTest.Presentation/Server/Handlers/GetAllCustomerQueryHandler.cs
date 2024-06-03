using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class GetAllCustomerQueryHandler: IRequestHandler<GetAllCustomersQuery, IEnumerable<ViewModels.CustomerViewModel>>
{
    private readonly ICustomerService  _readService;

    public GetAllCustomerQueryHandler(ICustomerService readService)
    {
        _readService = readService;
    }
    public async Task<IEnumerable<ViewModels.CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return (await _readService.GetAllCustomers()).Select(customer => new ViewModels.CustomerViewModel(customer.Id,customer.History.ToArray() ,customer.FirstName, customer.LastName, customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value, customer.DateOfBirth?.Value.ToString()));
    }
}