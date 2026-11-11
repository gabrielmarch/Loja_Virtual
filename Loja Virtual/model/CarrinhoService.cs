using System.Collections.ObjectModel;
using System.Linq;

namespace Loja_Virtual.services
{
    public class ItemCarrinho
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int Quantidade { get; set; } = 1;

        public decimal Subtotal => Preco * Quantidade;
    }

    public static class CarrinhoService
    {
        public static ObservableCollection<ItemCarrinho> Itens { get; }
            = new ObservableCollection<ItemCarrinho>();

        public static void Adicionar(int id, string nome, decimal preco)
        {
            var item = Itens.FirstOrDefault(p => p.Id == id);

            if (item != null)
            {
                item.Quantidade++;
            }
            else
            {
                Itens.Add(new ItemCarrinho
                {
                    Id = id,
                    Nome = nome,
                    Preco = preco,
                    Quantidade = 1
                });
            }
        }

        public static void Remover(ItemCarrinho item)
        {
            Itens.Remove(item);
        }
    }
}
