using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRN221.Application.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        public CustomerService(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetWithCondition(c => new Customer
            {
                CustomerBirthday = c.CustomerBirthday,
                CustomerId = c.CustomerId,
                CustomerName = c.CustomerName,
                CustomerStatus = c.CustomerStatus,
                Email = c.Email,
                Password = c.Password,
                RentingTransactions = c.RentingTransactions,
                Telephone = c.Telephone
            });
        }

        public int Update(Customer customer)
        {
            return _customerRepository.Update(c => c.Email == customer.Email,
               setter => setter.SetProperty(c => c.CustomerBirthday, customer.CustomerBirthday)
                                 .SetProperty(c => c.CustomerName, customer.CustomerName)
                                 .SetProperty(c => c.CustomerStatus, customer.CustomerStatus)
                                 .SetProperty(c => c.Password, customer.Password)
                                 .SetProperty(c => c.Telephone, customer.Telephone));
        }
    }
}
