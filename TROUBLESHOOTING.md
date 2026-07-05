# 🔧 Troubleshooting - Conductor Sizing

Guia completo para resolução de problemas comuns.

## 🚨 Problemas de Compilação

### ❌ Erro: "O SDK do .NET especificado não foi encontrado"

**Sintoma:**
```
A versão do SDK do .NET especificada no global.json [8.0.100] não foi encontrada
```

**Solução:**
```bash
# Verificar SDK instalado
dotnet --list-sdks

# Instalar .NET 8
# Fedora
sudo dnf install dotnet-sdk-8.0

# Ubuntu
sudo apt install dotnet-sdk-8.0

# Ou remover global.json temporariamente
mv global.json global.json.bak
```

### ❌ Erro: "Não foi possível restaurar pacotes NuGet"

**Sintoma:**
```
error NU1301: Unable to load the service index
```

**Solução:**
```bash
# Limpar cache NuGet
dotnet nuget locals all --clear

# Restaurar novamente
dotnet restore --force
```

### ❌ Erro: "CS0246: Tipo ou nome de namespace não encontrado"

**Sintoma:**
```
error CS0246: The type or namespace name 'X' could not be found
```

**Solução:**
```bash
# Limpar tudo
dotnet clean
rm -rf **/bin **/obj

# Restaurar e rebuild
dotnet restore
dotnet build
```

## 🐳 Problemas com Containers

### ❌ Podman: "Permission denied" em volumes

**Sintoma:**
```
Error: mkdir /workspace: permission denied
```

**Solução (SELinux):**
```bash
# Opção 1: Flag :Z (já configurado)
podman run -v /path:/app:Z ...

# Opção 2: Ajustar contexto
chcon -Rt container_file_t /path

# Opção 3: Desabilitar SELinux temporariamente
sudo setenforce 0
```

### ❌ Container não inicia: "Port already in use"

**Sintoma:**
```
Error: cannot listen on the TCP port: address already in use
```

**Solução:**
```bash
# Encontrar processo usando porta 8080
sudo ss -tlnp | grep 8080

# Matar processo
sudo kill -9 <PID>

# Ou usar porta diferente
podman run -p 8081:8080 ...
```

### ❌ Container: "Health check failed"

**Sintoma:**
```
Health check failed: container unhealthy
```

**Solução:**
```bash
# Verificar logs
podman logs -f conductor-sizing

# Verificar conectividade
podman exec conductor-sizing curl http://localhost:8080/health

# Aumentar timeout no Dockerfile
HEALTHCHECK --interval=30s --timeout=10s --retries=5 \
  CMD curl -f http://localhost:8080/health || exit 1
```

## 🌐 Problemas de Rede

### ❌ "Unable to connect to http://localhost:8080"

**Sintoma:**
Navegador não consegue acessar a aplicação.

**Solução:**
```bash
# Verificar se aplicação está rodando
dotnet list | grep ConductorSizing.Web

# Verificar porta aberta
sudo ss -tlnp | grep 8080

# Testar com curl
curl http://localhost:8080

# Verificar firewall
sudo firewall-cmd --list-ports
sudo firewall-cmd --add-port=8080/tcp --permanent
sudo firewall-cmd --reload
```

### ❌ SignalR: "Connection lost"

**Sintoma:**
```
Error: Connection disconnected with error 'Error: Server timeout elapsed...'
```

**Solução no appsettings.json:**
```json
{
  "CircuitOptions": {
    "DetailedErrors": true,
    "DisconnectedCircuitMaxRetained": 100,
    "DisconnectedCircuitRetentionPeriod": "00:03:00"
  }
}
```

**Ou no Program.cs:**
```csharp
builder.Services.AddSignalR(options => {
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
});
```

## 💾 Problemas de Memória

### ❌ "OutOfMemoryException"

**Sintoma:**
```
System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown
```

**Solução:**
```csharp
// Reduzir limite de histórico em DimensionamentoStateService
builder.Services.AddSingleton<DimensionamentoStateService>(sp => 
    new DimensionamentoStateService(maxHistorico: 50)); // Reduzir de 200 para 50

// Ou limitar memória do container
podman run --memory="512m" ...
```

### ❌ Container usando muita memória

**Sintoma:**
```bash
$ podman stats
CONTAINER  CPU%  MEM USAGE / LIMIT  MEM%
conductor  5%    800MB / 512MB      156%
```

**Solução:**
```bash
# Limitar recursos
podman run -d \
  --memory="512m" \
  --memory-swap="1g" \
  --cpus="1.0" \
  -p 8080:8080 \
  conductor-sizing:latest
```

## 🎨 Problemas de Interface

### ❌ Tailwind CSS não carrega

**Sintoma:**
Página sem estilos, parece HTML puro.

