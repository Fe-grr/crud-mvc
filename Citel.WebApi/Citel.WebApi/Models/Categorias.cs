using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Citel.WebApi.Models
{
    [Table("Categorias")]
    public class Categorias
    {
        [Key]
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
    }
}