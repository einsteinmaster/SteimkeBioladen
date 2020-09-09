using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SteimkeBioladen.Models;
using SteimkeBioladen.Views;
using SteimkeBioladen.ViewModels;
using Xamarin.Essentials;
using System.Diagnostics;

namespace SteimkeBioladen.Views
{
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items != null && viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }

        private async void BuyItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var email = new EmailMessage
                {
                    Subject = await viewModel.GetEmailSubject(),
                    To = await viewModel.GetEmailRecipients(),
                    Body = await viewModel.CreateBuyItemsEmail(),
                };
                await Email.ComposeAsync(email);
            }
            catch(Exception exc)
            {
                Debug.WriteLine("Exception while buy items");
                Debug.WriteLine(exc.ToString());
                await DisplayAlert("Error while buy items", exc.ToString(), "ok");
            }
        }
    }
}