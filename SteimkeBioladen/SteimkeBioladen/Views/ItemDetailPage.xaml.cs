using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SteimkeBioladen.Models;
using SteimkeBioladen.ViewModels;

namespace SteimkeBioladen.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }

        private async void DeleteItem_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
            await Navigation.PopModalAsync();
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            viewModel.SetAmount(((int)e.NewValue).ToString());
        }
    }
}