using Microsoft.AspNetCore.Mvc;
using Poc.Cliente.Mensageria.Services;
using Poc.Cliente.Publish.Entities;

using Newtonsoft.Json;

namespace Poc.Cliente.Publish.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PublishAccountController : ControllerBase
{
        private IProducerService _service;

    private readonly ILogger<PublishAccountController> _logger;

    public PublishAccountController(ILogger<PublishAccountController> logger, IProducerService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost(Name = "Publish/{:tenant}")]
    public async Task<ActionResult> Publish(string tenant, [FromBody] Transaction transaction)
    {
        try {
            var message = JsonConvert.Serialize(transaction);
            await _service.EnviarMensagem(message, tenant);

            return Ok(true);
        } catch(Exception ex) {
            _logger.LogError("Não foi possível realizar a transação", ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { erro = "Não foi possível realizar a transação" });
        }
    }
}
