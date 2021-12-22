using Citel.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Citel.web.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly HttpClient cliente;

        public CategoriasController()
        {
            this.cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:58242/api/categorias/");
        }
        public ActionResult Index()
        {
            try
            {
                IEnumerable<CategoriaViewModel> categorias = null;
                var resposta = cliente.GetAsync("");
                resposta.Wait();
                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudo = resultado.Content.ReadAsAsync<IList<CategoriaViewModel>>();
                    conteudo.Wait();
                    categorias = conteudo.Result;
                }
                else
                {
                    categorias = Enumerable.Empty<CategoriaViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor.");
                }

                return View(categorias);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                IEnumerable<CategoriaViewModel> categorias = null;
                var resposta = cliente.GetAsync("");
                resposta.Wait();
                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudo = resultado.Content.ReadAsAsync<IList<CategoriaViewModel>>();
                    conteudo.Wait();
                    categorias = conteudo.Result;
                }
                else
                {
                    categorias = Enumerable.Empty<CategoriaViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no servidor.");
                }

                return Json(categorias, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(CategoriaViewModel categoria)
        {
            try
            {
                if (categoria == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                var categoriaCriado = cliente.PostAsJsonAsync("criar", categoria);
                categoriaCriado.Wait();
                var resultado = categoriaCriado.Result;

                if (resultado.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Categoria criada com sucesso!" }, JsonRequestBehavior.AllowGet);

                return Json(new { sucesso = false, msg = "Erro ao criar categoria." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao criar categoria" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Edit(int? id)
        {
            try
            {
                if (id == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                CategoriaViewModel categoria = null;

                var resposta = cliente.GetAsync("categoria?id=" + id.ToString());
                resposta.Wait();
                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var conteudo = resultado.Content.ReadAsAsync<CategoriaViewModel>();
                    conteudo.Wait();
                    categoria = conteudo.Result;
                    return Json(categoria, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { sucesso = false, msg = "Erro ao tentar encontrar categoria." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao editar categoria" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Edit(CategoriaViewModel categoria)
        {
            try
            {
                if (categoria == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                var categoriaAtualizado = cliente.PutAsJsonAsync("editar", categoria);
                categoriaAtualizado.Wait();
                var resultado = categoriaAtualizado.Result;

                if (resultado.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Categoria editada com sucesso!" }, JsonRequestBehavior.AllowGet);

                return Json(new { sucesso = false, msg = "Erro ao editar categoria." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao editar produto " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                    return Json(new { sucesso = false, msg = "" }, JsonRequestBehavior.AllowGet);

                var resposta = cliente.DeleteAsync("excluir?id=" + id.ToString());
                resposta.Wait();
                var resultado = resposta.Result;

                if (resultado.IsSuccessStatusCode)
                    return Json(new { sucesso = true, msg = "Categoria deletada com sucesso." }, JsonRequestBehavior.AllowGet);

                else if (resultado.StatusCode.Equals((HttpStatusCode)422))
                    return Json(new { sucesso = false, msg = "Categoria não pode ser deletada pois já há produtos cadastrados com ela. Exclua os produtos e tente novamente!" }, JsonRequestBehavior.AllowGet);

                return Json(new { sucesso = false, msg = "Erro ao tentar excluir." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { sucesso = false, msg = "Ocorreu um erro ao deletar a categoria " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}