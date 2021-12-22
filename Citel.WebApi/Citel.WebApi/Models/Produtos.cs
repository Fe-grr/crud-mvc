using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Citel.WebApi.Models
{
    [Table("Produtos")]
    public class Produtos
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required]
        public string Nome{ get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public virtual int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categorias Categoria { get; set; }
    }
}