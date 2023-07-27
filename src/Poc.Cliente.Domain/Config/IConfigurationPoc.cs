using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Cliente.Domain.Config
{
    public interface IConfigurationPoc
    {
         //Serviço de Mensageria
        public string MensageriaSubscription { get; set; }
        public string MensageriaTopic { get; set; }
        public string Mensageria_ConnectionString { get; set; }
        public string Mensageria_TopicTaskStatus { get; set; }
        public string Mensageria_TopicConvitesEnviados { get; set; }
        public string Mensageria_LimiteConvitesEnviadosPorMensagem { get; set; }
    }
}