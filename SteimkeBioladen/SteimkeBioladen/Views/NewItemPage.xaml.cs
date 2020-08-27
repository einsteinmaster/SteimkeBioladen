using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SteimkeBioladen.Models;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace SteimkeBioladen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BarcodeScanView.IsScanning = true;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Item.Id))
                MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void CloseBarcodeScan()
        {
            BarcodeScanView.IsScanning = false;
        }

        public static async Task<Item> TryFindItemAsync(string barcode)
        {
            var content = await Get7pFile();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var row = GetRowFromCsv(content, barcode);
                if (row != null)
                {
                    var rowList = row.Split(';');
                    if (rowList[1] == barcode)
                    {
                        var name = rowList[3];
                        var price = rowList[11];
                        Item it = new Item();
                        it.Id = barcode;
                        it.Price = price;
                        it.Text = name;
                        return it;
                    }
                }
            }
            content = await Get19pFile();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var row = GetRowFromCsv(content, barcode);
                if (row != null)
                {
                    var rowList = row.Split(';');
                    if (rowList[1] == barcode)
                    {
                        var name = rowList[3];
                        var price = rowList[11];
                        Item it = new Item();
                        it.Id = barcode;
                        it.Price = price;
                        it.Text = name;
                        return it;
                    }
                }
            }
            return null;
        }

        public static Task<string> Get7pFile()
        {
            string requri_7p = "http://rkp.intecelektro.de/steimkebioladen/Elkershausen%20Preisliste%20Excel%207%25%202.18.csv";
            return GetHttpFile(requri_7p);
        }

        public static Task<string> Get19pFile()
        {
            string requri_19p = "http://rkp.intecelektro.de/steimkebioladen/Elkershausen%20Preisliste%20Excel%2019%25%202.18.csv";
            return GetHttpFile(requri_19p);
        }

        public static async Task<string> GetHttpFile(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Debug.WriteLine("No Successcode response");
                    Debug.WriteLine(response.ToString());
                    throw new FileNotFoundException(uri);
                }
            }
        }

        public static string GetRowFromCsv(string file, string barcode)
        {
            var contentstring = file;
            var stringlist = contentstring.Split('\n');
            foreach (var it in stringlist)
            {
                var rowList = it.Split(';');
                if (rowList[1] == barcode)
                {
                    var name = rowList[3];
                    var price = rowList[11];
                    Debug.WriteLine("Entry found in DB: " + name + "   price: " + price);
                    return it;
                }
            }
            return null;
        }

        private async void BarcodeScanView_OnScanResult(ZXing.Result result)
        {
            Debug.WriteLine("Barcode: " + result.Text);
            BarcodeScanView.IsScanning = false;

            try
            {
                Item = await TryFindItemAsync(result.Text);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception while read DB");
                Debug.WriteLine(exc.ToString());
            }

            //MainThread.BeginInvokeOnMainThread(async () =>
            //{
            //});

            Cancel_Clicked(this, null);
        }
    }
}