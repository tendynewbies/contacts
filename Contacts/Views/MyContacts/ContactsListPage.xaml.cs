using Contacts.Models;
using Contacts.ViewModels.Contact;
using Xamarin.Forms;

namespace Contacts.Views.MyContacts
{
    public partial class ContactsListPage : ViewPage
    {
        public ContactsListPage()
        {
            InitializeComponent();
            BindingContext = new ContactListPageViewModel();
        }

        private async void OnContactSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedContact = (MyContact)e.SelectedItem;
            if (selectedContact != null)
            {
                await Navigation.PushAsync(new UpdateContactPage(selectedContact));
            }
        }
    }
}
