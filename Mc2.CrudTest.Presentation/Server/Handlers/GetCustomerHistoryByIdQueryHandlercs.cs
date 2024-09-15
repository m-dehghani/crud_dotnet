using Mc2.CrudTest.Presentation.Server.DomainServices;
using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers
{
    public class GetCustomerHistoryByIdQueryHandlercs(ICustomerService readService)
        : IRequestHandler<GetCustomerHistoryByIdQuery, CustomerHistoryViewModel>
    {
        public async Task<CustomerHistoryViewModel> Handle(GetCustomerHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await readService.GetCustomerHistory(request.Id);
        }
    }
}
