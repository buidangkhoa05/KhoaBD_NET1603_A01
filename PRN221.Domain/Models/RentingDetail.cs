using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class RentingDetail
{
    public int RentingTransactionId { get; set; }

    public int CarId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal? Price { get; set; }

    public virtual CarInformation Car { get; set; } = null!;

    public virtual RentingTransaction RentingTransaction { get; set; } = null!;
}


public class RetingDetailValidator : AbstractValidator<RentingDetail>
{
    public RetingDetailValidator()
    {
        RuleFor(x => x.RentingTransactionId)
    .GreaterThanOrEqualTo(-1).WithMessage("Mã giao dịch thuê xe phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.CarId)
            .GreaterThanOrEqualTo(-1).WithMessage("Mã xe phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Ngày bắt đầu không được để trống.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Ngày kết thúc không được để trống.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Giá thuê không được để trống.");

    }
}
