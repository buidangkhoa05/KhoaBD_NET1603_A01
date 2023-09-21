
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        private CarInformationValidator _carValidator = new CarInformationValidator();

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

        public (bool, string) Create(CarInformation car)
        {
            var (isValid, message) = Validate4Create(car);
            if (isValid)
            {
                try
                {
                    _carRepository.Create(car);
                    var result = _carRepository.SaveChanges();

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

        public (bool, string) Update(CarInformation car)
        {
            var (isValid, message) = Validate4Create(car);
            if (isValid)
            {
                try
                {
                    var resul = _carRepository.Update(ci => ci.CarId == car.CarId,
                         setPropertyCalls: setter
                         => setter.SetProperty(c => c.CarName, car.CarName)
                         .SetProperty(c => c.CarDescription, car.CarDescription)
                         .SetProperty(c => c.NumberOfDoors, car.NumberOfDoors)
                         .SetProperty(c => c.SeatingCapacity, car.SeatingCapacity)
                         .SetProperty(c => c.FuelType, car.FuelType)
                         .SetProperty(c => c.Year, car.Year)
                         .SetProperty(c => c.ManufacturerId, car.ManufacturerId)
                         .SetProperty(c => c.SupplierId, car.SupplierId)
                         .SetProperty(c => c.CarStatus, car.CarStatus)
                         .SetProperty(c => c.CarRentingPricePerDay, car.CarRentingPricePerDay)
                         );

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

        public (bool, string) Validate4Create(CarInformation car)
        {
            if (car == null)
                return (false, "Car is null");

            var result = _carValidator.Validate(car);

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

        public (bool, string) Delete(int carId)
        {
            try
            {
                var (isValid, message) = Validate4Delete(carId);
                if (isValid)
                {
                    _carRepository.Delete(c => c.CarId == carId);
                }
                else
                {
                    _carRepository.Update(c => c.CarId == carId,
                                               setter => setter.SetProperty(c => c.CarStatus, byte.Parse("1")));

                    return (false, "Car is renting");
                }


                return (true, string.Empty);
            }
            catch (Exception)
            {
                return (false, "Delete have error");

            }
        }

        public (bool, string) Validate4Delete(int carId)
        {
            var car = _carRepository.GetWithCondition(car => new CarInformation()
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
            predicate: car => car.CarId == carId,
            includes: car => car.RentingDetails);

            if (car.FirstOrDefault().RentingDetails.Any())
            {
                return (false, "Car is renting");
            }

            return (true, string.Empty);
        }

    }
}
