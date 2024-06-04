using Mc2.CrudTest.Presentation.ViewModels;

using MediatR;
using System.Diagnostics;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>, INotification
{
    public GetAllCustomersQuery()
    {
    }

    // TODO: add some parameters to enable searching and pagination

}