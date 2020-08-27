using System;

using SteimkeBioladen.Models;

namespace SteimkeBioladen.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;
        }

        internal void SetAmount(string v)
        {
            Item.Amount = v;
            OnPropertyChanged("Item");
        }
    }
}
