using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerViewModel>, INotification

{
    public GetCustomerByIdQuery(Guid customerId)
    {
        CustomerId = customerId;
    }
    public Guid CustomerId { get; set; }
}