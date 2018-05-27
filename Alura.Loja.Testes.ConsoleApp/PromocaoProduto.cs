namespace Alura.Loja.Testes.ConsoleApp
{
    /// <summary>
    /// Nessa versão do EntityFrameworkCore, o mesmo não consegue fazer o mapeamento das
    /// entidades N-N e por isso é necessário criar uma entidade como essa no banco
    /// também é necessário epsecificar no LojaContext a criação da chave primária composta entre
    /// ProdutoId e PromocaoId
    /// </summary>
    public class PromocaoProduto
    {
        public int ProdutoId { get; set; }
        public int PromocaoId { get; set; }
        public Produto Produto { get; set; }
        public Promocao Promocao { get; set; }
    }
}
