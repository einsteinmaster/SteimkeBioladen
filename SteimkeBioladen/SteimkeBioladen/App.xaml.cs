using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SteimkeBioladen.Services;
using SteimkeBioladen.Views;
using Xamarin.Essentials;

namespace SteimkeBioladen
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            VersionTracking.Track();
            DependencyService.Register<DataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
