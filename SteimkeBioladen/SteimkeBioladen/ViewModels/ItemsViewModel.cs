using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using SteimkeBioladen.Models;
using SteimkeBioladen.Views;
using System.Collections.Generic;

namespace SteimkeBioladen.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public double TotalPrice
        {
            get
            {
                double sum = 0.0;
                foreach (var it in Items)
                {
                    sum += it.GetTotalPrice();
                }
                return sum;
            }
        }
        public double Tax7p
        {
            get
            {
                double sum = 0.0;
                foreach (var it in Items)
                {
                    if (it.Tax == TaxClass.p7)
                    {
                        sum += it.GetTotalPrice() * 0.07;
                    }
                }
                return sum;
            }
        }
        public double Tax19p
        {
            get
            {
                double sum = 0.0;
                foreach (var it in Items)
                {
                    if (it.Tax == TaxClass.p19)
                    {
                        sum += it.GetTotalPrice() * 0.19;
                    }
                }
                return sum;
            }
        }
        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });

            MessagingCenter.Subscribe<ItemDetailPage, Item>(this, "DeleteItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Remove(newItem);
                await DataStore.DeleteItemAsync(newItem.Id);
            });
        }

        public async Task<string> GetItemListAsString()
        {
            string strlist = "";
            var items = await DataStore.GetItemsAsync(true);
            foreach (var item in items)
            {
                strlist += item.ToString();
            }
            return strlist;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task<string> CreateBuyItemsEmail()
        {
            string timestamp = DateTime.Now.ToString();
            // create body
            string body = "Hallo Corrie,\nHier mein einkauf vom " + timestamp + "\n";
            body += await GetItemListAsString() + "\n";
            body += "Gesamtpreis: " + TotalPrice + "\n";
            body += "Steuern 7%: " + Tax7p + "\t 19%: " + Tax19p + "\n";
            body += "\n\n";
            return body;
        }

        public Task<List<string>> GetEmailRecipients()
        {
            // create mail
            List<string> recipients = new List<string>();
            recipients.Add("lagerplatz@gut-steimke.de");
            return Task.FromResult(recipients);
        }

        public Task<string> GetEmailSubject()
        {
            string timestamp = DateTime.Now.ToString();
            return Task.FromResult("Einkauf vom " + timestamp);
        }
    }
}