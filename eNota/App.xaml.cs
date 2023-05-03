using eNota.Pages;
using Xamarin.Forms;

namespace eNota
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new pgMainMenu());
            MainPage.BindingContext = new pgMainMenuVM();
        }

        protected async override void OnStart()
        {
            await Global.tryRequestStoragePermission();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
