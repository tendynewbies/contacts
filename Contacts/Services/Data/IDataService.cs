using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Models;

namespace Contacts.Services.Data
{
    public interface IDataService
    {
        Task<int> AddContact(MyContact contact);
        Task<int> UpdateContact(MyContact contact);
        Task<List<MyContact>> GetAllContacts();
        Task<List<MyContact>> GetFavoriteContacts();
        Task<int> DeleteContact(MyContact contact);
    }
}
