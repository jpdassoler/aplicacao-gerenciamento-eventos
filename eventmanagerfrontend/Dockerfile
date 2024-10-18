# Usa uma imagem Node.js para construção
FROM node:18 AS build
WORKDIR /app

# Copia o package.json e instala dependências
COPY package*.json ./
RUN npm install

# Copia o código-fonte e constrói a aplicação
COPY . ./
RUN npm run build

# Usa uma imagem nginx para servir os arquivos estáticos
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html

# Expor a porta 80
EXPOSE 3000

