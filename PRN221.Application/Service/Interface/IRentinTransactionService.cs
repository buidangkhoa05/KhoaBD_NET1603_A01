using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN221.Application.Service.Interface
{
    public interface IRentinTransactionService
    {
        IEnumerable<RentingTransaction> GetAll();
        IEnumerable<RentingTransaction> GetAllFullField(int rentingTransId, bool isTracking = false);
        (bool, string) Update(RentingTransaction trans);
        (bool, string) DeleteTransDetail(int rentingTransactionId, int cardId);
        (bool, string) DeleteTrans(int id);
    }
}
