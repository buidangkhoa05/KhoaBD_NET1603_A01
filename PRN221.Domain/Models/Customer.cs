using FluentValidation;
using System;
using System.Collections.Generic;

namespace PRN221.Domain.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Telephone { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? CustomerBirthday { get; set; }

    public byte? CustomerStatus { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<RentingTransaction> RentingTransactions { get; set; } = new List<RentingTransaction>();
}

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        //RuleFor(customer => customer.CustomerId)
        //    .GreaterThan(0).WithMessage("Customer ID must be greater than zero.");

        RuleFor(customer => customer.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .Length(0, 50).WithMessage("Customer name cannot exceed 50 characters.");

        RuleFor(customer => customer.Telephone)
            .NotEmpty().WithMessage("Telephone is required.")
            .MinimumLength(10).WithMessage("Telephone number is invalid length")
            .Length(0, 12).WithMessage("Telephone cannot exceed 12 characters.");

        RuleFor(customer => customer.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Length(0, 50).WithMessage("Email cannot exceed 50 characters.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(customer => customer.CustomerBirthday)
            .Must(BeAValidDate).WithMessage("Customer birthday must be a valid date.");

        RuleFor(customer => customer.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(5).WithMessage("Password must be at least 5 characters long.")
            .Length(0, 50).WithMessage("Email cannot exceed 50 characters.");

        RuleFor(customer => customer.CustomerStatus)
            .Must(BeValidStatus)
            .WithMessage("Please enter a valid customer status (0-255).");
    }


    private bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value.Year >= 1900 && date.Value.Year <= DateTime.Now.Year;
    }

    private bool BeValidStatus(byte? status)
    {
        return status >= 0 && status <= 255;
    }
}