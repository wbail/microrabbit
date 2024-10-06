using FluentValidation;
using FluentValidation.Results;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;

namespace MicroRabbit.Banking.Application.Validators;

public class AccountTransferValidator : AbstractValidator<AccountTransferRequest>, IAccountTransferValidator
{
    public AccountTransferValidator()
    {
        RuleFor(x => x.AccountFrom)
            .NotEmpty()
            .WithMessage("The 'account from' field cannot be empty");

        RuleFor(x => x.AccountTo)
            .NotEmpty()
            .WithMessage("The 'account to' field cannot be empty");

        RuleFor(x => x.TransferAmount)
            .NotEmpty()
            .LessThanOrEqualTo(0)
            .WithMessage("The 'transfer amount' field cannot be less than or equal to zero");
    }

    public async Task<ValidationResult> IsAccountTransferRequestValid(AccountTransferRequest request)
    {
        return await ValidateAsync(request);
    }
}
