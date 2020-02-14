using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Models;
using Contacts.Views.Home;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels.Menu
{
    public class MenuPageViewModel : ViewModel
    {
        public List<HomeMenuItem> MenuItems { get; set; }
        public ReactiveCommand<HomeMenuItem> SelectMenuItemCommand { get; set; } = new ReactiveCommand<HomeMenuItem>();
        private HamburgerMenuPage RootPage { get => Application.Current.MainPage as HamburgerMenuPage; }

        public MenuPageViewModel()
        {
            PopulateMenuList();
            SelectMenuItemCommand.Subscribe(async(x) => await OnExecuteSelectMenuItemCommand(x));
        }

        private void PopulateMenuList()
        {
            if(null == MenuItems)
            {
                MenuItems = new List<HomeMenuItem>();
                HomeMenuItem menuItem = new HomeMenuItem()
                {
                    Id = MenuItemType.ContactList,
                    Title = UIConstants.ContactList
                };
                MenuItems.Add(menuItem);
                menuItem = new HomeMenuItem()
                {
                    Id = MenuItemType.FavoriteContacts,
                    Title = UIConstants.FavoriteContacts
                };
                MenuItems.Add(menuItem);
                menuItem = new HomeMenuItem()
                {
                    Id = MenuItemType.AddContact,
                    Title = UIConstants.AddContact
                };
                MenuItems.Add(menuItem);
            }
        }

        private async Task OnExecuteSelectMenuItemCommand(HomeMenuItem selectedItem)
        {
            await RootPage.NavigateFromMenu(selectedItem);
        }
    }
}
