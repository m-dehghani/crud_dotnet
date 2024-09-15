using System.Text.Json;
using Confluent.Kafka;
using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.DomainExceptions;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.Helper;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Presentation.Server.DomainServices
{
    public class SlowerCustomerService : ICustomerService
    {
        private readonly IEventRepository _eventStore;
        
        private readonly JsonSerializerOptions _options;
        
        public SlowerCustomerService
            (IEventRepository eventStore)
        {
            
            _eventStore = eventStore;
           
            _options = new JsonSerializerOptions
            {
                
                PropertyNameCaseInsensitive = true
                
            };
            
            _options.Converters.Add(new DateOnlyJsonConverter());
        }

        private async Task<(bool, bool)> CheckUniqueness(Customer customer)
        {
            bool emailUnique = false;

            bool dataUnique = false;

            IEnumerable<Customer> customers = await GetAllCustomers();
            
            IEnumerable<Customer> enumerable = customers.ToList();
            
            Customer? tempCustomer = enumerable
                .FirstOrDefault(c => c.Email == customer.Email && !c.IsDeleted);
            
            if (tempCustomer == null  || tempCustomer.Id == customer.Id)
            {
                emailUnique = true;
            }
            
            tempCustomer = enumerable
                .FirstOrDefault(c => c.FirstName == customer.FirstName && 
                                     c.LastName == customer.LastName && 
                                     c.DateOfBirth == customer.DateOfBirth);

            if (tempCustomer == null || tempCustomer.Id == customer.Id)
            {
                dataUnique = true;
            }

            return (emailUnique, dataUnique);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            (bool emailIsUnique, bool firstAndLastNameISUnique) isUnique = await CheckUniqueness(customer);

            if (!isUnique.emailIsUnique)
            {
                throw new DuplicateEmailException("This email address was taken" +
                                                  " by another user. Please select another one ", email: customer.Email.Value);
            }

            if (!isUnique.firstAndLastNameISUnique)
            {
                throw new DuplicatedFirstnameAndLastnameException("This user has registered before");
            }

            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            
            CustomerCreatedEvent customerCreatedEvent = 
                new(customer.Id, customer.FirstName, customer.LastName,
                customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value,
                customer.DateOfBirth.Value)
            {
                Data = JsonSerializer.Serialize(customer,_options),
         
                OccurredOn = DateTimeOffset.UtcNow
            };

            await _eventStore.SaveEventAsync(customerCreatedEvent, () => { });
        }
        
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            CustomerDeletedEvent customerDeletedEvent = new(customerId)
            {
                OccurredOn = DateTimeOffset.UtcNow
            };

            await _eventStore.SaveEventAsync(customerDeletedEvent, () => { });
        }

        // Query: Get customer by ID
        public async Task<Customer> GetCustomer(Guid customerId)
        {

            List<EventBase> events = await _eventStore.GetEventsAsync(customerId);

            Customer customer = new();
         
            foreach (EventBase? @event in events)
            {
                customer.Apply(@event);
            }

            return customer.IsDeleted ? new Customer() : customer;
        }

        public async Task<CustomerHistoryViewModel> GetCustomerHistory(Guid customerId)
        {
            List<EventBase> events = await _eventStore.GetEventsAsync(customerId);
          
            CustomerHistoryViewModel history = new(events);
         
            return history;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            Dictionary<Guid, Customer> customers = new();

            try
            {
                List<EventBase> events =
                    await _eventStore.GetAllEvents().ToListAsync();

                foreach (EventBase? @event in events)
                {
                    if (customers.TryGetValue(@event.AggregateId, out Customer? value))
                    {
                        value.Apply(@event);
                    }

                    else
                    {
                        Customer customer = new();

                        customer.Apply(@event);

                        customers.Add(@event.AggregateId, customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return customers.Values.Where(customer => !customer.IsDeleted);
        }

        public async Task UpdateCustomerAsync(Customer customer, Guid customerId)
        {
            customer.Id = customerId;
         
            (bool, bool) isUnique = await CheckUniqueness(customer);

            if (!isUnique.Item1)
            {
                throw new ArgumentException("201");
            }

            if (!isUnique.Item2)
            {
                throw new ArgumentException("202");
            }
            
            CustomerUpdatedEvent customerUpdatedEvent = 
                new(customer.Id, customer.FirstName
                    , customer.LastName, customer.Email.Value, customer.PhoneNumber.Value
                    , customer.BankAccount.Value, customer.DateOfBirth.Value)
            {
                Data = JsonSerializer.Serialize(customer, _options),
                AggregateId = customer.Id,
                OccurredOn = DateTimeOffset.UtcNow
            };
            
            await _eventStore.SaveEventAsync(customerUpdatedEvent, () => { });
        }

    }
}
