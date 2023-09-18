using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class RentingTransaction
{
    public int RentingTransationId { get; set; }

    public DateTime? RentingDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public int CustomerId { get; set; }

    public byte? RentingStatus { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<RentingDetail> RentingDetails { get; set; } = new List<RentingDetail>();
}


public class RentingTransactionValidator : AbstractValidator<RentingTransaction>
{
    public RentingTransactionValidator()
    {
        RuleFor(x => x.RentingTransationId)
    .GreaterThanOrEqualTo(-1).WithMessage("Mã giao dịch thuê phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.RentingDate)
            .NotEmpty().WithMessage("Ngày thuê không được để trống.");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("Tổng giá thuê không được để trống.");

        RuleFor(x => x.CustomerId)
            .GreaterThanOrEqualTo(-1).WithMessage("Mã khách hàng phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.RentingStatus)
            .NotEmpty().WithMessage("Trạng thái thuê không được để trống.");

    }
}
