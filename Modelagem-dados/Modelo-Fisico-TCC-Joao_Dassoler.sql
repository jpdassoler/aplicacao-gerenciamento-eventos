CREATE TABLE Cliente (
    Usuario VARCHAR(15) PRIMARY KEY,
    Senha CHAR(60) NOT NULL, --Considerando armazenamento de hash
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Telefone VARCHAR(11) UNIQUE,
    Instagram VARCHAR(50)
);

CREATE INDEX idx_cliente_instagram ON Cliente(Instagram);

CREATE TABLE Evento (
    ID_Evento INT PRIMARY KEY AUTO_INCREMENT,
    ID_Endereco INT NOT NULL,
    Nome VARCHAR(50) NOT NULL,
    Data DATETIME NOT NULL,
    Banner VARCHAR(200),
    Descricao VARCHAR(200),
    Preco_Ingresso DECIMAL(15,2) CHECK (Preco_Ingresso >= 0),
    URL_Ingresso VARCHAR(200),
    FOREIGN KEY (ID_Endereco) REFERENCES Endereco(ID_Endereco)
);

CREATE INDEX idx_evento_nome ON Evento(Nome);
CREATE INDEX idx_evento_data ON Evento(Data);

CREATE TABLE Cliente_Evento (
    Usuario VARCHAR(15),
    ID_Evento INT,
    Ind_Comparecimento CHAR(1) NOT NULL,
    PRIMARY KEY (Usuario, ID_Evento),
    CHECK (Ind_Comparecimento IN ('O', 'S', 'N', 'T'))
);

CREATE TABLE Endereco (
    ID_Endereco INT PRIMARY KEY AUTO_INCREMENT,
    CEP INT NOT NULL,
    Rua VARCHAR(200) NOT NULL,
    Numero INT,
    Complemento VARCHAR(200),
    Bairro VARCHAR(100) NOT NULL,
    Cidade VARCHAR(100) NOT NULL,
    UF CHAR(2) NOT NULL
);

CREATE TABLE Mensagem (
    ID_Mensagem INT PRIMARY KEY AUTO_INCREMENT,
    Usuario VARCHAR(15) NOT NULL,
    ID_Evento INT NOT NULL,
    Conteudo VARCHAR(500) NOT NULL,
    Likes INT DEFAULT 0,
    Midia VARCHAR(200),
    FOREIGN KEY (Usuario) REFERENCES Cliente(Usuario),
    FOREIGN KEY (ID_Evento) REFERENCES Evento(ID_Evento)
);

CREATE TABLE Resposta (
    ID_Resposta INT PRIMARY KEY AUTO_INCREMENT,
    ID_Mensagem INT NOT NULL,
    Usuario VARCHAR(15) NOT NULL,
    Conteudo VARCHAR(500) NOT NULL,
    Likes INT DEFAULT 0,
    Midia VARCHAR(200),
    FOREIGN KEY (ID_Mensagem) REFERENCES Mensagem(ID_Mensagem),
    FOREIGN KEY (Usuario) REFERENCES Cliente(Usuario)
);

CREATE TABLE Aviso (
    ID_Aviso INT PRIMARY KEY AUTO_INCREMENT,
    Usuario VARCHAR(15) NOT NULL,
    ID_Evento INT NOT NULL,
    Conteudo VARCHAR(500) NOT NULL,
    Midia VARCHAR(200),
    FOREIGN KEY (Usuario) REFERENCES Cliente(Usuario),
    FOREIGN KEY (ID_Evento) REFERENCES Evento(ID_Evento)
);