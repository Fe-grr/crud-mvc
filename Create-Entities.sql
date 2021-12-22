--Executar script a script de cima a baixo

CREATE DATABASE Citel;

CREATE TABLE Categorias
(
	CategoriaId INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(255),
	CriadoEm DATETIME,
	AtualizadoEm DATETIME
)

CREATE TABLE Produtos
(
	ProdutoId INT IDENTITY(1,1) PRIMARY KEY,
	Nome VARCHAR(255),
	Preco DECIMAL,
	Descricao VARCHAR(MAX),
	CriadoEm DATETIME,
	AtualizadoEm DATETIME,
	CategoriaId INT FOREIGN KEY REFERENCES Categorias(CategoriaId)
)
