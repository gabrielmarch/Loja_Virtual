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

        public DbSet<Produto> produtos { get; set; } = null!;

        public DbSet<Pedido> pedidos { get; set; } = null!;

        public DbSet<ItemPedido> itemPedidos { get; set; } = null!;

        public DbSet<Fornecedor> fornecedores { get; set; } = null!;

        public DbSet<Cartao> cartoes { get; set; } = null!;

        public DbSet<Pagamento> Pagamentos { get; set; } = null!;



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




            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("Clientes");
                e.HasKey(c => c.Id);

                e.HasDiscriminator<string>("TipoCliente")
                    .HasValue<PessoaFisica>("PessoaFisica")
                    .HasValue<PessoaJuridica>("PessoaJuridica");

                e.Property(c => c.Nome).HasMaxLength(150).IsRequired();
                e.Property(c => c.Email).HasMaxLength(100).IsRequired();
                e.Property(c => c.Telefone).HasMaxLength(20);
                e.Property(c => c.Endereco).HasMaxLength(200);
                e.Property(c => c.SenhaHash).HasMaxLength(200).IsRequired();

                e.HasIndex(c => c.Email).IsUnique();

            });

            modelBuilder.Entity<PessoaFisica>(e =>
            {
                e.Property(pf => pf.CPF).HasMaxLength(14).IsRequired();
                e.Property(pf => pf.DataNascimento).IsRequired();
                e.HasIndex(pf => pf.CPF).IsUnique();
            });

            modelBuilder.Entity<PessoaJuridica>(e =>
            {
                e.Property(pj => pj.CNPJ).HasMaxLength(18).IsRequired();
                e.Property(pj => pj.RazaoSocial).HasMaxLength(200);
                e.Property(pj => pj.NomeFantasia).HasMaxLength(200);
                e.HasIndex(pj => pj.CNPJ).IsUnique();
            });

            modelBuilder.Entity<Fornecedor>(e =>
            {
                e.ToTable("Fornecedores");
                e.HasKey(f => f.idFornecedor);
                e.Property(f => f.NomeFornecedor).HasMaxLength(200).IsRequired();
                e.Property(f => f.EmailFornecedor).HasMaxLength(100);
                e.Property(f => f.TelefoneFornecedor).HasMaxLength(20);
                e.Property(f => f.CNPJFornecedor).HasMaxLength(18).IsRequired();
                e.HasIndex(f => f.CNPJFornecedor).IsUnique();
                e.HasKey(f => f.idFornecedor);
            });

            modelBuilder.Entity<Produto>(e =>
            {
                e.ToTable("Produtos");
                e.HasKey(p => p.IdProduto);
                e.Property(p => p.NomeProduto).HasMaxLength(200).IsRequired();
                e.Property(p => p.Preco).HasColumnType("decimal(18,2)");
                e.Property(p => p.Peso).HasColumnType("decimal(10,3)");
                e.Property(p => p.Unidade).HasMaxLength(50);
                e.Property(p => p.DescricaoProduto).HasMaxLength(500);
                e.Property(p => p.CodigoBarras).HasMaxLength(50);
                e.HasIndex(p => p.CodigoBarras).IsUnique();
                e.HasKey(p => p.IdProduto);
                e.HasOne(p => p.Fornecedor)
                    .WithMany(f => f.Produtos)
                    .HasForeignKey(p => p.FornecedorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Servico>(e =>
            {
                e.ToTable("Servicos");
                e.HasKey(s => s.IdServico);
                e.Property(s => s.TipoServico).HasMaxLength(100).IsRequired();
                e.Property(s => s.DescricaoServico).HasMaxLength(500);
                e.Property(s => s.Preco).HasPrecision(18, 2);
            });

            modelBuilder.Entity<Cupom>(e =>
            {
                e.ToTable("Cupons");
                e.HasKey(c => c.idCupom);
                e.Property(c => c.CodigoCupom).HasMaxLength(50).IsRequired();
                e.Property(c => c.Desconto).HasPrecision(18, 2);
                e.HasIndex(c => c.CodigoCupom).IsUnique();
            });

            modelBuilder.Entity<Pedido>(e =>
            {
                e.ToTable("Pedidos");
                e.HasKey(p => p.idPedido);
                e.HasOne(p => p.Cliente)
                    .WithMany(c => c.Pedidos)
                    .HasForeignKey(p => p.ClienteId)
                    .OnDelete(DeleteBehavior.Restrict);
 
                e.HasOne(p => p.Cupom)
                    .WithMany()
                    .HasForeignKey(p => p.CupomId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.Pagamento)
                    .WithOne(pag => pag.Pedido)
                    .HasForeignKey<Pagamento>(pag => pag.PedidoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.Property(p => p.Subtotal).HasPrecision(18, 2);
                e.Property(p => p.Desconto).HasPrecision(18, 2);
                e.Property(p => p.Status).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<ItemPedido>(e =>
            {
                e.ToTable("ItensPedido", t =>
                {
                    t.HasCheckConstraint("CK_ItensPedido_ProdutoOuServico",
                    "([ProdutoId] IS NOT NULL AND [ServicoId] IS NULL) OR ([ProdutoId] IS NULL AND [ServicoId] IS NOT NULL)");

                    t.HasCheckConstraint("CK_ItensPedido_Quantidade", "Quantidade > 0");

                    t.HasCheckConstraint("CK_ItensPedido_Valores", "PrecoUnitario >= 0 AND Subtotal >= 0");
                });
                e.HasKey(i => i.Id);
                e.Property(i => i.PrecoUnitario).HasPrecision(18, 2);
                e.Property(i => i.Subtotal).HasPrecision(18, 2);

                e.HasOne(i => i.Pedido)
                    .WithMany(p => p.Itens)
                    .HasForeignKey(i => i.PedidoId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(i => i.Produto)
                    .WithMany()
                    .HasForeignKey(i => i.ProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(i => i.Servico)
                    .WithMany()
                    .HasForeignKey(i => i.ServicoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Pagamento>(e =>
            {
                e.ToTable("Pagamentos");

                e.HasKey(p => p.Id);
                e.HasOne(p => p.Pedido)
                    .WithOne(p => p.Pagamento)
                    .HasForeignKey<Pagamento>(p => p.PedidoId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasIndex(p => p.PedidoId);
                e.Property(p => p.ValorPagamento).HasPrecision(18, 2);
                e.Property(p => p.Status).HasMaxLength(50).IsRequired();
                e.HasDiscriminator<string>("TipoPagamento").HasValue<Boleto>("Boleto").HasValue<Cartao>("Cartao");
            });

            modelBuilder.Entity<Boleto>(e =>
            {
                e.Property(b => b.CodigoBarras).HasMaxLength(100);
                e.Property(b => b.linhaDigitavel).HasMaxLength(100);
            });

            modelBuilder.Entity<Cartao>(e =>
            {
                e.Property(c => c.NomeTitular).HasMaxLength(150);
                e.Property(c => c.bandeira).HasMaxLength(30);
                e.Property(c => c.numeroCartao).HasMaxLength(19);
            });
        }
    }
}