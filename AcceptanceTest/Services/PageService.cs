using AcceptanceTest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.Services
{
    public interface IPageService
    {
        CustomerPage CustomerPage { get; }
    }

    public class PagesService(CustomerPage customerPage) : IPageService
    {
        public CustomerPage CustomerPage { get; } = customerPage;
    }
}
