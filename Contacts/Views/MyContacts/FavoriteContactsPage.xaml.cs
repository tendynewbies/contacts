using Contacts.Models;
using Contacts.ViewModels.Contact;
using Xamarin.Forms;

namespace Contacts.Views.MyContacts
{
    public partial class FavoriteContactsPage : ViewPage
    {
        public FavoriteContactsPage()
        {
            InitializeComponent();
            BindingContext = new FavoriteContactsPageViewModel();
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
