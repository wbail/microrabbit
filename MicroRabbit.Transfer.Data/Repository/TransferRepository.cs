using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Data.Repository;

public class TransferRepository : ITransferRepository
{
    private readonly TransferDbContext _context;

    public TransferRepository(TransferDbContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<TransferLog> GetTransferLogs()
    {
        return _context.TransferLogs.AsAsyncEnumerable();
    }
}
