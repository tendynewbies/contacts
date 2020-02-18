using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contacts.Models;
using Contacts.Services.Database;
using Xamarin.Forms;

namespace Contacts.Services.Data
{
    public class DataService : IDataService
    {
        private readonly IDatabaseService dbService;

        public DataService()
        {
            if(null == dbService)
            {
                dbService = DependencyService.Get<IDatabaseService>();
            }
        }

        public async Task<int> AddContact(MyContact contact)
        {
            return await dbService.InsertItem(contact);
        }

        public async Task<List<MyContact>> GetAllContacts()
        {
            var contactList = await dbService.GetAllItems<MyContact>();
            return contactList.OrderBy(contact => contact.Name.ToUpper()).ToList();
        }

        public async Task<List<MyContact>> GetFavoriteContacts()
        {
            var favoriteContactList = await dbService.GetItemsbyQuery<MyContact>(contact => contact.IsFavourite == true);
            return favoriteContactList.OrderBy(contact => contact.Name.ToUpper()).ToList();
        }

        public async Task<int> UpdateContact(MyContact contact)
        {
            return await dbService.UpdateItem(contact);
        }

        public async Task<int> DeleteContact(MyContact contact)
        {
            return await dbService.DeleteItem(contact);
        }
    }
}
