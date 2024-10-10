using MicroRabbit.Banking.Application.Handlers;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;
using Microsoft.Extensions.Logging;
using Moq;

namespace MicroRabbit.Banking.Application.UnitTest.Handlers;

public class TransferHandlerTests
{
    private readonly TransferHandler _transfersHandler;
    private readonly Mock<IEventBus> _busMock;
    private readonly Mock<ILogger<TransferHandler>> _logger;

    public TransferHandlerTests()
    {
        _busMock = new Mock<IEventBus>();
        _logger = new Mock<ILogger<TransferHandler>>();
        _transfersHandler = new TransferHandler(_busMock.Object, _logger.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new AccountTransferRequest()
        {
            AccountFrom = Guid.NewGuid(),
            AccountTo = Guid.NewGuid(),
            TransferAmount = 100
        };

        _busMock.Setup(x => x.SendCommand(It.IsAny<CreateTransferCommand>()))
            .Returns(Task.CompletedTask);

        var transfersHandler = new TransferHandler(_busMock.Object, _logger.Object);

        var logMessage = $"Transfered from account '{request.AccountFrom}' to account '{request.AccountTo}' the amount '{request.TransferAmount}'";

        // Act
        await transfersHandler.Handle(request, CancellationToken.None);

        // Assert
        _busMock.Verify(x => x.SendCommand(It.IsAny<CreateTransferCommand>()), Times.Once);
        _logger.Verify(
            _logger => _logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(logMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }
}
