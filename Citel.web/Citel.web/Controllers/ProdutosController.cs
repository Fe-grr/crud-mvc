using Citel.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace Citel.web.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly HttpClient cliente;

        public ProdutosController()
        {
            this.cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:58242/api/produtos/");
        }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                IEnumerable<ProdutoViewModel> produtos = null;
                var resposta = cliente.GetAsync("");
                resposta.Wait();
                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudo = resultado.Content.ReadAsAsync<IList<ProdutoViewModel>>();
                    conteudo.Wait();
                    produtos = conteudo.Result;
                }
                else
                {
                    produtos = Enumerable.Empty<ProdutoViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor.");
                }

                return View(produtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(ProdutoViewModel produto)
         {
            try
            {
                if (produto == null)
                    return Json(new { sucesso = false, msg = "Produto inválido." }, JsonRequestBehavior.AllowGet);

                var produtoCriado = cliente.PostAsJsonAsync<ProdutoViewModel>("criar", produto);
                produtoCriado.Wait();
                var resultado = produtoCriado.Result;

                if (resultado.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Produto criado com sucesso!" }, JsonRequestBehavior.AllowGet);

                return Json(new { sucesso = false, msg = "Ocorreu um erro ao criar produto!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao editar produto " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Edit(int? id)
        {
            try
            {
                if (id == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                ProdutoViewModel produto = null;

                var resposta = cliente.GetAsync("produto?id=" + id.ToString());
                resposta.Wait();
                var result = resposta.Result;

                if (result.IsSuccessStatusCode)
                {
                    var conteudo = result.Content.ReadAsAsync<ProdutoViewModel>();
                    conteudo.Wait();
                    produto = conteudo.Result;
                    return Json(produto, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { sucesso = false, msg = "Erro ao tentar encontrar produto." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao editar produto " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Edit(ProdutoViewModel produto)
        {
            try
            {
                if (produto == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                var produtoAtualizado = cliente.PutAsJsonAsync<ProdutoViewModel>("editar", produto);
                produtoAtualizado.Wait();
                var result = produtoAtualizado.Result;

                if (result.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Produto editado com sucesso!" }, JsonRequestBehavior.AllowGet);

                return Json(new { sucesso = false, msg = "Erro ao editar produto." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao editar produto " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int? id)
        {
            try
            {
                if (id == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                var resposta = cliente.DeleteAsync("excluir?id=" + id.ToString());
                resposta.Wait();
                var result = resposta.Result;

                if (result.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Produto deletado com sucesso." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { sucesso = false, msg = "Erro ao tentar excluir." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao deletar o produto " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}