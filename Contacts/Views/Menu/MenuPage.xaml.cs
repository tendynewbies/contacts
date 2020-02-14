
using Contacts.ViewModels.Menu;
using Xamarin.Forms;

namespace Contacts.Views.Menu
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new MenuPageViewModel();
        }
    }
}
