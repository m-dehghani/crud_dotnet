using MediatR;

namespace Mc2.CrudTest.Presentation.Shared.Commands;

public class DeleteCustomerCommand : IRequest, INotification
{
    public DeleteCustomerCommand(Guid customerId)
    {
        CustomerId = customerId;
    }
    public Guid CustomerId { get; set; }
}