namespace Poc.Cliente.Mensageria.Entities
{
    public class ConfiguracaoServicoMensageria
    {
        public string ConnectionString { get; set; }
        public string Topic { get; set; }
        public string TaskStatusTopic { get; set; }
    }
}
