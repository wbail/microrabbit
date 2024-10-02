using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces;

public interface ITransferRepository
{
    IAsyncEnumerable<TransferLog> GetTransferLogs();
}
