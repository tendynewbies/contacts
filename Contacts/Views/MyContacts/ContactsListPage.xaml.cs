using Contacts.ViewModels.Contact;

namespace Contacts.Views.MyContacts
{
    public partial class ContactsListPage : ViewPage
    {
        public ContactsListPage()
        {
            InitializeComponent();
            BindingContext = new ContactListPageViewModel();
        }
    }
}
