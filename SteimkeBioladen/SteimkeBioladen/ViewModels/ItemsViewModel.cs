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
            body += await GetItemListAsString();
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