using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Poc.Cliente.Infra;

public class TenantProvider : ITenantProvider
{
    public string TenantMensagem { get; set; }
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IConfiguration Configuration { get; }

    public TenantProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        Configuration = configuration;
    }

    public string ObterTenant()
    {
        var idTenant = "";

        if (_httpContextAccessor.HttpContext != null)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("IdTenant", out var traceValue))
            {
                idTenant = traceValue;
            }

            if (string.IsNullOrEmpty(idTenant))
            {
                throw new MultiTenancyException("Não foi possível obter o IdTenant no header da requisição");
            }
        }
        return idTenant;
    }
}
