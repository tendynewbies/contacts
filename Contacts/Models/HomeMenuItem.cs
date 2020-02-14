namespace Contacts.Models
{
    public enum MenuItemType
    {
        ContactList,
        FavoriteContacts,
        AddContact
    }

    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }
        public string Title { get; set; }
    }
}
