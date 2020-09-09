using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SteimkeBioladen.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string AppVersion
        {
            get
            {
                return "Version: "+VersionTracking.CurrentVersion + " (" + VersionTracking.CurrentBuild + ")";
            }
        }
        public AboutViewModel()
        {
            Title = "About";
        }

        public Task UpdateDB()
        {
            return DataStore.Update();
        }
    }
}