using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Interface
{
    public interface IAuthenService
    {
        (bool,string) Login(string mail, string password, out bool isAdmin);
    }
}
