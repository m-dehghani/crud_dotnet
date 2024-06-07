using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using System.Text.Json.Serialization;
using StackExchange.Redis;
using Mc2.CrudTest.Presentation.Shared.ReadModels;
using System.Reflection.Metadata.Ecma335;


namespace Mc2.CrudTest.Presentation.DomainServices
{
    public class CustomerService: ICustomerService
    {
        private readonly IEventRepository _eventStore; 
        static  IDatabase _redisDB; 
        public CustomerService(IEventRepository eventStore, IDatabase redis)
        {
            _redisDB = redis;
            _eventStore = eventStore;
        }
       
        // Command: Create a new customer
        public async Task CreateCustomerAsync(Customer customer)
        {
            //check for the existence of the Email value in Redis DB. If couldn't find the value for the email so the email is unique else should return an error indicating 'This email has used before'.
            //also check for the combination of the Firstname, Lastname, and DateOfBirth
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }
            var customerCreatedEvent = new CustomerCreatedEvent(customer.Id, customer.FirstName, customer.LastName,
                customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value,
                customer.DateOfBirth.Value)
            {
                
                Data = System.Text.Json.JsonSerializer.Serialize(customer),
                             
            };
          
            customerCreatedEvent.OccurredOn = DateTimeOffset.UtcNow;
          
            await _eventStore.SaveEventAsync(customerCreatedEvent, () => SetCustomerInRedis(customer));

        }


        public static void SetCustomerInRedis(Customer customer)
        {
            var customerData = $"{customer.FirstName}-{customer.LastName}-{customer.DateOfBirth.Value}";

            if (!string.IsNullOrEmpty(_redisDB.StringGet(customer.Email.Value)))
                throw new ArgumentException("This email address was taken by another user. Please select another one ");

            if (!string.IsNullOrEmpty(_redisDB.StringGet(customerData)))
                throw new ArgumentException("This user has registered before");


            _redisDB.StringSet(customer.Email.Value, $"{customer.Id}");

            _redisDB.StringSet(customerData, $"{customer.Id}");
        }
        static bool IsEmailUnique(IDatabase db, string email)
        {
            return !db.SetContains("taken_emails", email);
        }
        // Command: Update an existing customer
        public async Task UpdateCustomerAsync(Customer customer, Guid customerId)
        {
            var customerUpdatedEvent = new CustomerUpdatedEvent(customer.Id, customer.FirstName, customer.LastName, customer.Email.Value, customer.PhoneNumber.Value, customer.BankAccount.Value, customer.DateOfBirth.Value)
            {
                Data = System.Text.Json.JsonSerializer.Serialize(customer),
                AggregateId = customer.Id
            };
            customerUpdatedEvent.OccurredOn = DateTimeOffset.UtcNow;
            await _eventStore.SaveEventAsync(customerUpdatedEvent, () => UpdateCustomerInRedis(customer));
            
        }

        private static void UpdateCustomerInRedis(Customer customer)
        {
            var customerData = $"{customer.FirstName}-{customer.LastName}-{customer.DateOfBirth.Value}";
            var redisEmail = _redisDB.StringGet(customer.Email.Value);
            if (!string.IsNullOrEmpty(redisEmail) && redisEmail != new RedisValue(customer.Id.ToString()))
                throw new ArgumentException("This email address was taken by another user. Please select another one ");

            if (!string.IsNullOrEmpty(_redisDB.StringGet(customerData)) && customerData != new RedisValue(customer.Id.ToString()))
                throw new ArgumentException("This user has registered before");

            _redisDB.StringSet(customer.Email.Value, 1);
            _redisDB.StringSet(customerData, 1);
        }

        // Command: Delete a customer
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            var customerDeletedEvent = new CustomerDeletedEvent(customerId);
            customerDeletedEvent.OccurredOn = DateTimeOffset.UtcNow;
            await _eventStore.SaveEventAsync(customerDeletedEvent, () => {});
        }

        // Query: Get customer by ID
        public async Task<Customer> GetCustomer(Guid customerId)
        {
           
            var events = await _eventStore.GetEventsAsync(customerId);
        
            var customer = new Customer();
            foreach (var @event in events)
            {
                var specific_event = GetEventsFromGenericEvent(@event);
                customer.Apply(specific_event);
            }

            return customer.IsDeleted ? new Customer() : customer;
        }
    
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var events = await _eventStore.GetAllEventsAsync();
            
            var customers = new Dictionary<Guid,Customer>();
            try
            {
                foreach (var @event in events)
                {
                    if (customers.ContainsKey(@event.AggregateId))
                        customers[@event.AggregateId].Apply(@event);
                    else
                    {
                        var customer = new Customer();
                        var specific_event = GetEventsFromGenericEvent(@event);
                        customer.Apply(specific_event);
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

        private EventBase GetEventsFromGenericEvent(CustomerReadModel customerEvent)
        {
            var e = new EventBase();
            switch (customerEvent.EventType)
            {
                case "customer_create":
                    e = new CustomerCreatedEvent(customerEvent.AggregateId, customerEvent.FirstName, customerEvent.LastName, customerEvent.PhoneNumber, customerEvent.Email, customerEvent.BankAccount, customerEvent.DateOfBirth, customerEvent.OccurredOn);
                    break;

                case "customer_update": e = new CustomerUpdatedEvent(customerEvent.AggregateId, customerEvent.FirstName, customerEvent.LastName, customerEvent.Email, customerEvent.PhoneNumber, customerEvent.BankAccount,customerEvent.DateOfBirth, customerEvent.OccurredOn);
                    break;

                case "customer_delete": e = new CustomerDeletedEvent(customerEvent.AggregateId);
                    e.OccurredOn = customerEvent.OccurredOn;
                    break;
            }
          return e;
        }
    }
}