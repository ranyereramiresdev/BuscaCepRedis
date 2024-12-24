Projeto .8 - API ASP.NET
Como Configurar e Rodar o Projeto
1. Instalando o Necessário
1.1. Baixar o .NET 8 SDK
Faça o download e instale o .NET 8 SDK a partir do site oficial:
Download .NET 8

1.2. Baixar o Visual Studio
Baixe e instale o Visual Studio. Durante a instalação, selecione a carga de trabalho Desenvolvimento para ASP.NET e Web.
Download Visual Studio

2. Alterando a String de Conexão do Redis
No arquivo appsettings.json, você pode configurar a string de conexão do Redis na propriedade RedisSettings:ConnectString.

"RedisSettings": {
    "ConnectString": "localhost:6379,password=guest"
}

3. Executando o Projeto
Após ajustar a string de conexão, basta abrir o projeto no Visual Studio e pressionar F5 para rodar.
