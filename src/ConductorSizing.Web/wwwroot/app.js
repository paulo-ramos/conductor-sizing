// Função para fazer download de arquivo a partir de um stream de bytes
window.downloadFileFromStream = async (fileName, byteArray) => {
    const blob = new Blob([byteArray], { type: 'application/pdf' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};

// Aguardar o Blazor estar pronto antes de configurar reconexão
window.addEventListener('DOMContentLoaded', () => {
    // Aguardar o Blazor inicializar completamente
    const interval = setInterval(() => {
        if (window.Blazor) {
            clearInterval(interval);
            
            // Configurar manipulador de reconexão personalizado
            const originalStarted = Blazor.start;
            Blazor.start = function(options) {
                options = options || {};
                options.reconnectionOptions = {
                    maxRetries: 10,
                    retryIntervalMilliseconds: 3000
                };
                
                return originalStarted.call(this, options);
            };
            
            console.log('Blazor Server configurado com reconexão automática');
        }
    }, 100);
});
