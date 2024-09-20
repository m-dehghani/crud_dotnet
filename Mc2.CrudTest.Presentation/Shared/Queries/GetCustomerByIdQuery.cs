using Mc2.CrudTest.Presentation.Shared.ViewModels;
using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Queries;

public class GetCustomerByIdQuery : IRequest<CustomerViewModel>, INotification

{
    public GetCustomerByIdQuery(Guid Id)
    {
        this.Id = Id;
    }
    public Guid Id { get; set; }
}