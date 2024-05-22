using System.Diagnostics;
using Mc2.CrudTest.Presentation.Infrastructure;
using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.Events;
using Newtonsoft.Json;
using StackExchange.Redis;


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
            //check for the existence of the Email value in Redis DB. If it couldn't find the value for the email so the email is unique else should return an error indicating 'This email has used before'.
            //also check for the combination of the Firstname, Lastname, and DateOfBirth
            try
            {
                
                var customerCreatedEvent = new CustomerCreatedEvent(customer.Id, customer.FirstName, customer.LastName,
                    customer.PhoneNumber.Value, customer.Email.Value, customer.BankAccount.Value,
                    customer.DateOfBirth.Value)
                {
                    Data = JsonConvert.SerializeObject(customer)
                };
                
                await _eventStore.SaveEventAsync(customerCreatedEvent, () => SetCustomerInRedis(customer));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void SetCustomerInRedis(Customer customer) 
        {
            var customerData = $"{customer.Id}-{customer.FirstName}-{customer.LastName}-{customer.DateOfBirth.Value}";
            if (!string.IsNullOrEmpty(_redisDB.StringGet(customer.Email.Value)))
                throw new ArgumentException("email is not unique");

            if (!string.IsNullOrEmpty(_redisDB.StringGet(customerData)))
                throw new ArgumentException("This user has registered before");

              
            _redisDB.StringSet(customer.Email.Value, 1);
            _redisDB.StringSet(customerData, 1);
        }

        // Command: Update an existing customer
        public async Task UpdateCustomerAsync(Customer customer)
        {
            var customerUpdatedEvent = new CustomerUpdatedEvent(customer.Id, customer.FirstName, customer.LastName, customer.PhoneNumber.Value, customer.Email.Value,customer.BankAccount.Value, customer.DateOfBirth.Value)
            {
                Data = JsonConvert.SerializeObject(customer),
                AggregateId = customer.Id
            };
            
            await _eventStore.SaveEventAsync(customerUpdatedEvent, () => UpdateCustomerInRedis(customer));
            
        }

        private static void UpdateCustomerInRedis(Customer customer)
        {
            var customerData = $"{customer.Id}-{customer.FirstName}-{customer.LastName}-{customer.DateOfBirth.Value}";
            if (! string.IsNullOrEmpty(_redisDB.StringGet(customer.Email.Value) ))
                throw new ArgumentException("email is not unique");
            
            if(!string.IsNullOrEmpty(_redisDB.StringGet(customerData)))
                throw new ArgumentException("This user has registered before");
          
            _redisDB.StringSet(customer.Email.Value, 1);
            _redisDB.StringSet(customerData, 1);
        }

        // Command: Delete a customer
        public async Task DeleteCustomerAsync(Guid customerId)
        {
            var customerDeletedEvent = new CustomerDeletedEvent(customerId);
            await _eventStore.SaveEventAsync(customerDeletedEvent, () => {});
        }

        // Query: Get customer by ID
        public async Task<Customer> GetCustomer(Guid customerId)
        {
           
            var events = await _eventStore.GetEventsAsync(customerId);
        
            var customer = new Customer();
            foreach (var @event in events)
            {
               customer.Apply(@event);
            }

            return customer;
        }
    
        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var events = await _eventStore.GetAllEventsAsync();
            
            var customers = new Dictionary<Guid,Customer>();
            foreach (var @event in events)
            {
                if(customers.ContainsKey(@event.AggregateId))
                    customers[@event.AggregateId].Apply(@event);
                else
                {
                    customers.Add(@event.AggregateId,JsonConvert.DeserializeObject<Customer>(@event.Data));
                }
            }

            return customers.Values;
        }
    }
}