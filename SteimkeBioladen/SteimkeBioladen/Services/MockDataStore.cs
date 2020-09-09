using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteimkeBioladen.Models;

namespace SteimkeBioladen.Services
{
    public class MockDataStore : IDataStore
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public Task<string> GetFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(string path)
        {
            throw new NotImplementedException();
        }

        public Task SaveFile(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}