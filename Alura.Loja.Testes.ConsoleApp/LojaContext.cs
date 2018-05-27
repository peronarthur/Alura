using Microsoft.EntityFrameworkCore;
using System;

namespace Alura.Loja.Testes.ConsoleApp
{
    public class LojaContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Promocao> Promocoes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        //public DbSet<Endereco> Enderecos { get; set; }

        public LojaContext() { }

        public LojaContext(DbContextOptions<LojaContext> options): base(options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=DESKTOP-GK5FL2O\\SQLEXPRESS;Database=LojaDB;user id=sa;pwd=minduin;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Definindo chave composta para a tabela específica
            modelBuilder
                .Entity<PromocaoProduto>()
                .HasKey(p => new { p.ProdutoId, p.PromocaoId });

            modelBuilder
                .Entity<Endereco>()
                .ToTable("Enderecos");

            ///Está definindo que, a classe endereço possui a chave primária com o mesmo valor que a classe
            ///cliente e isso não é mostrado dentro da classe endereço
            ///Para o entity isso se chama ShadowProperty
            modelBuilder
                .Entity<Endereco>()
                .Property<int>("ClienteId");

            modelBuilder
                .Entity<Endereco>()
                .HasKey("ClienteId");

            base.OnModelCreating(modelBuilder);
        }

    }
}