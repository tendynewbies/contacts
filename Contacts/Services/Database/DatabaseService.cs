using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Contacts.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;

namespace Contacts.Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private SQLiteAsyncConnection sqliteAsyncConnection;
        private SQLiteAsyncConnection DBConnection
        {
            get
            {
                if (sqliteAsyncConnection == null)
                {
                    sqliteAsyncConnection = new SQLiteAsyncConnection(DatabaseFilePath);
                }
                return sqliteAsyncConnection;
            }
        }

        public string DatabaseName
        {
            get
            {
                return "PuneetJi.db3";
            }
        }

        public string DatabaseFilePath
        {
            get
            {
                string appFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseName);
                return appFolderPath;
            }
        }

        //+++START CREATE++++//
        public async Task CreateTables()
        {
            await DBConnection.CreateTableAsync<MyContact>();
        }

        public async Task<int> InsertItem<T>(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await DBConnection.InsertAsync(item);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        //+++END CREATE++++//


        //+++START READ++++//
        public async Task<List<T>> GetAllItems<T>() where T : new()
        {
            await _semaphore.WaitAsync();
            try
            {

                return await DBConnection.GetAllWithChildrenAsync<T>();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<List<T>> GetItemsbyQuery<T>(Expression<Func<T, bool>> expression) where T : new()
        {
            await _semaphore.WaitAsync();
            try
            {
                var value = DBConnection.Table<T>().Where(expression);
                if (value != null)
                {
                    return await value.ToListAsync();
                }
                return null;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> FindItem<T>(int id) where T : new()
        {
            await _semaphore.WaitAsync();
            try
            {
                return await DBConnection.FindWithChildrenAsync<T>(id);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        //+++END READ++++//

        //+++START UPDATE++++//
        public async Task<int> UpdateItem<T>(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await DBConnection.UpdateAsync(item);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<int> InsertOrReplace<T>(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await DBConnection.InsertOrReplaceAsync(item);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        //+++END UPDATE++++//

        //+++START DELETE++++//
        public async Task<int> DeleteItem<T>(T item)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await DBConnection.DeleteAsync(item);
            }
            finally
            {
                _semaphore.Release();
            }
        }
        //+++END DELETE++++//
    }
}