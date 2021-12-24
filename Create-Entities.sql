--Executar script a script

CREATE DATABASE Citel;

CREATE TABLE citel.Categorias
(
	CategoriaId INT not null AUTO_INCREMENT,
	Nome VARCHAR(255),
	CriadoEm DATETIME,
	AtualizadoEm DATETIME,
	primary key (CategoriaId)
)

CREATE TABLE citel.Produtos
(
	ProdutoId INT not null AUTO_INCREMENT,
	Nome VARCHAR(255),
	Preco DECIMAL,
	Descricao VARCHAR(3000),
	CriadoEm DATETIME,
	AtualizadoEm DATETIME,
	CategoriaId INT,
	primary key (ProdutoId),
	foreign key (CategoriaId) references Categorias(CategoriaId)
)