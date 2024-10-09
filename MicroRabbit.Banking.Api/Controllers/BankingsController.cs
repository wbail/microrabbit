using FluentValidation;
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAccountService _accountService;
    private readonly IValidator<AccountTransferRequest> _validator;

    public BankingsController(IMediator mediator, IAccountService accountService, IValidator<AccountTransferRequest> validator)
    {
        _mediator = mediator;
        _accountService = accountService;
        _validator = validator;
    }

    [HttpGet]
    public ActionResult<IAsyncEnumerable<Account>> Get()
    {
        return Ok(_accountService.GetAccounts());
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] AccountTransferRequest accountTransfer)
    {
        var isAccountTransferValid = await _validator.ValidateAsync(accountTransfer);

        if (!isAccountTransferValid.IsValid)
        {
            return Results.ValidationProblem(isAccountTransferValid.ToDictionary());
        }

        await _mediator.Send(accountTransfer);

        return Results.Ok(accountTransfer);
    }
}
