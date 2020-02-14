using System.Threading.Tasks;
using Contacts.Helpers;

namespace Contacts.ViewModels.Contact
{
    public class ContactListPageViewModel : ViewModel
    {
        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.ContactList;
        }
    }
}
