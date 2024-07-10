using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using Mc2.CrudTest.Presentation.Shared.Helper;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Mc2.CrudTest.Presentation.DomainServices
{
    public class SlowerCustomerService : ICustomerService
    {
        private readonly IEventRepository _eventStore;
        JsonSerializerOptions _options;
        public SlowerCustomerService(IEventRepository eventStore)
        {
            _eventStore = eventStore;
            
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _options.Converters.Add(new DateOnlyJsonConverter());
        }

        private async Task<(Boolean, Boolean)> CheckUniqueness(Customer customer)
        {
            var emailUnique = false;

            var dataUnique = false;

            var customers = await GetAllCustomers();
            var tempCustomer = customers.FirstOrDefault(c => c.Email == customer.Email && !c.IsDeleted);
            if (tempCustomer == null  || tempCustomer.Id == customer.Id)
            {
                emailUnique = true;
            }
            
            tempCustomer = customers.FirstOrDefault(c => c.FirstName == customer.FirstName && c.LastName == customer.LastName && c.DateOfBirth == customer.DateOfBirth);
            if (tempCustomer == null || tempCustomer.Id == customer.Id)
                dataUnique = true;

            return (emailUnique, dataUnique);
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            var isUnique = await CheckUniqueness(customer);

            if (!isUnique.Item1)
                throw new ArgumentException("This email address was taken by another user. Please select another one ");

            if (!isUnique.Item2)
                throw new ArgumentException("This user has registered before");

            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            
             var customerCreatedEvent = new CustomerCreatedEvent(customer.Id, customer.FirstName, customer.LastName,
                customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value,
                customer.DateOfBirth.Value)
            {
                Data = System.Text.Json.JsonSerializer.Serialize(customer,_options),
            };

            customerCreatedEvent.OccurredOn = DateTimeOffset.UtcNow;

            await _eventStore.SaveEventAsync(customerCreatedEvent, () => { });
        }
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            var customerDeletedEvent = new CustomerDeletedEvent(customerId);
            customerDeletedEvent.OccurredOn = DateTimeOffset.UtcNow;
            await _eventStore.SaveEventAsync(customerDeletedEvent, () => { });
        }

        // Query: Get customer by ID
        public async Task<Customer> GetCustomer(Guid customerId)
        {

            var events = await _eventStore.GetEventsAsync(customerId);

            var customer = new Customer();
            foreach (var @event in events)
            {
                // var specific_event = GetEventsFromGenericEvent(@event);
                customer.Apply(@event);
            }

            return customer.IsDeleted ? new Customer() : customer;
        }

        public async Task<CustomerHistoryViewModel> GetCustomerHistory(Guid customerId)
        {
            var events = await _eventStore.GetEventsAsync(customerId);
            var history = new CustomerHistoryViewModel(events);
            return history;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customers = new Dictionary<Guid, Customer>();
            try
            {
                var events = await _eventStore.GetAllEvents().ToListAsync();

           
           
                foreach (var @event in events)
                {
                    if (customers.ContainsKey(@event.AggregateId))
                        customers[@event.AggregateId].Apply(@event);
                    else
                    {
                        var customer = new Customer();
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
            var isUnique = await CheckUniqueness(customer);

            if (!isUnique.Item1)
                throw new ArgumentException("201");

            if (!isUnique.Item2)
                throw new ArgumentException("202");
            var customerUpdatedEvent = new CustomerUpdatedEvent(customer.Id, customer.FirstName, customer.LastName, customer.Email.Value, customer.PhoneNumber.Value, customer.BankAccount.Value, customer.DateOfBirth.Value)
            {
                Data = System.Text.Json.JsonSerializer.Serialize(customer, _options),
                AggregateId = customer.Id
            };
            customerUpdatedEvent.OccurredOn = DateTimeOffset.UtcNow;
            await _eventStore.SaveEventAsync(customerUpdatedEvent, () => { });

        }

    }
}
