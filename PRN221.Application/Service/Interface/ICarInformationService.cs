using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Interface
{
    public interface ICarInformationService
    {
        IEnumerable<CarInformation> GetAll();
        (bool, string) Create(CarInformation car);
        (bool, string) Delete(int carId);
        (bool, string) Update(CarInformation car);
    }
}
