using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.model;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Loja_Virtual.data
{
    public class CupomRepositoryAdo : ICupomRepository
    {
        private readonly string _conexao =
            @"Server=(localdb)\MSSQLLocalDB;
              Database=LojaVirtualDb;
              Trusted_Connection=True;
              TrustServerCertificate=True;";
        public async Task<List<Cupom>> ListarAsync()
        {
            var cupons = new List<Cupom>();

            const string sql = @"
                SELECT idCupom,
                       CodigoCupom,
                       Desconto,
                       DataValidade,
                       ativo
                FROM Cupons
                ORDER BY idCupom DESC;";

            using var conexao = new SqlConnection(_conexao);
            await conexao.OpenAsync();

            using var comando = new SqlCommand(sql, conexao);
            using var leitor = await comando.ExecuteReaderAsync();

            while (await leitor.ReadAsync())
            {
                cupons.Add(new Cupom
                {
                    idCupom = leitor.GetInt32(0),
                    CodigoCupom = leitor.GetString(1),
                    Desconto = leitor.GetDecimal(2),
                    DataValidade = leitor.GetDateTime(3),
                    ativo = leitor.GetBoolean(4)
                });
            }

            return cupons;
        }
        public async Task<int> CadastrarAsync(Cupom cupom)
        {
            const string sql = @"
                INSERT INTO Cupons
                    (CodigoCupom, Desconto, DataValidade, ativo)
                OUTPUT INSERTED.idCupom
                VALUES
                    (@codigo, @desconto, @validade, @ativo);";

            using var conexao = new SqlConnection(_conexao);
            await conexao.OpenAsync();

            using var comando = new SqlCommand(sql, conexao);

            AdicionarParametrosCupom(comando, cupom);

            object? resultado = await comando.ExecuteScalarAsync();

            return Convert.ToInt32(resultado);
        }
        public async Task<bool> AtualizarAsync(Cupom cupom)
        {
            const string sql = @"
                UPDATE Cupons
                SET CodigoCupom = @codigo,
                    Desconto = @desconto,
                    DataValidade = @validade,
                    ativo = @ativo
                WHERE idCupom = @idCupom;";

            using var conexao = new SqlConnection(_conexao);
            await conexao.OpenAsync();

            using var comando = new SqlCommand(sql, conexao);

            AdicionarParametrosCupom(comando, cupom);

            comando.Parameters.Add(
                "@idCupom", SqlDbType.Int).Value =
                cupom.idCupom;

            int linhasAfetadas =
                await comando.ExecuteNonQueryAsync();

            return linhasAfetadas > 0;
        }
        public async Task<bool> AlterarStatusAsync(
            int idCupom, bool ativo)
        {
            const string sql = @"
                UPDATE Cupons
                SET ativo = @ativo
                WHERE idCupom = @idCupom;";

            using var conexao = new SqlConnection(_conexao);
            await conexao.OpenAsync();

            using var comando = new SqlCommand(sql, conexao);

            comando.Parameters.Add(
                "@idCupom", SqlDbType.Int).Value = idCupom;

            comando.Parameters.Add(
                "@ativo", SqlDbType.Bit).Value = ativo;

            int linhasAfetadas =
                await comando.ExecuteNonQueryAsync();

            return linhasAfetadas > 0;
        }

        private static void AdicionarParametrosCupom(
            SqlCommand comando, Cupom cupom)
        {
            comando.Parameters.Add(
                "@codigo", SqlDbType.NVarChar, 50).Value =
                cupom.CodigoCupom;

            var parametroDesconto = comando.Parameters.Add(
                "@desconto", SqlDbType.Decimal);

            parametroDesconto.Precision = 18;
            parametroDesconto.Scale = 2;
            parametroDesconto.Value = cupom.Desconto;

            comando.Parameters.Add(
                "@validade", SqlDbType.DateTime2).Value =
                cupom.DataValidade;

            comando.Parameters.Add(
                "@ativo", SqlDbType.Bit).Value =
                cupom.ativo;
        }
    }
}
