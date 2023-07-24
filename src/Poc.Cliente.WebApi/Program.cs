using System.Text.Json.Serialization;
using Poc.Cliente.Infra;
using Poc.Cliente.Infra.Helpers;
using Poc.Cliente.Infra.Services;
using Poc.Cliente.Infra.Repositories;
using Poc.Cliente.Domain.Repositories;

using Microsoft.OpenApi.Models;
using Serilog;
using Gvdasa.Termos.Infra;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
ConfigurationManager Configuration = builder.Configuration;

CriarLogger();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddApplicationInsightsTelemetry();


builder.Services.Configure<InformacoesMensageriaAppsetting>(Configuration.GetSection("Mensageria"));

builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.
    JsonSerializerOptions.
    Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddControllers();

builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILogService, LogService>();

var origins = Configuration["OrigensCORS"].Split(";");

builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
                builder.WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });

builder.Services.AddEndpointsApiExplorer();

if (Configuration.GetValue<bool>("ExporSwagger"))
    StartupSwagger.ConfigureServices(builder);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    // Customize the message template
    options.MessageTemplate = "Handled {RequestPath}";

    // Emit debug-level events instead of the defaults
    // options.GetLevel = (httpContext, elapsed, ex) => Serilog.Events.LogEventLevel.Debug;

    // Attach additional properties to the request completion event
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RemoteIp", httpContext.Connection.RemoteIpAddress?.ToString());
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.GetValueOrDefault("User-Agent"));
        diagnosticContext.Set("IdTenant", httpContext.Request.Headers.GetValueOrDefault("IdTenant"));
        diagnosticContext.Set("IdCorrelacao", httpContext.Request.Headers.GetValueOrDefault("IdCorrelacao"));
    };
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (Configuration.GetValue<bool>("ExporSwagger"))
    StartupSwagger.ConfigureApp(app);

app.Run();

void CriarLogger()
{
    var configuration = new ConfigurationBuilder()
        .AddConfiguration(Configuration)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}
