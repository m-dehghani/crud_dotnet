using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;

namespace Mc2.CrudTest.Presentation.Shared.ViewModels
{
    public class CustomerHistoryViewModel: BaseViewModel
    {
        public CustomerHistoryViewModel(List<EventBase> events)
        {
            Items = new();
            events.ForEach(e => Apply(e));
        }
        public CustomerHistoryViewModel()
        {
        }
        public void Apply(EventBase @event)
        {
            Customer customer = new();
            customer.Apply(@event);
            CustomerHistoryViewModelItem item = new CustomerHistoryViewModelItem();
            Version += 1;
           
            item.EventType = @event.GetType().Name;
            item.FirstName = customer.FirstName;
            item.LastName = customer.LastName;
            item.BankAccount = customer.BankAccount.Value;
            item.DateOfBirth = customer.DateOfBirth.Value.ToString();
            item.PhoneNumber = customer.PhoneNumber.Value;
            item.Email = customer.Email.Value;
            item.OccurredOn = @event.OccurredOn;
            item.IsDeleted = customer.IsDeleted.ToString();

            Items.Add(item);
        }
       
     
        public List<CustomerHistoryViewModelItem> Items { get; set; } 
        public int Version { get; set; } = 0;
    }

    public class CustomerHistoryViewModelItem
    {
        public Guid Id { get; set; }

        public string EventType { get; set; } 
      
        public DateTimeOffset OccurredOn { get; set; } 

        public string FirstName { get; set; } 

        public string LastName { get; set; }

        public string BankAccount { get; set; }

        public string DateOfBirth { get; set; }

        public string PhoneNumber { get; set; } 

        public string Email { get; set; }



        public string IsDeleted { get; set; } = bool.FalseString;
    }
}
