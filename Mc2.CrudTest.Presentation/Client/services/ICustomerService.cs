using Mc2.CrudTest.Presentation.Client.Models;
using Mc2.CrudTest.Presentation.Shared.ViewModels;
using Mc2.CrudTest.Presentation.ViewModels;

namespace Mc2.CrudTest.Presentation.Client.services
{
    public interface ICustomerService
    {
        public Task Update(CustomerUpdateModel model);
        
        public Task Create(CustomerCreateModel model);

        public Task<List<CustomerViewModel>> GetAll();

        public Task<CustomerUpdateModel> Get(Guid id);

        public Task Delete(Guid id);

        public Task<CustomerHistoryViewModel> GetCustomerHistory(Guid id);

        public List<string> GetErrors();
    }
}
