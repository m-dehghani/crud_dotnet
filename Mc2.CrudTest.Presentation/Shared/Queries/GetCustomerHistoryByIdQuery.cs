using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Mc2.CrudTest.Presentation.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Shared.Queries
{
    public class GetCustomerHistoryByIdQuery : IRequest<CustomerHistoryViewModel>, INotification

    {
        public GetCustomerHistoryByIdQuery(Guid customerId)
        {
            CustomerId = customerId;
        }
        public Guid CustomerId { get; set; }
    }
}
