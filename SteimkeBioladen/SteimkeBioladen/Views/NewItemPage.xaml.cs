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
                    await Navigation.PopModalAsync();
                    return;
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Item not found", "Item not found", "ok");
                    });
                }
            }
            catch(FileNotFoundException)
            {
                Debug.WriteLine("Filenotfound Exception");
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("FileNotFound Exception", "File is not at server", "ok");
                });
            }
            catch (Exception exc)
            {
                Debug.WriteLine("Exception while read DB");
                Debug.WriteLine(exc.ToString());
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Exception at Barcode proc", exc.ToString(), "ok");
                });
            }
            
            await Navigation.PopModalAsync();
        }
    }
}