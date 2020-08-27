using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
