using Contacts.Services;
using Contacts.Views;
using Contacts.Views.Home;
using Xamarin.Forms;

namespace Contacts
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new HamburgerMenuPage();
        }
    }
}
