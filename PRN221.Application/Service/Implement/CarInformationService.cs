
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class CarInformationService : ICarInformationService
    {
        private readonly IGenericRepository<CarInformation> _carRepository;

        public CarInformationService(IGenericRepository<CarInformation> carInformationRepository)
        {
            _carRepository = carInformationRepository;
        }


        public IEnumerable<CarInformation> GetAll()
        {
            return _carRepository.GetWithCondition(car => new CarInformation()
            {
                CarId = car.CarId,
                CarName = car.CarName,
                CarStatus = car.CarStatus,
                CarDescription = car.CarDescription,
                CarRentingPricePerDay = car.CarRentingPricePerDay,
                FuelType = car.FuelType,
                NumberOfDoors = car.NumberOfDoors,
                RentingDetails = car.RentingDetails,
                SeatingCapacity = car.SeatingCapacity,
                Supplier = car.Supplier,
                Year = car.Year,
                Manufacturer = car.Manufacturer,
            },
            null,
            car => car.Manufacturer,
            car => car.Supplier);
        }

    }
}
