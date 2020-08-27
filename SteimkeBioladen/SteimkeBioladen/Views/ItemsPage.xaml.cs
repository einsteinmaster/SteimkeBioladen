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

namespace SteimkeBioladen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
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

            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }

        private async void BuyItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                string timestamp = DateTime.Now.ToString();
                // create body
                string body = "Hallo Corrie,\nHier mein einkauf vom "+timestamp+"\n";
                body += await viewModel.GetItemListAsString();
                body += "\n\n";

                // create mail
                List<string> recipients = new List<string>();
                recipients.Add("lagerplatz@gut-steimke.de");
                var message = new EmailMessage
                {
                    Subject = "Einkauf vom " + timestamp ,
                    To = recipients,
                    Body = body,
                };
                await Email.ComposeAsync(message);
            }catch(Exception exc)
            {

            }
        }
    }
}