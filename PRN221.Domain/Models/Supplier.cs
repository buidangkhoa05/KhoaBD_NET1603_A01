using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? SupplierDescription { get; set; }

    public string? SupplierAddress { get; set; }

    public virtual ICollection<CarInformation> CarInformations { get; set; } = new List<CarInformation>();
}

public class SupplierValidator : AbstractValidator<Supplier>
{
    public SupplierValidator()
    {
        RuleFor(x => x.SupplierId)
    .GreaterThanOrEqualTo(-1).WithMessage("Mã nhà cung cấp phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.SupplierName)
            .Length(0, 25).WithMessage("Tên nhà cung cấp không được quá 25 ký tự.");

        RuleFor(x => x.SupplierDescription)
            .Length(0, 125).WithMessage("Mô tả không được quá 125 ký tự.");

        RuleFor(x => x.SupplierAddress)
            .Length(0, 40).WithMessage("Địa chỉ nhà cung cấp không được quá 40 ký tự.");

    }
}
