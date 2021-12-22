using Citel.WebApi.Models;
using Citel.WebApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Citel.WebApi.Controllers
{
    /// <summary>
    /// Citel
    /// </summary>
    [RoutePrefix("api/produtos")]
    public class ProdutosController : ApiController
    {
        private readonly CitelContext contexto;

        /// <summary>
        /// Construtor para controller da API de produtos 
        /// </summary>
        public ProdutosController() => this.contexto = new CitelContext();

        /// <summary>
        /// Obter lista de todos os produtos
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public HttpResponseMessage ObterProdutos()
        {
            try
            {
                IList<Produtos> produtos = null;
                produtos = contexto.Produtos.ToList()
                    .Select(x => new Produtos()
                    {
                        ProdutoId = x.ProdutoId,
                        Nome = x.Nome,
                        Preco = x.Preco,
                        Descricao = x.Descricao,
                        CategoriaId = x.CategoriaId,
                        Categoria = new Categorias()
                        {
                            CategoriaId = x.Categoria.CategoriaId,
                            Nome = x.Categoria.Nome
                        }
                    }).ToList();

                if (produtos.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, produtos);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produtos não encontrados.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar consultar os produtos. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Obter produto por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("produto"), HttpGet]
        public HttpResponseMessage ObterProdutoPorId(int? id)
        {
            try
            {
                if (id == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id do produto não pode ser nulo.");

                Produtos produto = produto = contexto.Produtos.Include("Categoria").ToList()
                         .Where(x => x.ProdutoId == id)
                         .Select(y => new Produtos()
                         {
                             ProdutoId = y.ProdutoId,
                             Nome = y.Nome,
                             Descricao = y.Descricao,
                             CategoriaId = y.CategoriaId,
                             Categoria = new Categorias()
                             {
                                 CategoriaId = y.Categoria.CategoriaId,
                                 Nome = y.Categoria.Nome
                             }
                         }).FirstOrDefault<Produtos>();

                if (produto == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Produto não encontrado.");

                return Request.CreateResponse(HttpStatusCode.OK, produto);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar consultar o produto {id}. Erro:\n {ex}");
            }
        }
        /// <summary>
        /// Criar produto
        /// </summary>
        /// <returns></returns>
        [Route("criar"), HttpPost]
        public HttpResponseMessage CriarProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                if (!ModelState.IsValid || produto == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dados para criação do produto inválidos.");

                contexto.Produtos.Add(new Produtos()
                {
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Descricao = produto.Descricao,
                    CategoriaId = produto.CategoriaId,
                    CriadoEm = DateTime.Now
                });

                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, "Produto criado com sucesso!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar criar produto. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Editar produto
        /// </summary>
        /// <param name="produto"></param>
        /// <returns></returns>
        [Route("editar"), HttpPut]
        public HttpResponseMessage AtualizarProduto([FromBody] ProdutoDTO produto)
        {
            try
            {
                if (!ModelState.IsValid || produto == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dados do produto inválidos.");

                var produtoEditado = contexto.Produtos.Where(p => p.ProdutoId == produto.ProdutoId).FirstOrDefault();

                if (produtoEditado == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Não foi possível encontrar o produto.");

                produtoEditado.Nome = produto.Nome;
                produtoEditado.Preco = produto.Preco;
                produtoEditado.Descricao = produto.Descricao;
                produtoEditado.AtualizadoEm = DateTime.Now;
                contexto.Entry(produtoEditado).State = EntityState.Modified; ;
                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Produto editado com sucesso!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar editar produto. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Excluir produto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("excluir"), HttpDelete]
        public HttpResponseMessage ExcluirProduto(int? id)
        {
            try
            {
                if (id == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id do produto não pode ser nulo.");

                var produtoSelecionado = contexto.Produtos.Find(id);

                if (produtoSelecionado == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Não foi possível encontrar o produto.");

                contexto.Entry(produtoSelecionado).State = EntityState.Deleted;
                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Produto deletado com sucesso!");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar deletar o produto. Erro:\n {ex}");
            }
        }
    }
}