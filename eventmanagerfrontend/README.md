# Frontend - Aplicação de Gerenciamento de Eventos

Este é o frontend da aplicação de gerenciamento de eventos, desenvolvido em React. Ele permite a interação do usuário com o sistema, como a criação e visualização de eventos.

## Tecnologias

- **React**
- **Axios** para comunicação com a API
- **CSS** para estilização
- **AWS Elastic Beanstalk** para deploy

## Funcionalidades

- Página de cadastro de clientes
- Página de login de clientes.
- Página de criação de eventos.
- Página de home, com exibição dos eventos criados.
- Página com detalhes do evento, incluindo endereço e preço e confirmação de participação em eventos.

## Configuração

As URLs de API são configuradas dinamicamente no arquivo `config.js`. Dependendo do ambiente (produção ou desenvolvimento), o arquivo de configuração (`config.json` ou `config-dev.json`) será carregado automaticamente.
Para rodar localmente, a URL será http://localhost:5285

## Como Rodar

1. Acesse o diretório do frontend:
   ```bash
   cd eventmanagerfrontend

2. Instale as dependências:
   npm install

3. Rode o servidor de desenvolvimento:
   npm start

A aplicação está disponível em http://localhost:3000