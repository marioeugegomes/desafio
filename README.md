# DESAFIO - Instalação .NET Core

Este é o guia de instalação do .NET Core em seu sistema. O .NET Core é uma plataforma de desenvolvimento gratuita e de código aberto, criada pela Microsoft, que suporta várias linguagens de programação e permite desenvolver aplicativos para diferentes sistemas operacionais.

## Requisitos do sistema

Antes de prosseguir com a instalação do .NET Core, verifique se seu sistema atende aos seguintes requisitos:

- **Sistema Operacional**: O .NET Core é compatível com Windows, macOS e várias distribuições Linux. Certifique-se de que seu sistema está atualizado e compatível com as versões suportadas pelo .NET Core.

- **Espaço em Disco**: É necessário espaço livre em disco para instalar o .NET Core e quaisquer pacotes adicionais que você possa precisar.

## Instruções de Instalação

Siga as etapas abaixo para instalar o .NET Core em seu sistema:

1. **Baixe o instalador**: Acesse o site oficial do .NET Core em https://dotnet.microsoft.com/ e procure a seção de downloads. Baixe o instalador apropriado para o seu sistema operacional.

2. **Execute o instalador**: Após o download ser concluído, execute o arquivo de instalação. Siga as instruções do assistente de instalação para configurar o .NET Core em seu sistema. Durante o processo, você pode ter a opção de selecionar componentes adicionais ou adicionar o .NET Core ao PATH do sistema.

3. **Verifique a instalação**: Após a instalação ser concluída, abra um terminal ou prompt de comando e digite o seguinte comando:

   ```
   dotnet --version
   ```

   Se a instalação foi bem-sucedida, você verá a versão do .NET Core instalada no seu sistema.

## Configuração do Ambiente de Desenvolvimento (opcional)

Se você planeja desenvolver aplicativos .NET Core, pode ser útil configurar um ambiente de desenvolvimento integrado (IDE) adequado, como o Visual Studio Code ou o Visual Studio, ambos compatíveis com .NET Core.

1. **Visual Studio Code**: Acesse https://code.visualstudio.com/ e baixe o Visual Studio Code. Instale o VS Code em seu sistema e, em seguida, procure extensões relacionadas ao .NET Core para melhorar a experiência de desenvolvimento.

2. **Visual Studio**: Se você preferir a IDE completa do Visual Studio, acesse https://visualstudio.microsoft.com/pt-br/ e baixe a versão mais recente. Durante a instalação, selecione as cargas de trabalho relevantes para o desenvolvimento do .NET Core.

## Executando um projeto de exemplo

Para garantir que tudo esteja configurado corretamente, vamos executar um projeto de exemplo:

1. **Execute o projeto**: Digite o seguinte comando para compilar e executar o projeto:

   ```
   dotnet run
   ```

   Você deve ver a saída do projeto de exemplo no console.

Parabéns! Você instalou com sucesso o .NET Core e executou um projeto de exemplo. Agora você está pronto para começar a desenvolver seus próprios aplicativos com .NET Core.

## Recursos adicionais

Para obter mais informações sobre o .NET Core, como tutoriais, documentação e amostras, consulte o site oficial do .NET Core em https://dotnet.microsoft.com/. Lá você encontrará uma riqueza de recursos para ajudá-lo em sua jornada de desenvolvimento com .NET Core.


## Arquitetura

1. **Projetos**
   - Poc.Cliente.Domain - Responsável pelo domínio da aplicação (Entities, Enums, Interfaces, Handlers);
   - Poc.Cliente.Infra - Responsável por manter as camadas de negócio (services), persistência (repositories, contexts), middlewares e helpers (extensions, helpers);
   - Poc.Cliente.Mensageria - Responsável pela recuperação e operação transacional dos registros de publish Transaction;
   - Poc.Cliente.Publish - Responsável por receber e publicar em filas as transações recebidas;
   - Poc.Cliente.WebApi - Responsável por manter Account e Transaction, bem como disponibilizar os registros transacionados;

2. **Toolkit**
   Devido a necessidade de negócio as seguintes soluções foram utilizadas:

   - Cloud: Azure (Obs.: Foi utilizado Terraform para implementação dos ambientes)
   - Database: MongoDB (Obs.: camada context da a possibilidade de configurar outros drivers para banco de dados)
   - Mensageria: ServiceBus Azure (Obs.: Implementado com ServiceBus (Obs.: Possibilidade de configuração, Kafka, PUB SUB), entretanto podemos utilziar o package Message como factory para demais aplicações)
   - Observable: ApplicationInsights
   - Logs: AzureTableStorage para gravação de logs de auditoria

3. **Arquitetura**
   A escolhida foi de microsserviços, devido suas características que permitem que você se concentre no desenvolvimento dos serviços sem se preocupar com as dependências, resiliência, observabilidade, descentralização do negócio e escalabilidade.

   O foco direcionado para solução é a transação para contas correntes, o que possibilita uma alta concorrência e volumetria de dados. Baseado nisso essa arquietura nos dá a alta disponibilidade necessária para esse tipo de aplicação, quando escolhemos utilizar filas (Mensageria) para processamento das transações, nos possibilitou que quando uma aplicação enviasse uma transação, a mesma não se perderia caso houvesse uma indisponibilidade do serviço de transação.

   Foi colocado observabilidade para monitoramento de desempenho, rastreamento de transações e inteligência artificial para fornecer insights acionáveis sobre o ambiente de TI.

   Pensei em três tipos de logs:
      - Aplicação: utilizado para detecção de erros e identificar falhas de fluxo, não deve conter dados sensíveis.
      - Auditoria: utilizado para detecção de falhas de segurança e potenciais fraudes cometidas.
      - Negócio: utilizado para avaliar o comportamento do usuário dentro de um fluxo específico.

      Nesse modelo definiria uma ferramenta que possibilite a análise comportamental dos dados (SIEM), sugeriria como ferramentas ELK (Elastic Search, Logstash e Kiana)

   Para monitoramento de serviços em ambientes críticos sugeriria o Prometheus com Grafana para análise de dados, gerando gráficos com informações em tempo real.