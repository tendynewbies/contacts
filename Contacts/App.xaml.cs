using Contacts.Services;
using Contacts.Services.Data;
using Contacts.Services.Database;
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
            DependencyService.Register<DataService>();
            DependencyService.Register<DatabaseService>();
            MainPage = new HamburgerMenuPage();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await DependencyService.Get<IDatabaseService>().CreateTables();
        }
    }
}
