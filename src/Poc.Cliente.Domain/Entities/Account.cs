using Poc.Cliente.Domain.Enums;

namespace Poc.Cliente.Domain.Entities;

public class Account: Entidade
{
    public int number_bank { get; set; }

    public string document { get; set; }

    public float balance { get; set; }

    public AccountStatus status { get; set; }

    public string created { get; set; }

    public string disable { get; set; }
}
