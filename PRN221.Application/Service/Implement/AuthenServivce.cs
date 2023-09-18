using PRN221.Application.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class AuthenServivce : IAuthenService
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        public AuthenServivce(IGenericRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public (bool, string) Login(string mail, string password, out bool isAdmin)
        {
            var message = ValidateLogin(mail, password, out isAdmin);

            if (!string.IsNullOrEmpty(message))
            {
                return (false, message);
            }

            return (true, null);
        }

        private string ValidateLogin(string mail, string pass, out bool isAdmin)
        {
            isAdmin = false;

            if (string.IsNullOrEmpty(mail.Trim()) || string.IsNullOrEmpty(pass.Trim()))
            {
                return "Vui lòng nhập đầy đủ thông tin";
            }

            if (!IsValidEmail(mail))
            {
                return "Email không hợp lệ";
            }

            isAdmin = mail.Equals(AppConfig.Admin.Email) && pass.Equals(AppConfig.Admin.Password);

            try
            {
                if (!isAdmin)
                {
                    var user = (
                        _customerRepository.GetWithCondition(x => new Customer
                        {
                            CustomerId = x.CustomerId,
                            Email = x.Email,
                            CustomerBirthday = x.CustomerBirthday,
                            CustomerName = x.CustomerName,
                        }, c => c.Email.Equals(mail) && c.Password.Equals(pass), null)
                     ).FirstOrDefault();

                    if (AppConfig.Customer == null && user != null)
                    {
                        AppConfig.Customer = user;
                    }
                    else if (user == null)
                    {
                        return "Tài khoản hoặc mật khẩu khẩu không đúng";
                    }
                }
            }
            catch (Exception)
            {
                return "Have error !!!";
            }

            return null;
        }

        static bool IsValidEmail(string email)
        {
            // Define a regular expression pattern for a simple email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
    }
}
