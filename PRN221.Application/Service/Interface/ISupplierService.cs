using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Interface
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
    }
}
