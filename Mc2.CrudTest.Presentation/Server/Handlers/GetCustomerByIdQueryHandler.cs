using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ViewModels.CustomerViewModel>
{
    private readonly ICustomerService  _readService;

    public GetCustomerByIdQueryHandler(ICustomerService readService)
    {
        _readService = readService;
    }
    public async Task<ViewModels.CustomerViewModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _readService.GetCustomer(request.CustomerId);
        return new ViewModels.CustomerViewModel(customer.Id,customer.History.ToArray(), customer.FirstName, customer.LastName, customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value, customer.DateOfBirth?.Value.ToString());
    }
}