using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProdutosApp.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "A quantidade é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
    [Display(Name = "Quantidade")]
    public int Quantidade { get; set; }

    [Required(ErrorMessage = "O valor é obrigatório.")]
    [Column(TypeName = "decimal(12,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    [Display(Name = "Valor (R$)")]
    public decimal Valor { get; set; }

    [Required]
    [Display(Name = "Data de Cadastro")]
    [DataType(DataType.DateTime)]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Informe o usuário que está cadastrando.")]
    [StringLength(100)]
    [Display(Name = "Usuário Cadastro")]
    public string UsuarioCadastro { get; set; } = string.Empty;
}