using ConductorSizing.Web.Components;
using ConductorSizing.Application.Services;
using ConductorSizing.Domain.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para aceitar a porta do Railway ou padrão
builder.WebHost.ConfigureKestrel(options =>
{
    // Tentar pegar a porta da variável de ambiente PORT (Railway) ou usar 8080
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    var portNumber = int.Parse(port);
    options.ListenAnyIP(portNumber);
});

// Configurar para funcionar atrás de proxy (Railway)
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Adicionar serviços ao container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar serviços da aplicação
builder.Services.AddScoped<IDimensionamentoService, DimensionamentoService>();

// Configurar SignalR para funcionar em produção atrás de proxy
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 1024 * 1024; // 1 MB
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
    options.KeepAliveInterval = TimeSpan.FromSeconds(15);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

// IMPORTANTE: UseForwardedHeaders deve vir ANTES de outros middlewares
app.UseForwardedHeaders();

// Configurar o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
