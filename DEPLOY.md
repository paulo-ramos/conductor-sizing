# 🚀 Guia de Deploy - Conductor Sizing

Este guia fornece instruções completas para deploy da aplicação Conductor Sizing em diferentes ambientes.

## 📋 Pré-requisitos

- Podman ou Docker instalado
- .NET 8 SDK (para desenvolvimento local)
- Porta 8080 disponível

## 🐳 Deploy com Podman/Docker

### 1. Build da Imagem

```bash
# Usando Podman (Fedora)
podman build -t conductor-sizing:latest .

# Ou usando Docker
docker build -t conductor-sizing:latest .
```

### 2. Executar Container

```bash
# Script automático (recomendado)
chmod +x run-podman.sh
./run-podman.sh

# Ou manualmente
podman run -d \
  --name conductor-sizing \
  -p 8080:8080 \
  --restart=unless-stopped \
  conductor-sizing:latest
```

### 3. Verificar Status

```bash
# Ver logs
podman logs -f conductor-sizing

# Status do container
podman ps

# Estatísticas de uso
podman stats conductor-sizing
```

### 4. Gerenciar Container

```bash
# Parar
podman stop conductor-sizing

# Iniciar
podman start conductor-sizing

# Reiniciar
podman restart conductor-sizing

# Remover
./stop-podman.sh
# ou
podman rm -f conductor-sizing
```

## 🔄 Deploy com Docker Compose

```bash
# Iniciar serviços
podman-compose up -d

# Ver logs
podman-compose logs -f

# Parar serviços
podman-compose down
```

## ☸️ Deploy no Kubernetes

### 1. Criar Namespace

```bash
kubectl create namespace conductor-sizing
```

### 2. Deploy com Manifests

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: conductor-sizing
  namespace: conductor-sizing
spec:
  replicas: 3
  selector:
    matchLabels:
      app: conductor-sizing
  template:
    metadata:
      labels:
        app: conductor-sizing
    spec:
      containers:
      - name: conductor-sizing
        image: conductor-sizing:latest
        ports:
        - containerPort: 8080
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: ASPNETCORE_URLS
          value: "http://0.0.0.0:8080"
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /
            port: 8080
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /
            port: 8080
          initialDelaySeconds: 5
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: conductor-sizing
  namespace: conductor-sizing
spec:
  selector:
    app: conductor-sizing
  ports:
    - protocol: TCP
      port: 80
      targetPort: 8080
  type: LoadBalancer
```

```bash
# Aplicar configuração
kubectl apply -f deployment.yaml

# Verificar status
kubectl get pods -n conductor-sizing
kubectl get svc -n conductor-sizing
```

## 🌐 Deploy com Ingress (Kubernetes)

```yaml
# ingress.yaml
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: conductor-sizing
  namespace: conductor-sizing
  annotations:
    kubernetes.io/ingress.class: nginx
    cert-manager.io/cluster-issuer: letsencrypt-prod
spec:
  tls:
  - hosts:
    - conductor-sizing.example.com
    secretName: conductor-sizing-tls
  rules:
  - host: conductor-sizing.example.com
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: conductor-sizing
            port:
              number: 80
```

## 📦 Deploy em Servidor Linux

### Com Systemd

1. **Criar usuário de serviço:**

```bash
sudo useradd -r -s /bin/false conductor-sizing
```

2. **Copiar binários:**

```bash
# Build release
dotnet publish src/ConductorSizing.Web -c Release -o /opt/conductor-sizing

# Ajustar permissões
sudo chown -R conductor-sizing:conductor-sizing /opt/conductor-sizing
```

3. **Criar serviço systemd:**

```bash
sudo nano /etc/systemd/system/conductor-sizing.service
```

```ini
[Unit]
Description=Conductor Sizing - Blazor App
After=network.target

[Service]
Type=notify
User=conductor-sizing
Group=conductor-sizing
WorkingDirectory=/opt/conductor-sizing
ExecStart=/usr/bin/dotnet /opt/conductor-sizing/ConductorSizing.Web.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:8080

