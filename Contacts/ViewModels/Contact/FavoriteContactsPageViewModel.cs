using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Models;
using Contacts.Services.Data;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels.Contact
{
    public class FavoriteContactsPageViewModel : ViewModel
    {
        private readonly IDataService dataService;
        public ReactiveProperty<List<MyContact>> FavoriteContactList { get; set; } = new ReactiveProperty<List<MyContact>>(new List<MyContact>());
        public ReactiveProperty<bool> HasData { get; set; } = new ReactiveProperty<bool>(false);

        public FavoriteContactsPageViewModel()
        {
            dataService = DependencyService.Get<IDataService>();
        }

        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.FavoriteContacts;
            await GetFavoriteContactList();
        }

        private async Task GetFavoriteContactList()
        {
            var favoriteList = await dataService.GetFavoriteContacts();
            if (null == favoriteList)
            {
                HasData.Value = false;
                FavoriteContactList.Value = new List<MyContact>();
            }
            else
            {
                HasData.Value = true;
                FavoriteContactList.Value = favoriteList;
            }
        }
    }
}
