using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Citel.web.Models
{
    public class ProdutoViewModel
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public virtual int CategoriaId { get; set; }
        public virtual CategoriaViewModel Categoria { get; set; }
    }
}