[Install]
WantedBy=multi-user.target
```

4. **Habilitar e iniciar:**

```bash
sudo systemctl daemon-reload
sudo systemctl enable conductor-sizing
sudo systemctl start conductor-sizing
sudo systemctl status conductor-sizing
```

## 🔒 Nginx como Reverse Proxy

```nginx
# /etc/nginx/sites-available/conductor-sizing
server {
    listen 80;
    server_name conductor-sizing.example.com;
    
    location / {
        proxy_pass http://localhost:8080;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection $http_connection;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        
        # SignalR configuration
        proxy_buffering off;
        proxy_read_timeout 100s;
    }
}
```

```bash
# Habilitar site
sudo ln -s /etc/nginx/sites-available/conductor-sizing /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

## 🔐 HTTPS com Let's Encrypt

```bash
# Instalar certbot
sudo dnf install certbot python3-certbot-nginx

# Obter certificado
sudo certbot --nginx -d conductor-sizing.example.com

# Renovação automática
sudo systemctl enable certbot-renew.timer
```

## 📊 Monitoramento

### Prometheus + Grafana

1. **Adicionar métricas no Program.cs:**

```csharp
builder.Services.AddHealthChecks();

app.MapHealthChecks("/health");
app.MapMetrics("/metrics");
```

2. **Configurar Prometheus:**

```yaml
# prometheus.yml
scrape_configs:
  - job_name: 'conductor-sizing'
    static_configs:
      - targets: ['localhost:8080']
    metrics_path: '/metrics'
```

## 🔧 Variáveis de Ambiente

```bash
# Produção
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS=http://0.0.0.0:8080

# Logging
export Logging__LogLevel__Default=Information
export Logging__LogLevel__Microsoft__AspNetCore=Warning

# Configurações customizadas
export Port=8080
```

## 💾 Backup e Restore

Como a aplicação não usa banco de dados, o histórico é mantido em memória e perdido ao reiniciar. Se precisar persistência:

### Opção 1: Volume Docker

```bash
podman run -d \
  -v conductor-data:/app/data \
  -p 8080:8080 \
  conductor-sizing:latest
```

### Opção 2: Implementar Persistência

Modificar `DimensionamentoStateService` para salvar em arquivo JSON periodicamente.

## 🐛 Troubleshooting

### Container não inicia

```bash
# Verificar logs
podman logs conductor-sizing

# Verificar porta
ss -tlnp | grep 8080

# Executar interativamente
podman run -it --rm -p 8080:8080 conductor-sizing:latest
```

### Permissão negada (SELinux)

```bash
# Verificar contexto SELinux
ls -Z /path/to/volume

# Ajustar contexto
chcon -Rt container_file_t /path/to/volume
```

### Alto uso de memória

```bash
# Limitar recursos no Podman
podman run -d \
  --memory="512m" \
  --cpus="0.5" \
  -p 8080:8080 \
  conductor-sizing:latest
```

## 📈 Performance Tips

1. **Habilitar Response Compression** (já configurado)
2. **Usar CDN** para assets estáticos
3. **Configurar Cache Headers** no Nginx
4. **Limitar histórico** em memória (já implementado)
5. **Escalar horizontalmente** com Load Balancer

## 🔄 CI/CD Pipeline

### GitHub Actions Example

```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Build Image
      run: |
        podman build -t conductor-sizing:${{ github.sha }} .
        
    - name: Push to Registry
      run: |
        podman push conductor-sizing:${{ github.sha }} \
          registry.example.com/conductor-sizing:${{ github.sha }}
```

## 📞 Suporte

Para problemas de deploy, verifique:
- Logs da aplicação
- Logs do container
- Logs do reverse proxy (se aplicável)
- Conectividade de rede
- Permissões de arquivo/SELinux

---

**Nota**: Adapte as configurações conforme seu ambiente de produção.
