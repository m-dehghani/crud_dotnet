using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Shared.ViewModels
{
    public class CustomerHistoryViewModel
    {
        public CustomerHistoryViewModel(List<EventBase> events)
        {
            events.ForEach(e => Apply(e));
        }
        
        public void Apply(EventBase @event)
        {
            Customer customer = new();
            customer.Apply(@event);
            FirstName.Add(customer.FirstName);
            LastName.Add(customer.LastName);
            BankAccount.Add(customer.BankAccount);
            DateOfBirth.Add(customer.DateOfBirth);
            PhoneNumber.Add(customer.PhoneNumber);
            Email.Add(customer.Email);
            IsDeleted = customer.IsDeleted;
            Version += 1;
        }
        
        public Guid Id { get; set; }
        
        public List<string> FirstName { get; private set; } = new List<string>();
        
        public List<string> LastName { get; private set; } = new List<string>();
        
        public List<BankAccount> BankAccount { get; private set; } = new List<BankAccount>();
        
        public List<DateOfBirth> DateOfBirth { get; private set; } = new List<DateOfBirth>();
        
        public List<PhoneNumber> PhoneNumber { get; private set; } = new List<PhoneNumber>();
        
        public List<Email> Email { get; private set; } = new List<Email>();
        
        public int Version { get; private set; } = 0;
        
        public bool IsDeleted { get; private set; }
    }
}
