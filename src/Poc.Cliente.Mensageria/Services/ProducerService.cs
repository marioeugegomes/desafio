using System;
using System.Text;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Config;
using Poc.Cliente.Domain.Repositories;
using Poc.Cliente.Mensageria.Entities;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace Poc.Cliente.Mensageria.Services
{
    public abstract class ProducerService
    {
        private readonly ISubscriptionClient _client;
        private readonly IConfiguracoesGVquest _configuracoesGVquest;
        private readonly ITransactionRepositories _repository;
        private readonly IAccountRepositories _accountRepository;


        public ProducerService(IConfiguracoesGVquest configuracoesGVquest, ITransactionRepositories repository, IAccountRepositories accountRepository)
        {
            _configuracoesGVquest = configuracoesGVquest;
            _repository = repository;
            _accountRepository = accountRepository;

            var managementClient = new ManagementClient(_configuracoesPoc.Cliente.Mensageria_ConnectionString);
            CriarTopicoSeNaoExistir(managementClient);
        }

        private void CriarTopicoSeNaoExistir(ManagementClient managementClient)
        {
            try
            {
                var topicExists = managementClient.TopicExistsAsync(_configuracoesPoc.Cliente.Mensageria_TopicConvitesEnviados).Result;
                if (topicExists)
                    return;

                var result = managementClient.CreateTopicAsync(_configuracoesPoc.Cliente.Mensageria_TopicConvitesEnviados).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnviarMensagem(string mensagem, string idTenant, string topic = null)
        {
            try
            {
                if (topic == null)
                {
                    topic = _configuracoesPoc.Cliente.Mensageria_TopicConvitesEnviados;
                }

                var mensagemTopico = new Message(Encoding.UTF8.GetBytes(mensagem));
                mensagemTopico.UserProperties.Add("idTenant", idTenant);

                var topicClient = new TopicClient(_configuracoesPoc.Cliente.Mensageria_ConnectionString, topic);
                await topicClient.SendAsync(mensagemTopico);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       public static void GettingMessageViaSubscription(string idTenant, string topic = null)
        {
            var sC = SubscriptionClient.CreateFromConnectionString(_configuracoesPoc.Cliente.Mensageria_ConnectionString, idTenant, topic);
            sC.OnMessage(m =>
            {
                List<MensagemTopico> transactions = JsonConvert.DeserializeObject<MensagemTopico>(m.GetBody<string>());

                transactions.ForEach( item => {
                    Account user = _accountRepository.find(account => account.document = item.document);
                    if (!user) {
                        throw Exception("Cliente não encontrado");
                    }

                    Transaction transaction = new Transaction {
                        type = item.type,
                        payment = item.payment,
                        value = item.value,
                        created = item.date,
                        date_create = new Date(),
                        account = user
                     };

                     _repository.Save(transaction);
                });

            });
        }

        public async Task<bool> TopicoExiste()
        {
            var managementClient = new ManagementClient(_configuracoesPoc.Cliente.Mensageria_ConnectionString);
            return await managementClient.TopicExistsAsync(_configuracoesPoc.Cliente.Mensageria_TopicConvitesEnviados);
        }
    }
}
