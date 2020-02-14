using System.Threading.Tasks;
using Contacts.Helpers;

namespace Contacts.ViewModels.Contact
{
    public class AddContactPageViewModel : ViewModel
    {
        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.AddContact;
        }
    }
}
