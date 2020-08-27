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
using SteimkeBioladen.ViewModels;

namespace SteimkeBioladen.Views
{
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel viewModel;
        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewItemViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BarcodeScanView.IsScanning = true;
        }
        protected override void OnDisappearing()
        {
            BarcodeScanView.IsScanning = false;
            base.OnDisappearing();
        }
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            BarcodeScanView.IsScanning = false;
            await Navigation.PopModalAsync();
        }
        private async void BarcodeScanView_OnScanResult(ZXing.Result result)
        {
            Debug.WriteLine("Barcode: " + result.Text);
            BarcodeScanView.IsScanning = false;
            try
            {
                var itemFound = await viewModel.ProcessScanResult(result.Text);
                if (itemFound)
                {
                    Debug.WriteLine("Item found");
                    MessagingCenter.Send(this, "AddItem", viewModel.ScannedItem);
                    return;
                }
                else
                {
                    Debug.WriteLine("Item not found");
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception while read DB");
                Debug.WriteLine(exc.ToString());
            }
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Item not found", "Item not found", "ok");
            });
            Cancel_Clicked(this, null);
        }
    }
}