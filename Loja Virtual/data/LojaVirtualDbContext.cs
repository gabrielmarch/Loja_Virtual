using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.data
{
    public class LojaVirtualDbContext : DbContext
    {
            
        public DbSet<Cliente> Clientes { get; set; } = null!;

        public DbSet<PessoaFisica> PessoasFisicas { get; set; } = null!;

        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; } = null!;


        public DbSet<Servico> Servicos { get; set; } = null!;

        public DbSet<Boleto> Boletos { get; set; } = null!;

        public DbSet<Cupom> cupoms { get; set; } = null!;

        public DbSet<Produto> Produtos { get; set; } = null!;

        public DbSet<Pedido> Pedidos { get; set; } = null!;

        public DbSet<ItemPedido> itemPedidos { get; set; } = null!;

        public DbSet<Fornecedor> fornecedores { get; set; } = null!;

        public DbSet<Cartao> cartoes { get; set; } = null!;



        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string conexao =
                    @"Server=(localdb)\MSSQLLocalDB;
                      Database=LojaVirtualDb;
                      Trusted_Connection=True;
                      TrustServerCertificate=True;";

                optionsBuilder.UseSqlServer(conexao);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da tabela de clientes
            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            // Herança entre Cliente, PessoaFisica e PessoaJuridica
            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TipoCliente")
                .HasValue<PessoaFisica>("PessoaFisica")
                .HasValue<PessoaJuridica>("PessoaJuridica");

            // Relacionamento entre Pedido e Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId);
        }


    }
}
