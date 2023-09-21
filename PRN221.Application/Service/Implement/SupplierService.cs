using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class SupplierService : ISupplierService
    {
        private readonly IGenericRepository<Supplier> _supplierRepository;

        public SupplierService(IGenericRepository<Supplier> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _supplierRepository.GetWithCondition(s => new Supplier
            {
                SupplierId = s.SupplierId,
                SupplierAddress = s.SupplierAddress,
                SupplierName = s.SupplierName,
                SupplierDescription = s.SupplierDescription
            });
        }
    }
}
