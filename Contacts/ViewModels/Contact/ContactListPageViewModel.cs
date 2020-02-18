using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Models;
using Contacts.Services.Data;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels.Contact
{
    public class ContactListPageViewModel : ViewModel
    {
        private readonly IDataService dataService;
        public ReactiveProperty<List<MyContact>> ContactList { get; set; } = new ReactiveProperty<List<MyContact>>(new List<MyContact>());
        public ReactiveProperty<bool> HasData { get; set; } = new ReactiveProperty<bool>(false);

        public ReactiveCommand AddContactCommand { get; set; } = new ReactiveCommand();
        private HomeMenuItem addContactMenuItem;

        public ContactListPageViewModel()
        {
            dataService = DependencyService.Get<IDataService>();
            AddContactCommand.Subscribe(async () => await OnExecuteAddContactCommand());
        }

        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.ContactList;
            await GetContactList();
        }

        private async Task GetContactList()
        {
            var allContactList = await dataService.GetAllContacts();
            if(null == allContactList)
            {
                HasData.Value = false;
                ContactList.Value = new List<MyContact>();
            }
            else
            {
                HasData.Value = true;
                ContactList.Value = allContactList;
            }
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
