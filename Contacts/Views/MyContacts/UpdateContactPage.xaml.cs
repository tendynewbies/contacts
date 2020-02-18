using Contacts.Models;
using Contacts.ViewModels.Contact;

namespace Contacts.Views.MyContacts
{
    public partial class UpdateContactPage : ViewPage
    {

        public UpdateContactPage(MyContact contactToUpdate)
        {
            InitializeComponent();
            BindingContext = new UpdateContactPageViewModel(contactToUpdate, Navigation);
        }
    }
}
