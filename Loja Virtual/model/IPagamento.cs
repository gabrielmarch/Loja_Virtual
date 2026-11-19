namespace Loja_Virtual.model
{
    public interface IPagamento
    {
        decimal ValorPagamento { get; }
        DateTime DataPagamento { get; }
        string Status { get; }
    }
}
