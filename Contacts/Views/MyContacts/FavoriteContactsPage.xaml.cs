
using Contacts.ViewModels.Contact;

namespace Contacts.Views.MyContacts
{
    public partial class FavoriteContactsPage : ViewPage
    {
        public FavoriteContactsPage()
        {
            InitializeComponent();
            BindingContext = new FavoriteContactsPageViewModel();
        }
    }
}
