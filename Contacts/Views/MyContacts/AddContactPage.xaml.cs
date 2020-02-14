using Contacts.ViewModels.Contact;

namespace Contacts.Views.MyContacts
{
    public partial class AddContactPage : ViewPage
    {
        public AddContactPage()
        {
            InitializeComponent();
            BindingContext = new AddContactPageViewModel();
        }
    }
}
