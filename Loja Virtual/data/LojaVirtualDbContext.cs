using Loja_Virtual.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja_Virtual.data
{
    public class LojaVirtualDbContext : DbContext
    {
            
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<PessoaFisica> PessoasFisicas { get; set; }

        public DbSet<PessoaJuridica> PessoasJuridicas { get; set; }

        
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


            modelBuilder.Entity<Cliente>()
                .ToTable("Clientes");

            modelBuilder.Entity<Cliente>()
                .HasDiscriminator<string>("TipoCliente")
                .HasValue<PessoaFisica>("PessoaFisica")
                .HasValue<PessoaJuridica>("PessoaJuridica");
        }
    }
}
