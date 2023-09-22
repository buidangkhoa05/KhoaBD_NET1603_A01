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

        private CustomerValidator _customerValidator;

        private CustomerValidator CustomerValidator => (_customerValidator ??= new CustomerValidator());

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

        public (bool, string) Create(Customer customer)
        {
            var (isValid, message) = Validate4Update(customer);
            if (isValid)
            {
                try
                {
                    _customerRepository.Create(customer);
                    var result = _customerRepository.SaveChanges();

                    return (true, string.Empty);
                }
                catch (Exception)
                {
                    return (false, "Create have error");

                }
            }
            else
            {
                return (false, message);
            }
        }

        public (bool, string) Delete(int customerId)
        {
            try
            {
                var resutl = _customerRepository.Delete(c => c.CustomerId == customerId);
                return (true, string.Empty);
            }
            catch (Exception)
            {
                return (false, "Delete have error");

            }
        }

        public (bool, string) Update(Customer customer)
        {
            var (isValid, message) = Validate4Update(customer);
            if (isValid)
            {
                try
                {
                    var resultUpdate = _customerRepository.Update(c => c.Email == customer.Email,
                        setter => setter.SetProperty(c => c.CustomerBirthday, customer.CustomerBirthday)
                                .SetProperty(c => c.CustomerName, customer.CustomerName)
                                .SetProperty(c => c.CustomerStatus, customer.CustomerStatus)
                                .SetProperty(c => c.Password, customer.Password)
                                .SetProperty(c => c.Telephone, customer.Telephone));

                    return (true, string.Empty);
                }
                catch (Exception)
                {
                    return (false, "Update have error");

                }
            }
            else
            {
                return (false, message);
            }

        }

        public (bool, string) Validate4Update(Customer customer)
        {
            if (customer.Email == AppConfig.Admin.Email || HasMailAddressExist(customer))
            {
                return (false, "Email is invalid.");
            }

            var result = CustomerValidator.Validate(customer);

            if (result.IsValid)
            {
                return (true, "Validation succeeded.");
            }
            else
            {
                // Gather error messages into a single string
                var errorMessage = string.Join("\n", result.Errors.Select(error => error.ErrorMessage));
                return (false, errorMessage);
            }
        }

        public bool HasMailAddressExist(Customer customer)
        {
            return _customerRepository.GetWithCondition(c => new Customer
            {
                CustomerId = c.CustomerId
            }, true,
                     c => c.CustomerId != customer.CustomerId
                         && c.Email == customer.Email).Any();
        }

        public Customer GetById(int customerId)
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
            }, false, c => c.CustomerId == customerId, c => c.RentingTransactions).FirstOrDefault();
        }
    }
}
