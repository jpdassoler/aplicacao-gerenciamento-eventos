### README do Backend

```markdown
# Backend - Aplicação de Gerenciamento de Eventos

Este é o backend da aplicação de gerenciamento de eventos, responsável pela lógica de negócio, autenticação e comunicação com o banco de dados MySQL.

## Tecnologias

- **.NET Core 7**
- **Entity Framework Core**
- **MySQL**
- **AWS Elastic Beanstalk** para deploy

## Estrutura

- **Controllers:** Controladores responsáveis pelas rotas e respostas HTTP.
- **Services:** Serviços que contêm a lógica de negócio.
- **Repositories:** Classes responsáveis pelo acesso ao banco de dados.
- **Models:** Classes que representam as entidades do banco de dados.

## Configuração do Banco de Dados

O backend utiliza MySQL como banco de dados. Para rodar localmente:

1. Crie um banco de dados MySQL.

2. Crie as tabelas dentro do banco de dados, que estão na parte Modelagem de dados do projeto

2. Configure a string de conexão com variáveis de ambiente:
   MYSQL_DB_HOST="localhost"
   MYSQL_DB_USER="seuUsuario"
   MYSQL_DB_PASSWORD="suaSenha"
   MYSQL_DB_NAME="seuDb"
   
O frontend deve ser liberado pelo CORS para acesso ao backend. Para isso, use a variável de ambiente:
   FRONTEND_URL="http://localhost:3000"

## Como rodar

1. Instale as dependências:
   dotnet restore

2. Configure a string de conexão conforme as instruções acima

3. Rode a aplicação
   dotnet run

## Testes

Os testes utilizam NUnit. Para rodar os testes, certifique-se que o banco de dados esteja vazio:
   dotnet test