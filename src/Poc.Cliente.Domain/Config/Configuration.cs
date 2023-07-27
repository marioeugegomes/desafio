using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Poc.Cliente.Domain.Config
{
    public class Configuration: IConfigurationPoc
    {
        internal Configuration(IConfiguration configuration)
        {
            CarregarConfiguracoes(configuration);
        }

        public string Mensageria_ConnectionString { get; set; }
        public string Mensageria_TopicTaskStatus { get; set; }
        public string Mensageria_TopicConvitesEnviados { get; set; }
        public string Mensageria_LimiteConvitesEnviadosPorMensagem { get; set; }
        public string MensageriaSubscription { get; set; }
        public string MensageriaTopic { get; set; }

        private void CarregarConfiguracoes(IConfiguration configuration)
        {
            Mensageria_ConnectionString = configuration["Mensageria:ConnectionString"];
            Mensageria_TopicConvitesEnviados = configuration["Mensageria:TopicConvitesEnviados"];
            Mensageria_TopicTaskStatus = configuration["Mensageria:TopicTaskStatus"];
            Mensageria_LimiteConvitesEnviadosPorMensagem = configuration["Mensageria:LimiteConvitesEnviadosPorMensagem"];

        }
    }
}