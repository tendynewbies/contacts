using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Models;
using Contacts.Views.Home;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels.Contact
{
    public class ContactListPageViewModel : ViewModel
    {
        public ReactiveCommand AddContactCommand { get; set; } = new ReactiveCommand();
        private HamburgerMenuPage RootPage { get => Application.Current.MainPage as HamburgerMenuPage; }
        private HomeMenuItem addContactMenuItem;

        public ContactListPageViewModel()
        {
            AddContactCommand.Subscribe(async () => await OnExecuteAddContactCommand());
        }

        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.ContactList;
        }

        private async Task OnExecuteAddContactCommand()
        {
            if(null == addContactMenuItem)
            {
                addContactMenuItem = new HomeMenuItem()
                {
                    Id = MenuItemType.AddContact,
                    Title = UIConstants.AddContact
                };
            }
            await RootPage.NavigateFromMenu(addContactMenuItem);
        }
    }
}
