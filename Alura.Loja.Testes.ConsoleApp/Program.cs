using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var contexto2 = new LojaContext())
            {
                var serviceProvider = contexto2.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                ///Esse .include() se chama "Carregamento Implícito"
                var cliente = contexto2
                    .Clientes
                    .Include(x => x.EnderecoDeEntrega)
                    .FirstOrDefault();

                Console.WriteLine($"Endereço de entrega: {cliente.EnderecoDeEntrega.Logradouro}");

                var produto = contexto2
                    .Produtos
                    .FirstOrDefault(x => x.Id == 8);

                ///Gerando uma pesquisa em que, o where está sendo feito sobre
                ///a entidade do join e não na principal                
                ///Esse jeito se chama "Carregamento explícito"
                contexto2.Entry(produto)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 10)
                    .Load();

                Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");
                foreach (var item in produto.Compras)
                {
                    Console.WriteLine("\t" + item);
                }
            }

        }

        private static void ExibeProdutosDaPromocao()
        {
            using (var contexto2 = new LojaContext())
            {
                var serviceProvider = contexto2.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //Fazendo join com as entidades relacionadas
                var promocao = contexto2
                    .Promocoes
                    .Include(p => p.Produtos)
                    .ThenInclude(x => x.Produto)
                    .FirstOrDefault();
                Console.WriteLine("\nMostrando os produtos da promoção...");
                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }
            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var promocao = new Promocao()
                {
                    Descricao = "Queima total Janeiro 2017",
                    DataInicio = new DateTime(2017, 1, 1),
                    DataTermino = new DateTime(2017, 1, 31)
                };

                var produtos = contexto
                    .Produtos
                    .Where(x => x.Categoria == "Bebidas")
                    .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluiProduto(item);
                }

                contexto.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        private static void UmParaUm()
        {
            var fulano = new Cliente();
            fulano.Nome = "Fulano de tal";
            fulano.EnderecoDeEntrega = new Endereco()
            {
                Numero = 12,
                Logradouro = "Rua da casa",
                Compremento = "Complemento do endereco",
                Bairro = "Portão",
                Cidade = "Cidade",
                CEP = "80610280",
                Estado = "PR"
            };

            using (var contexto = new LojaContext())
            {

                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Add(fulano);

                //var promocao = contexto.Promocoes.Find(1);
                //contexto.Promocoes.Remove(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        private static void MuitosParaMuitos()
        {
            var p1 = new Produto()
            {
                Nome = "Suco de laranja",
                Categoria = "Bebidas",
                PrecoUnitario = 8.5,
                Unidade = "Litro"
            };
            var p2 = new Produto()
            {
                Nome = "Cafe",
                Categoria = "Bebidas",
                PrecoUnitario = 10.6,
                Unidade = "Pacote"
            };
            var p3 = new Produto()
            {
                Nome = "Macarrao",
                Categoria = "Alimentos",
                PrecoUnitario = 3.2,
                Unidade = "Pacote"
            };


            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Páscoa feliz";
            promocaoDePascoa.DataInicio = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncluiProduto(p1);
            promocaoDePascoa.IncluiProduto(p2);
            promocaoDePascoa.IncluiProduto(p3);

            using (var contexto = new LojaContext())
            {

                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Promocoes.Add(promocaoDePascoa);

                //var promocao = contexto.Promocoes.Find(1);
                //contexto.Promocoes.Remove(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        public static void InsertEntity()
        {
            #region Inserção ENtity
            //var paoFrances = new Produto()
            //{
            //    Nome = "Pão Françes",
            //    PrecoUnitario = 5.4,
            //    Categoria = "Padaria",
            //    Unidade = "Unidade"
            //};

            using (var contexto = new LojaContext())
            {

                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var paoFrances = contexto.Produtos.FirstOrDefault(x => x.Id == 8);

                var compra = new Compra();
                compra.Quantidade = 100;
                compra.Produto = paoFrances;
                compra.Preco = compra.Quantidade * paoFrances.PrecoUnitario;

                contexto.Compras.Add(compra);
                ExibeEntries(contexto.ChangeTracker.Entries());

                contexto.SaveChanges();
            }
            #endregion
        }

        public void qualquercoias()
        {
            using (var contexto = new LojaContext())
            {
                var produtos = contexto.Produtos.ToList();
                foreach (var item in produtos)
                {
                    Console.WriteLine(item);
                }

                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                ExibeEntries(contexto.ChangeTracker.Entries());

                //var p1 = produtos.Last();
                //p1.Nome = "007 - O Espiao Que Me Amava";

                //var novoProduto = new Produto()
                //{
                //    Nome = "Amaciante",
                //    Categoria = "Licccmpezas",
                //    Preco = 3.5
                //};

                //contexto.Produtos.Add(novoProduto);

                ExibeEntries(contexto.ChangeTracker.Entries());

                var p1 = produtos.First();

                contexto.Produtos.Remove(p1);
                ExibeEntries(contexto.ChangeTracker.Entries());
                RecuperarProdutos();
                contexto.SaveChanges();
                ExibeEntries(contexto.ChangeTracker.Entries());


            }

        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            Console.WriteLine("=================");
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
            Console.WriteLine("=================");
        }

        private static void AtualizarProduto()
        {
            GravarUsandoEntityFramework();
            RecuperarProdutos();

            using (var repo = new ProdutoDAOEntity())
            {
                var produtos = repo.Produtos().FirstOrDefault();
                produtos.Nome = "Ordem da Alura";

                repo.Atualizar(produtos);
            }

            RecuperarProdutos();
        }

        private static void ExcluirProdutos()
        {
            using (var repo = new ProdutoDAOEntity())
            {
                var produtos = repo.Produtos();
                foreach (var item in produtos)
                {
                    repo.Remover(item);
                }
            }
        }

        private static void RecuperarProdutos()
        {
            using (var repo = new ProdutoDAOEntity())
            {
                var produtos = repo.Produtos();
                Console.WriteLine("Foram encontradas {0} produtos", produtos.Count());
                foreach (var item in produtos)
                {
                    Console.WriteLine(item.Nome);
                }
            }
        }

        private static void GravarUsandoEntityFramework()
        {
            try
            {
                Produto p1 = new Produto();
                p1.Nome = "Harry Potter e a Ordem da Fênix";
                p1.Categoria = "Livros";
                p1.PrecoUnitario = 19.89;

                using (var contexto = new ProdutoDAOEntity())
                {
                    contexto.Adicionar(p1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void GravarUsandoAdoNet()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.PrecoUnitario = 19.89;

            using (var repo = new ProdutoDAO())
            {
                repo.Adicionar(p);
            }
        }
    }

}
