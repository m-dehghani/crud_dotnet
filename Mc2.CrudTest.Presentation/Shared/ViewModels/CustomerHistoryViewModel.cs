using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;

namespace Mc2.CrudTest.Presentation.Shared.ViewModels
{
    public class CustomerHistoryViewModel
    {
        public CustomerHistoryViewModel(List<EventBase> events)
        {
            events.ForEach(e => Apply(e));
        }
        public CustomerHistoryViewModel()
        {
            
        }
        public void Apply(EventBase @event)
        {
            Customer customer = new();
            customer.Apply(@event);
            EventTypes.Add(@event.GetType().Name);
            FirstNames.Add(customer.FirstName);
             LastNames.Add(customer.LastName);
             BankAccounts.Add(customer.BankAccount.Value);
            DateOfBirths.Add(customer.DateOfBirth.Value);
            PhoneNumbers.Add(customer.PhoneNumber.Value);
            Emails.Add(customer.Email.Value);
            OccurredOn.Add(@event.OccurredOn);
            IsDeleted = customer.IsDeleted;
            Version += 1;
        }
        
        public Guid Id { get; set; }

        public List<string> EventTypes { get; set; } = [];
        public List<DateTimeOffset> OccurredOn { get; set; } = [];

        public List<string> FirstNames { get;  set; } = [];
        
        public List<string> LastNames { get;  set; } = [];
        
        public List<string> BankAccounts { get;  set; } = [];
        
        public List<DateOnly> DateOfBirths { get;  set; } = [];
        
        public List<string> PhoneNumbers { get;  set; } = [];
        
        public List<string> Emails { get;  set; } = [];

        
        
        public int Version { get;  set; } = 0;

        public bool IsDeleted { get; set; } = false;
    }
}
