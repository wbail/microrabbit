using FluentValidation.Results;
using MicroRabbit.Banking.Application.Models;

namespace MicroRabbit.Banking.Application.Interfaces;

public interface IAccountTransferValidator
{
    Task<ValidationResult> IsAccountTransferRequestValid(AccountTransferRequest request);
}
