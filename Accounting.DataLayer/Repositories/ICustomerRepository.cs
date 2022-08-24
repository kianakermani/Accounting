using Accounting.ViewModels.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetAllCustomers();
        IEnumerable<Customer> GetCustomerByFilter(string parameter);
        List<ListCustomerViewModel> GetNameCustomers(string filter = "");
        Customer GetCustomerById(int CustomerId);
        bool InsertCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        bool DeleteCustomer(int CustomerId);
        int GetCustomerIdByName(string name);
        string GetCustomerNameById(int customerId);

  
        
    }
}
