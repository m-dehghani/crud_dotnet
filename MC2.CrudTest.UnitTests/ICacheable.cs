using Mc2.CrudTest.Presentation.Shared.Entities;
using StackExchange.Redis;

namespace MC2.CrudTest.UnitTests;
public interface ICacheable
{
    void SetCustomerData(Customer customer);
}

public sealed class RedisCacheHandler : ICacheable
{
    private readonly IConnectionMultiplexer multiplexer;

    public RedisCacheHandler(IConnectionMultiplexer multiplexer)
    {
        this.multiplexer = multiplexer;
    }

    public void SetCustomerData(Customer customer) 
    {
        
        IDatabase? _redisDB = multiplexer.GetDatabase(1);
        string? customerData = $"{customer.FirstName}-{customer.LastName}-{customer.DateOfBirth.Value}";
        if (!string.IsNullOrEmpty(_redisDB.StringGet(customer.Email.Value)))
            throw new ArgumentException("Email address already exists");

        if (!string.IsNullOrEmpty(_redisDB.StringGet(customerData)))
            throw new ArgumentException("This user has registered before");

              
        _redisDB.StringSet(customer.Email.Value, 1);
        _redisDB.StringSet(customerData, 1);
    }
}