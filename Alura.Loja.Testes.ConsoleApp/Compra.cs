namespace Alura.Loja.Testes.ConsoleApp
{
    public class Compra
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public Produto Produto { get; internal set; }
        public int Quantidade { get; internal set; }
        public double Preco { get; internal set; }
        
        public Compra()
        {
        }

        public override string ToString()
        {
            return $"Compra de {Quantidade} {Produto.Unidade} do produto {Produto.Nome} a R${Preco}";
        }
    }
}