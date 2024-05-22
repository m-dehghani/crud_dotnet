using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Queries;
using MediatR;

namespace Mc2.CrudTest.Presentation.Handlers;

public class GetAllCustomerQueryHandler: IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
{
    private readonly ICustomerService  _readService;

    public GetAllCustomerQueryHandler(ICustomerService readService)
    {
        _readService = readService;
    }
    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return  await _readService.GetAllCustomers();
    }
}