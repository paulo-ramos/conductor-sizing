using ConductorSizing.Web.Components;
using ConductorSizing.Application.Services;
using ConductorSizing.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel para container local
builder.WebHost.ConfigureKestrel(options =>
{
    var port = builder.Configuration.GetValue<int?>("Port") ?? 8080;
    options.ListenAnyIP(port);
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
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
