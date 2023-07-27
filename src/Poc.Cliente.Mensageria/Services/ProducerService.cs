using System;
using System.Text;
using System.Threading.Tasks;
using Poc.Cliente.Domain.Config;
using Poc.Cliente.Domain.Repositories;
using Poc.Cliente.Mensageria.Entities;

using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using System.Threading;

namespace Poc.Cliente.Mensageria.Services
{
    public abstract class ProducerService
    {
        ManualResetEvent CompletedResetEvent = new ManualResetEvent(false);
        SubscriptionClient Client;
        private readonly IConfigurationPoc _configuracoesPoc;
        private readonly ITransactionRepository _repository;
        private readonly IAccountRepository _accountRepository;


        public ProducerService(IConfigurationPoc configuracoes, ITransactionRepository repository, IAccountRepository accountRepository)
        {
            _configuracoesPoc = configuracoes;
            _repository = repository;
            _accountRepository = accountRepository;

            var managementClient = new ManagementClient(_configuracoesPoc.Mensageria_ConnectionString);
            CriarTopicoSeNaoExistir(managementClient);
        }

        private void CriarTopicoSeNaoExistir(ManagementClient managementClient)
        {
            try
            {
                var topicExists = managementClient.TopicExistsAsync(_configuracoesPoc.Mensageria_TopicConvitesEnviados).Result;
                if (topicExists)
                    return;

                var result = managementClient.CreateTopicAsync(_configuracoesPoc.Mensageria_TopicConvitesEnviados).Result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task EnviarMensagem(string mensagem, string idTenant)
        {
            try
            {
                var topic = _configuracoesPoc.Mensageria_TopicConvitesEnviados;

                var mensagemTopico = new Message(Encoding.UTF8.GetBytes(mensagem));
                mensagemTopico.UserProperties.Add("idTenant", idTenant);

                var topicClient = new TopicClient(_configuracoesPoc.Mensageria_ConnectionString, topic);
                await topicClient.SendAsync(mensagemTopico);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void GettingMessageViaSubscription()
        {

            var subscriptionClient = SubscriptionClient.CreateFromConnectionString
                        (
                            _configuracoesPoc.Mensageria_ConnectionString,
                            _configuracoesPoc.MensageriaTopic,
                            _configuracoesPoc.MensageriaSubscription,
                            ReceiveMode.PeekLock
                        );

                var onMessageOptions = new OnMessageOptions();
                onMessageOptions.ExceptionReceived += OnMessageError;

                subscriptionClient.OnMessageAsync(OnMessageReceived, onMessageOptions);

                Console.ReadKey();
        }

    private static void OnMessageError(object sender, ExceptionReceivedEventArgs e)
    {
        if (e != null && e.Exception != null)
        {
            Console.WriteLine("Hey, there's an error!" + e.Exception.Message + "\r\n\r\n");
        }
    }

    private static async Task OnMessageReceived(BrokeredMessage arg)
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
        await arg.CompleteAsync();
    }

        public async Task<bool> TopicoExiste()
        {
            var managementClient = new ManagementClient(_configuracoesPoc.Mensageria_ConnectionString);
            return await managementClient.TopicExistsAsync(_configuracoesPoc.Mensageria_TopicConvitesEnviados);
        }
    }
}
