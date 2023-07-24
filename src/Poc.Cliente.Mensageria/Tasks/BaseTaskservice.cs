using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Poc.Cliente.Mensageria.Entities;
using Poc.Cliente.Mensageria.Services;

namespace Poc.Cliente.Mensageria.Tasks
{
    public abstract class BaseTaskService
    {
        protected MensagemTopico _mensagemTopico { get; set; }
        protected readonly string _loggerMessageTemplate  = "Request Logger - {Mensagem}";

        protected BaseTaskService()
        {
        }

        protected abstract ProducerService ObterAppProducerService(string taskStatusTopic = null);

        protected async Task EnviarMensagemLiberacaoConvite(string message, string idTenant, Dictionary<string, object> customProperties = null)
        {
            var appProducerService = ObterAppProducerService();

            try
            {
                //TODO: ver implementação de log
                //await SetLogMessageAsync(message, idTenant, customProperties);
                await appProducerService.EnviarMensagem(message, idTenant);
            }
            catch (Exception ex)
            {
                //await appProducerService.AtualizarSituacaoTarefa(_mensagemAgendamento.IdOcorrenciaAgendamento, TipoSituacaoHistorico.Erro, ex.Message);
            }
        }
    }
}