**Solução em App.razor:**
```html
<!-- Verificar se o CDN está correto -->
<script src="https://cdn.tailwindcss.com"></script>

<!-- Verificar configuração -->
<script>
  tailwind.config = {
    theme: {
      extend: {}
    }
  }
</script>
```

### ❌ Formulário não atualiza valores

**Sintoma:**
Valores digitados não aparecem no resultado.

**Solução:**
```csharp
// Verificar @bind no Input
<InputNumber @bind-Value="@_entrada.PotenciaAtivaWatts" ... />

// Verificar StateHasChanged() após calcular
private void CalcularDimensionamento() {
    // ... cálculo
    StateHasChanged(); // Forçar atualização
}
```

### ❌ Navegação entre páginas não funciona

**Sintoma:**
Clicar em links não muda de página.

**Solução em MainLayout.razor:**
```html
<!-- Usar NavLink, não <a> -->
<NavLink href="/dimensionamento" class="...">
    Dimensionamento
</NavLink>
```

## 🔒 Problemas de Permissão (Linux)

### ❌ "Permission denied" ao executar scripts

**Sintoma:**
```
bash: ./run-podman.sh: Permission denied
```

**Solução:**
```bash
# Adicionar permissão de execução
chmod +x run-podman.sh
chmod +x stop-podman.sh

# Ou executar com bash
bash run-podman.sh
```

### ❌ Dev Container: "Failed to mount workspace"

**Sintoma:**
```
Error: Failed to mount workspace folder
```

**Solução em .devcontainer/devcontainer.json:**
```json
{
  "mounts": [
    "source=${localWorkspaceFolder},target=/workspace,type=bind,Z"
  ],
  "runArgs": ["--userns=keep-id"]
}
```

## 📊 Problemas de Performance

### ❌ Aplicação lenta para responder

**Sintoma:**
Interface demora muito para calcular.

**Solução:**
```csharp
// Adicionar cache se necessário
private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

// Ou otimizar cálculos
private async Task CalcularDimensionamento() {
    await Task.Run(() => {
        _resultado = DimensionamentoService.Calcular(_entrada);
    });
    await InvokeAsync(StateHasChanged);
}
```

### ❌ SignalR desconecta frequentemente

**Sintoma:**
Mensagens de "reconnecting..." aparecem com frequência.

**Solução no Program.cs:**
```csharp
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => {
        options.DetailedErrors = true;
        options.MaxBufferedUnacknowledgedRenderBatches = 10;
    });

builder.Services.AddSignalR(options => {
    options.MaximumReceiveMessageSize = 102400 * 10; // Aumentar
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
});
```

## 🔍 Debugging

### Habilitar logs detalhados

**appsettings.Development.json:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug",
      "Microsoft.AspNetCore.SignalR": "Debug"
    }
  }
}
```

### Ver logs da aplicação

```bash
# Local
dotnet run --project src/ConductorSizing.Web

# Container
podman logs -f conductor-sizing

# Com timestamp
podman logs -f --since 10m conductor-sizing
```

### Verificar erros de compilação

```bash
# Build verbose
dotnet build --verbosity detailed

# Verificar warnings
dotnet build /warnaserror
```

## 🧪 Teste Rápido de Conectividade

```bash
# Aplicação rodando?
curl http://localhost:8080

# Health check
curl http://localhost:8080/health

# WebSocket (SignalR)
wscat -c ws://localhost:8080/_blazor
```

## 📞 Checklist de Diagnóstico

Quando algo não funciona, verificar:

- [ ] .NET 8 SDK instalado? `dotnet --version`
- [ ] Pacotes restaurados? `dotnet restore`
- [ ] Build sem erros? `dotnet build`
- [ ] Porta 8080 livre? `ss -tlnp | grep 8080`
- [ ] Firewall liberado? `sudo firewall-cmd --list-ports`
- [ ] SELinux configurado? (flag :Z nos volumes)
- [ ] Container rodando? `podman ps`
- [ ] Logs sem erros? `podman logs conductor-sizing`
- [ ] Navegador atualizado? (Suporta WebSocket)
- [ ] Cache limpo? Ctrl+Shift+R no navegador

## 🆘 Último Recurso

Se nada funcionar:

```bash
# Limpar tudo
dotnet clean
rm -rf **/bin **/obj
podman system prune -a -f

# Recomeçar do zero
dotnet restore
dotnet build
dotnet run --project src/ConductorSizing.Web
```

## 📚 Recursos Adicionais

- [Documentação .NET](https://learn.microsoft.com/dotnet/)
- [Blazor Troubleshooting](https://learn.microsoft.com/aspnet/core/blazor/fundamentals/troubleshoot)
- [Podman Docs](https://docs.podman.io/)
- [SELinux Guide](https://docs.fedoraproject.org/en-US/quick-docs/getting-started-with-selinux/)

---

**Se o problema persistir, verifique os logs detalhados e documente o erro completo!**
