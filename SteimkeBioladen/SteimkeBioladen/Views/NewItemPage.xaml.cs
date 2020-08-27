using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SteimkeBioladen.Models;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Net.Http;

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

        private void BarcodeScanView_OnScanResult(ZXing.Result result)
        {
            Debug.WriteLine("Barcode: " + result.Text);
            BarcodeScanView.IsScanning = false;
            string requri = "http://opengtindb.org/?ean=";
            requri += result.Text.Trim();
            requri += "&cmd=query&queryid=400000000";
            Item item = new Item();
            item.Id = result.Text;
            item.Text = "loading...";
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    int errorcode = 0;
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.GetAsync(requri);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = response.Content;
                            var contentstring = await content.ReadAsStringAsync();
                            var stringlist = contentstring.Split('\n');
                            foreach (var it in stringlist)
                            {
                                if (it.StartsWith("error="))
                                {

                                    if (int.TryParse(it.Substring(it.IndexOf('=') + 1), out errorcode))
                                    {
                                        if (errorcode == 1)
                                        {
                                            item.Text = "Item not found in Database";
                                        }
                                        if (errorcode == 5)
                                        {
                                            item.Text = "Database limit exeeded";
                                        }
                                        else if (errorcode != 0)
                                        {
                                            item.Text = "Database errorcode=" + errorcode;
                                        }
                                    }
                                }
                                if (it.StartsWith("name="))
                                {
                                    item.Text = it.Substring(it.IndexOf('=') + 1);
                                }
                                if (it.StartsWith("detailname="))
                                {
                                    item.Description = it.Substring(it.IndexOf('=') + 1);
                                }
                            }
                        }
                        else
                        {
                            item.Text = "not found";
                        }
                    }
                    if (errorcode == 0)
                    {
                        await DisplayAlert("Barcode found", item.ToString(), "ok");
                        this.Item = item;
                        Save_Clicked(this, null);
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Database error", item.ToString(), "ok");
                    }
                }
                catch (Exception exc)
                {
                    await DisplayAlert("Exception while scanning", exc.ToString(), "ok");
                }
                Cancel_Clicked(this, null);
            });
        }
    }
}