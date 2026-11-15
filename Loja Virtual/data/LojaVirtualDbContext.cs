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
        public DbSet<Pagamento> Pagamentos { get; set; } = null!;

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

            // CLIENTES
            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TipoCliente")
                .HasValue<PessoaFisica>("PessoaFisica")
                .HasValue<PessoaJuridica>("PessoaJuridica");

            // CUPONS
            modelBuilder.Entity<Cupom>()
                .ToTable("cupoms", "dbo");

            modelBuilder.Entity<Cupom>()
                .HasKey(c => c.Id);

            // PEDIDOS
            modelBuilder.Entity<Pedido>()
                .ToTable("Pedidos");

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.IdCupomId)
                .HasColumnName("idCupomId");

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cupom)
                .WithMany()
                .HasForeignKey(p => p.IdCupomId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .HasPrincipalKey(p => p.Id);

            // ITENS DOS PEDIDOS
            modelBuilder.Entity<ItemPedido>()
                .ToTable("itemPedidos");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.PedidoId)
                .HasColumnName("idPedidoId");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ProdutoId)
                .HasColumnName("idProdutoId");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.ServicoId)
                .HasColumnName("idServicoId");

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Servico)
                .WithMany()
                .HasForeignKey(i => i.ServicoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            // PAGAMENTOS
            modelBuilder.Entity<Pagamento>()
                .ToTable("Pagamento");

            modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Pedido)
                .WithOne(p => p.Pagamento)
                .HasForeignKey<Pagamento>(p => p.PedidoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pagamento>()
                .HasIndex(p => p.PedidoId)
                .IsUnique();
        }
    }
}
