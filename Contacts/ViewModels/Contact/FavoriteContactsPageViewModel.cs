using System.Threading.Tasks;
using Contacts.Helpers;

namespace Contacts.ViewModels.Contact
{
    public class FavoriteContactsPageViewModel : ViewModel
    {
        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.FavoriteContacts;
        }
    }
}
