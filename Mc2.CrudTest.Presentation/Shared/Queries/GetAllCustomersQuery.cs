using Mc2.CrudTest.Presentation.Shared.Entities;

using MediatR;
using System.Diagnostics;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetAllCustomersQuery : IRequest<IEnumerable<ViewModels.CustomerViewModel>>, INotification
{
    public GetAllCustomersQuery()
    {
    }

    // TODO: add some parameters to enable searching and pagination

}