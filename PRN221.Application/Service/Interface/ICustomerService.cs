using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Interface
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        (bool, string) Update(Customer customer);
        (bool, string) Create(Customer customer);
        (bool, string) Delete(int customerId);
    }
}
