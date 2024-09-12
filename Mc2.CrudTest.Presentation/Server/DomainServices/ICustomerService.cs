using Mc2.CrudTest.Presentation.Shared.Entities;
using Mc2.CrudTest.Presentation.Shared.ViewModels;

namespace Mc2.CrudTest.Presentation.Server.DomainServices;

public interface ICustomerService
{
    public Task CreateCustomerAsync(Customer customer);
    public Task UpdateCustomerAsync(Customer customer, Guid customerId);
    public Task DeleteCustomerAsync(Guid customerId);
    public Task<Customer> GetCustomer(Guid customerId);
    public Task<IEnumerable<Customer>> GetAllCustomers();
    public Task<CustomerHistoryViewModel> GetCustomerHistory(Guid customerId);
}