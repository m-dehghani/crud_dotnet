﻿using Mc2.CrudTest.Presentation.Client.Models;
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

        public string GetErrors();
    }
}
