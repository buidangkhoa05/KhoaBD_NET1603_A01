using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class Manufacturer
{
    public int ManufacturerId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public string? Description { get; set; }

    public string? ManufacturerCountry { get; set; }

    public virtual ICollection<CarInformation> CarInformations { get; set; } = new List<CarInformation>();
}

public class ManufacturerValidator : AbstractValidator<Manufacturer>
{
    public ManufacturerValidator()
    {
        RuleFor(x => x.ManufacturerId)
    .GreaterThanOrEqualTo(-1).WithMessage("Mã nhà sản xuất phải lớn hơn hoặc bằng -1.");

        RuleFor(x => x.ManufacturerName)
            .Length(0, 25).WithMessage("Tên nhà sản xuất không được quá 25 ký tự.");

        RuleFor(x => x.Description)
            .Length(0, 125).WithMessage("Mô tả không được quá 125 ký tự.");

        RuleFor(x => x.ManufacturerCountry)
            .NotEmpty().WithMessage("Quốc gia sản xuất không được để trống.");
    }
}
