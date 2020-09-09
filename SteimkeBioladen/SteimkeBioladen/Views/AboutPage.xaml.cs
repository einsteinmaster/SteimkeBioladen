using SteimkeBioladen.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SteimkeBioladen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        AboutViewModel viewModel;
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await viewModel.UpdateDB();
            }
            catch (Exception exc)
            {
                await DisplayAlert("Exception at update", exc.ToString(), "ok");
            }
        }
    }
}