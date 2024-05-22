using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerReadModel>>, INotification, IRequest<IEnumerable<Customer>>
{
    // TODO: add some parameters to enable searching and pagination
    
}