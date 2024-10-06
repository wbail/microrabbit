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

    public BankingsController(IMediator mediator, IAccountService accountService)
    {
        _mediator = mediator;
        _accountService = accountService;
    }

    [HttpGet]
    public ActionResult<IAsyncEnumerable<Account>> Get()
    {
        return Ok(_accountService.GetAccounts());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AccountTransferRequest accountTransfer)
    {
        //todo: return errors to client if the request is invalid

        await _mediator.Send(accountTransfer);

        return Ok(accountTransfer);
    }
}
