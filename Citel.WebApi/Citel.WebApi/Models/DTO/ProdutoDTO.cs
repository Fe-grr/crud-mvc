using System;

namespace Citel.WebApi.Models.DTO
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public virtual int CategoriaId { get; set; }
    }
}