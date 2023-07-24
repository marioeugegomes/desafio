using Poc.Cliente.Domain.Entities;
using Poc.Cliente.Infra.Services;
using Microsoft.AspNetCore.Mvc;

namespace Poc.Cliente.WebApi.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private IConfiguration configuration;
    private ITransactionService _service;

    public TransactionController(
        IConfiguration configuration,
        ITransactionService service
        )
    {
        this.configuration = configuration;
        this._service = service;
    }

    [HttpGet]
    [Route("byDocument/{document}")]
    public async Task<List<Transaction>> GetTransaction(string document)
    {
        return await _service.ListForUser(document);
    }
}
