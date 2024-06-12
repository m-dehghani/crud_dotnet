using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class DeleteCustomerCommand : IRequest, INotification
{
    public DeleteCustomerCommand(Guid Id)
    {
        this.Id = Id;
    }
    public Guid Id { get; set; }
}