using MicroRabbit.Mvc.Models.Dtos;
using System.Text;
using System.Text.Json;

namespace MicroRabbit.Mvc.Services;

public class TransferService : ITransferService
{
    private readonly HttpClient _client;

    public TransferService(HttpClient client)
    {
        _client = client;
    }

    public async Task Transfer(TransferDto transferDto)
    {
        var uri = "https://localhost:7162/api/bankings";

        var transferContent = new StringContent(JsonSerializer.Serialize(transferDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(uri, transferContent);

        response.EnsureSuccessStatusCode();
    }
}
