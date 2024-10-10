using FluentAssertions;
using FluentValidation.TestHelper;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Application.Validators;

namespace MicroRabbit.Banking.Application.UnitTest.Validators;

public class AccountTransferValidatorTests : IClassFixture<AccountTransferValidator>
{
    private readonly AccountTransferValidator _validator;

    public AccountTransferValidatorTests(AccountTransferValidator validator)
    {
        _validator = validator;
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.NewGuid(),
            AccountTo = Guid.NewGuid(),
            TransferAmount = 100
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidAccountFromRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.Empty,
            AccountTo = Guid.NewGuid(),
            TransferAmount = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.ShouldHaveValidationErrorFor(x => x.AccountFrom)
            .WithErrorMessage("The 'account from' field cannot be empty");
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidAccountToRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.NewGuid(),
            AccountTo = Guid.Empty,
            TransferAmount = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1);
        result.ShouldHaveValidationErrorFor(x => x.AccountTo)
            .WithErrorMessage("The 'account to' field cannot be empty");
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidTransferAmountRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.NewGuid(),
            AccountTo = Guid.NewGuid(),
            TransferAmount = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(2);
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("The 'transfer amount' field cannot be less than or equal to zero");
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("'Transfer Amount' must not be empty.");
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidAccountFromAndTransferAmountRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.Empty,
            AccountTo = Guid.NewGuid(),
            TransferAmount = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.ShouldHaveValidationErrorFor(x => x.AccountFrom)
            .WithErrorMessage("The 'account from' field cannot be empty");
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("The 'transfer amount' field cannot be less than or equal to zero");
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("'Transfer Amount' must not be empty.");
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidAccountToAndTransferAmountRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.NewGuid(),
            AccountTo = Guid.Empty,
            TransferAmount = 0
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.ShouldHaveValidationErrorFor(x => x.AccountTo)
            .WithErrorMessage("The 'account to' field cannot be empty");
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("The 'transfer amount' field cannot be less than or equal to zero");
        result.ShouldHaveValidationErrorFor(x => x.TransferAmount)
            .WithErrorMessage("'Transfer Amount' must not be empty.");
    }

    [Fact]
    public async Task IsAccountTransferRequestValid_WithInvalidAccountFromAndAccountToAndTransferAmountRequest_ReturnsFailure()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.Empty,
            AccountTo = Guid.Empty,
            TransferAmount = 1
        };

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.ShouldHaveValidationErrorFor(x => x.AccountFrom)
            .WithErrorMessage("The 'account from' field cannot be empty");
        result.ShouldHaveValidationErrorFor(x => x.AccountTo)
            .WithErrorMessage("The 'account to' field cannot be empty");
        result.ShouldHaveValidationErrorFor(x => x.AccountFrom)
            .WithErrorMessage("The 'account from' field must be different from the 'account to' field");
    }
}
