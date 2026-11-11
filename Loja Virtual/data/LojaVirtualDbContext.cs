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

        public DbSet<Pagamento> Pagamentos { get; set; } = null!;}



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
                e.Property(f => f.NomeFornecedor).HasMaxLength(200).IsRequired();
                e.Property(f => f.EmailFornecedor).HasMaxLength(100);
                e.Property(f => f.TelefoneFornecedor).HasMaxLength(20);
                e.Property(f => f.CNPJFornecedor).HasMaxLength(18).IsRequired();
                e.HasIndex(f => f.CNPJFornecedor).IsUnique();
            });
               
            modelBuilder.Entity<Produto>(e =>
            {
                e.ToTable("Produtos");
                e.Property(p => p.NomeProduto).HasMaxLength(200).IsRequired();
                e.Property(p => p.Preco).HasColumnType("decimal(18,2)");
                e.Property(p => p.Peso).HasColumnType("decimal(10,3");
                e.Property(p => p.Unidade).HasMaxLength(50);
                e.Property(p => p.DescricaoProduto).HasMaxLength(500);
                e.Property(p => p.CodigoBarras).HasMaxLength(50);
                e.HasIndex(p => p.CodigoBarras).IsUnique();
                e.HasOne(p => p.Fornecedor)
                    .WithMany(f => f.Produtos)
                    .HasForeignKey(p => p.FornecedorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


        }


    }
}
