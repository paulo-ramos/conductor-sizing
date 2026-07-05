using ConductorSizing.Web.Components;
using ConductorSizing.Application.Services;
using ConductorSizing.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para aceitar a porta do Railway ou padrão
builder.WebHost.ConfigureKestrel(options =>
{
    // Tentar pegar a porta da variável de ambiente PORT (Railway) ou usar 8080
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    var portNumber = int.Parse(port);
    options.ListenAnyIP(portNumber);
});

// Adicionar serviços ao container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrar serviços da aplicação
builder.Services.AddScoped<IDimensionamentoService, DimensionamentoService>();

// Configurar SignalR para melhor performance em tempo real
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 102400; // 100 KB
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

var app = builder.Build();

// Configurar o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Não usar HSTS/HTTPS redirect em produção - Railway gerencia isso
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
