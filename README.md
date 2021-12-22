# crud-mvc
CRUD de produtos e categorias

    Passo 1: Executar as queries do arquivo Create-Entities.sql

    Passo 2: Abrir solução Citel.WebApi.sln dentro da pasta Citel.WebApi
   
    2.1: Abrir arquivo Web.config
    
    2.2: Editar a connectionString (linha 19) com o seu servidor que usou para criar as entidades no banco de dados (ex.: connectionString="Data Source=SEUSERVIDOR;...)
 
    Passo 3: Executar o projeto Citel.WebApi

    Passo 4: Abrir solução Citel.web.sln dentro da pasta Citel.web

    4.1: Abrir os arquivos ProdutosController e CategoriasController e alterar a porta do BaseAddress para a porta identificada no link do Swagger (exibido ao executar o passo 3)
  
    4.2 : Executar o projeto Citel.web
