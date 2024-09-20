using MediatR;
using Mc2.CrudTest.Presentation.Shared.ViewModels;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>, INotification
{
    public GetAllCustomersQuery()
    {
    }

    // TODO: add some parameters to enable searching and pagination

}