using Poc.Cliente.Domain.Enums;

namespace Poc.Cliente.Domain.Entities;

public class Transaction: Entidade
{
    public TransactionType type { get; set; }

    public float value { get; set; }

    public string created { get; set; }

    public PaymentType payment { get; set; }

    public string date_create { get; set; }

    public string date_disable { get; set; }

    public Account account { get; set; }
}
