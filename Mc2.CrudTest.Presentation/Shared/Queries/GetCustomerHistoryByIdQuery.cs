using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Queries
{
    public class GetCustomerHistoryByIdQuery : IRequest<CustomerHistoryViewModel>, INotification

    {
        public GetCustomerHistoryByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
        public Guid Id { get; set; }
    }
}
