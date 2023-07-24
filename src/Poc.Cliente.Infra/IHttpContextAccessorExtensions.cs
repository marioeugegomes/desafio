using Microsoft.AspNetCore.Http;

namespace Poc.Cliente.Infra
{
    public static class IHttpContextAccessorExtensions
    {
        public static string GetIdTenant(this IHttpContextAccessor httpContextAccessor)
        {
            var idTenant = "";

            if (httpContextAccessor.HttpContext.Request.Headers.TryGetValue("IdTenant", out var traceValue))
            {
                idTenant = traceValue;
            }

            if (string.IsNullOrEmpty(idTenant))
            {
                throw new MultiTenancyException("Não foi possível obter o IdTenant no header da requisição");
            }

            return idTenant;
        }
    }
}
