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
    [RoutePrefix("api/categorias")]
    public class CategoriasController : ApiController
    {
        private readonly CitelContext contexto;

        /// <summary>
        /// Construtor para controller da API de categorias 
        /// </summary>
        public CategoriasController() => this.contexto = new CitelContext();

        /// <summary>
        /// Obter lista de todas as categorias
        /// </summary>
        /// <returns></returns>
        [Route(""), HttpGet]
        public HttpResponseMessage ObterCategorias()
        {
            try
            {
                IList<Categorias> categorias = null;
                categorias = contexto.Categorias.ToList()
                    .Select(x => new Categorias()
                    {
                        CategoriaId = x.CategoriaId,
                        Nome = x.Nome
                    }).ToList();

                if (categorias.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, categorias);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Categorias não encontradas.");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar consultar as categorias. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Obter categoria por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("categoria"), HttpGet]
        public HttpResponseMessage ObterCategoriaPorId(int? id)
        {
            try
            {
                if (id == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id da categoria não pode ser nulo.");

                Categorias categoria = contexto.Categorias
                         .Where(x => x.CategoriaId == id).ToList()
                         .Select(y => new Categorias()
                         {
                             CategoriaId = y.CategoriaId,
                             Nome = y.Nome
                         }).FirstOrDefault();

                if (categoria == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Categoria não encontrada.");

                return Request.CreateResponse(HttpStatusCode.OK, categoria);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar consultar a categoria {id}. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Criar categoria
        /// </summary>
        /// <returns></returns>
        [Route("criar"), HttpPost]
        public HttpResponseMessage CriarCategoria([FromBody] CategoriaDTO categoria)
        {
            try
            {
                if (!ModelState.IsValid || categoria == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dados para criação da categoria inválidos.");

                contexto.Categorias.Add(new Categorias()
                {
                    Nome = categoria.Nome,
                    CriadoEm = DateTime.Now
                });

                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created, "Categoria criada com sucesso!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar criar categoria. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Editar categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [Route("editar"), HttpPut]
        public HttpResponseMessage AtualizarProduto([FromBody] CategoriaDTO categoria)
        {
            try
            {
                if (!ModelState.IsValid || categoria == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Dados da categoria inválidos.");

                var categoriaEditada = contexto.Categorias.Where(p => p.CategoriaId == categoria.CategoriaId).FirstOrDefault();

                if (categoriaEditada == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Não foi possível encontrar a categoria.");

                categoriaEditada.Nome = categoria.Nome;
                categoriaEditada.AtualizadoEm = DateTime.Now;
                contexto.Entry(categoriaEditada).State = EntityState.Modified; ;
                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Categoria editada com sucesso!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar editar categoria. Erro:\n {ex}");
            }
        }

        /// <summary>
        /// Excluir categoria
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("excluir"), HttpDelete]
        public HttpResponseMessage ExcluirCategoria(int? id)
        {
            try
            {
                if (id == null)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id da categoria não pode ser nulo.");

                var podeExcluir = contexto.Produtos.Where(x => x.CategoriaId == id).FirstOrDefault() == null;

                if (!podeExcluir)
                    return Request.CreateResponse((HttpStatusCode)422, "Categoria não pode ser deletada pois já há produtos cadastrados com ela. Exclua os produtos e tente novamente!");


                var categoriaSelecionada = contexto.Categorias.Find(id);

                if (categoriaSelecionada == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Não foi possível encontrar a categoria.");

                contexto.Entry(categoriaSelecionada).State = EntityState.Deleted;
                contexto.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, "Categoria deletada com sucesso!");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, $"Erro ao tentar deletar a categoria. Erro:\n {ex}");
            }
        }
    }
}