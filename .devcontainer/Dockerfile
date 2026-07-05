# Base: .NET 8 SDK oficial (Debian Bookworm Slim)
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim

# Argumentos para customização do usuário
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=1000

# Instalar dependências do sistema
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends \
        # Ferramentas básicas
        git \
        curl \
        wget \
        ca-certificates \
        procps \
        gnupg2 \
        lsb-release \
        # Dependências nativas para SkiaSharp (usado pelo QuestPDF)
        libfontconfig1 \
        libfreetype6 \
        libharfbuzz0b \
        libicu72 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Criar usuário não-root com UID/GID compatíveis com Fedora host
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME -s /bin/bash \
    && mkdir -p /home/$USERNAME/.dotnet /home/$USERNAME/.nuget \
    && chown -R $USERNAME:$USERNAME /home/$USERNAME

# Adicionar sudo (opcional, para troubleshooting)
RUN apt-get update \
    && apt-get -y install sudo \
    && echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
    && chmod 0440 /etc/sudoers.d/$USERNAME \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Configurar permissões do workspace
RUN mkdir -p /workspace && chown -R $USERNAME:$USERNAME /workspace

# Instalar ferramentas .NET globais como usuário vscode
USER $USERNAME

# Instalar ferramentas de desenvolvimento .NET (opcional)
RUN dotnet tool install -g dotnet-ef || true \
    && dotnet tool install -g dotnet-format || true

# Configurar PATH para ferramentas globais
ENV PATH="$PATH:/home/$USERNAME/.dotnet/tools"

# Diretório de trabalho
WORKDIR /workspace

# Comando padrão
CMD ["/bin/bash"]
