namespace Poc.Cliente.Infra;

public interface ITenantProvider
{
    string TenantMensagem { get; set; }
    string ObterTenant();
}
