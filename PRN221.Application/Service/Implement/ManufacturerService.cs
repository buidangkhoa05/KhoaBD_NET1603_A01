using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IGenericRepository<Manufacturer> _manufacturerRepo;

        public ManufacturerService(IGenericRepository<Manufacturer> manufacturerRepo)
        {
            _manufacturerRepo = manufacturerRepo;
        }

        public IEnumerable<Manufacturer> GetAll()
        {
            return _manufacturerRepo.GetWithCondition(
            m => new Manufacturer
            {
                ManufacturerCountry = m.ManufacturerCountry,
                Description = m.Description,
                ManufacturerId = m.ManufacturerId,
                ManufacturerName = m.ManufacturerName, 
            });
        }
    }
}
