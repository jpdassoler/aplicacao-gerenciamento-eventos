# Etapa 1: Compilação
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia o arquivo de projeto e restaura dependências
COPY EventManagerBackend.csproj ./
RUN dotnet restore 

# Copia todos os outros arquivos e compila o projeto
COPY . ./ 
RUN dotnet publish -c Release -o /out

# Etapa 2: Execução
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copia os arquivos da etapa de build para a etapa de runtime
COPY --from=build /out ./

# Expor a porta que a aplicação irá usar
EXPOSE 80

# Definir o comando para rodar a aplicação
ENTRYPOINT ["dotnet", "EventManagerBackend.dll"]
