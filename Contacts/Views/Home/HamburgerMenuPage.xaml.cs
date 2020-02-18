using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Models;
using Contacts.Views.MyContacts;
using Xamarin.Forms;

namespace Contacts.Views.Home
{
    public partial class HamburgerMenuPage : MasterDetailPage
    {
        private Dictionary<MenuItemType, NavigationPage> MenuPages = new Dictionary<MenuItemType, NavigationPage>();
        public HamburgerMenuPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            MenuPages.Add(MenuItemType.ContactList, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(HomeMenuItem menuItem)
        {
            if (!MenuPages.ContainsKey(menuItem.Id))
            {
                switch (menuItem.Id)
                {
                    case MenuItemType.ContactList:
                        MenuPages.Add(menuItem.Id, new NavigationPage(new ContactsListPage()));
                        break;
                    case MenuItemType.FavoriteContacts:
                        MenuPages.Add(menuItem.Id, new NavigationPage(new FavoriteContactsPage()));
                        break;
                    case MenuItemType.AddContact:
                        MenuPages.Add(menuItem.Id, new NavigationPage(new AddContactPage()));
                        break;
                }
            }

            NavigationPage newPage = MenuPages[menuItem.Id];
            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(100);
                }
            }

            IsPresented = false;
        }
    }
}
