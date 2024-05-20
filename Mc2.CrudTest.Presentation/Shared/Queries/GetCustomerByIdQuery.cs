using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerReadModel>, INotification, IRequest<Customer>

{
    public Guid CustomerId { get; set; }
}