
using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class CarInformation
{
    public int CarId { get; set; }

    public string CarName { get; set; } = null!;

    public string? CarDescription { get; set; }

    public int? NumberOfDoors { get; set; }

    public int? SeatingCapacity { get; set; }

    public string? FuelType { get; set; }

    public int? Year { get; set; }

    public int ManufacturerId { get; set; }

    public int SupplierId { get; set; }

    public byte? CarStatus { get; set; }

    public decimal? CarRentingPricePerDay { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual ICollection<RentingDetail> RentingDetails { get; set; } = new List<RentingDetail>();

    public virtual Supplier Supplier { get; set; } = null!;
}

public class CarInformationValidator : AbstractValidator<CarInformation>
{
    public CarInformationValidator()
    {
        RuleFor(x => x.CarId)
        .GreaterThanOrEqualTo(-1).WithMessage("Mã xe phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.CarName)
            .Length(0, 25).WithMessage("Tên xe không được quá 25 ký tự.")
            .NotEmpty().WithMessage("Tên xe phải có");

        RuleFor(x => x.CarDescription)
            .Length(0, 110).WithMessage("Mô tả xe không được quá 110 ký tự.");

        RuleFor(x => x.NumberOfDoors)
            .NotEmpty().WithMessage("Số cửa không được để trống.");

        RuleFor(x => x.SeatingCapacity)
            .NotEmpty().WithMessage("Số chỗ ngồi không được để trống.");

        RuleFor(x => x.FuelType)
            .Length(0, 10).WithMessage("Loại nhiên liệu không được quá 10 ký tự.")
            .NotNull().NotEmpty().WithMessage("Loại nhiên liệu phải có");

        RuleFor(x => x.Year)
            .NotNull().NotEmpty().WithMessage("Năm sản xuất không được để trống.");

        RuleFor(x => x.ManufacturerId)
            .GreaterThanOrEqualTo(-1).WithMessage("Mã nhà sản xuất phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.SupplierId)
            .GreaterThanOrEqualTo(-1).WithMessage("Mã nhà cung cấp phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.CarStatus)
            .NotEmpty().WithMessage("Trạng thái xe không được để trống.");

        RuleFor(x => x.CarRentingPricePerDay)
            .NotEmpty().WithMessage("Giá thuê xe mỗi ngày không được để trống.");
    }
}
