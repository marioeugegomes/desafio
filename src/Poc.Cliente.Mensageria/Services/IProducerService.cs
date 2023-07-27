using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Entities;

namespace Poc.Cliente.Mensageria.Services
{
    public interface IProducerService
    {
       public Task EnviarMensagem(string mensagem, string idTenant);

    }
}
