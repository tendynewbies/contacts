using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contacts.Services.Database
{
    public interface IDatabaseService
    {
        string DatabaseName { get; }
        string DatabaseFilePath { get; }

        Task CreateTables();
        Task<int> InsertItem<T>(T item);

        Task<List<T>> GetAllItems<T>() where T : new();
        Task<T> FindItem<T>(int id) where T : new();
        Task<List<T>> GetItemsbyQuery<T>(Expression<Func<T, bool>> expression) where T : new();

        Task<int> UpdateItem<T>(T item);
        Task<int> InsertOrReplace<T>(T item);

        Task<int> DeleteItem<T>(T item);
    }
}
