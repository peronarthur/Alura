namespace Alura.Loja.Testes.ConsoleApp
{
    public class Endereco
    {
        public int Numero { get; internal set; }
        public string Logradouro { get; internal set; }
        public string Compremento { get; internal set; }
        public string Bairro { get; internal set; }
        public string Cidade { get; internal set; }
        public string CEP { get; internal set; }
        public string Estado { get; internal set; }

        public Cliente Cliente { get; set; }
    }
}