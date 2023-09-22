using LinqKit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PRN221.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Implement
{
    public class RentinTransactionService : IRentinTransactionService
    {
        private readonly IGenericRepository<RentingTransaction> _rentingTransactionRepository;
        private readonly IGenericRepository<RentingDetail> _rentingDetailRepository;
        public RentinTransactionService(IGenericRepository<RentingTransaction> rentingTransactionRepository,
             IGenericRepository<RentingDetail> rentingDetailRepository)
        {
            _rentingTransactionRepository = rentingTransactionRepository;
            _rentingDetailRepository = rentingDetailRepository;
        }

        public IEnumerable<RentingTransaction> GetAll()
        {
            return _rentingTransactionRepository.GetWithCondition(r => new RentingTransaction
            {
                CustomerId = r.CustomerId,
                RentingDate = r.RentingDate,
                RentingStatus = r.RentingStatus,
                RentingTransationId = r.RentingTransationId,
                TotalPrice = r.TotalPrice
            });
        }

        public IEnumerable<RentingTransaction> GetAllFullField(int rentingTransId, bool isTracking = false)
        {
            return _rentingTransactionRepository.GetWithCondition(r => new RentingTransaction
            {
                CustomerId = r.CustomerId,
                RentingDate = r.RentingDate,
                RentingStatus = r.RentingStatus,
                RentingTransationId = r.RentingTransationId,
                TotalPrice = r.TotalPrice,
                Customer = r.Customer,
                RentingDetails = r.RentingDetails
            }, isTracking,
            r => r.RentingTransationId == rentingTransId,
            r => r.Customer,
            r => r.RentingDetails);
        }

        public (bool, string) DeleteTrans(int id)
        {
            try
            {
                _rentingTransactionRepository.Delete(t => t.RentingTransationId == id);
                return (true, "");
            }
            catch (Exception)
            {
                return (false, "Delete error");
            }
        }

        public (bool, string) DeleteTransDetail(int rentingTransactionId, int cardId)
        {
            try
            {
                _rentingDetailRepository.Delete(t => t.RentingTransactionId == rentingTransactionId && t.CarId == cardId);
                return (true, "");
            }
            catch (Exception)
            {
                return (false, "Delete error");
            }
        }

        public (bool, string) Update(RentingTransaction trans)
        {
            var rentingTransValidator = new RentingTransactionValidator();
            var detailTransValdator = new RetingDetailValidator();

            var rentingTransResult = rentingTransValidator.Validate(trans);

            var detailTransResults = trans.RentingDetails.Select(t => detailTransValdator.Validate(t));

            if (rentingTransResult.IsValid && detailTransResults.All(t => t.IsValid))
            {
                try
                {
                    trans.RentingDetails.ForEach(rd => _rentingDetailRepository.Update(c => c.RentingTransactionId == rd.RentingTransactionId && c.CarId == rd.CarId,
                        setter => setter.SetProperty(c => c.Price, rd.Price)
                                        .SetProperty(c => c.StartDate, rd.StartDate)
                                        .SetProperty(c => c.EndDate, rd.EndDate)
                                        .SetProperty(c => c.CarId, rd.CarId)));

                    int resultUpdate = _rentingTransactionRepository.Update(t => t.RentingTransationId == trans.RentingTransationId,
                   setter => setter.SetProperty(c => c.RentingDate, trans.RentingDate)
                                   .SetProperty(c => c.TotalPrice, trans.TotalPrice)
                                   .SetProperty(c => c.RentingStatus, trans.RentingStatus));
                    return (true, "");
                }
                catch (Exception)
                {

                    return (false, "Update error");
                }
            }
            else
            {
                var errorMessage = string.Join("\n", rentingTransResult.Errors.Select(error => error.ErrorMessage));
                errorMessage += "\n" + string.Join("\n", detailTransResults.SelectMany(t => t.Errors).Select(error => error.ErrorMessage));

                return (false, errorMessage);
            }
        }
    }
}
