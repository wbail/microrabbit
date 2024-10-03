using MicroRabbit.Mvc.Models.Dtos;

namespace MicroRabbit.Mvc.Services;

public interface ITransferService
{
    Task Transfer(TransferDto transferDto);
}
