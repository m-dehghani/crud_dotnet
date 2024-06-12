using Mc2.CrudTest.Presentation.Shared.Queries;
using Mc2.CrudTest.Presentation.DomainServices;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Server.Handlers
{
    public class GetCustomerHistoryByIdQueryHandlercs : IRequestHandler<GetCustomerHistoryByIdQuery, CustomerHistoryViewModel>
    {
        private readonly ICustomerService _readService;

        public GetCustomerHistoryByIdQueryHandlercs(ICustomerService readService)
        {
            _readService = readService;
        }
        public async Task<CustomerHistoryViewModel> Handle(GetCustomerHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _readService.GetCustomerHistory(request.Id);
        }
    }
}
