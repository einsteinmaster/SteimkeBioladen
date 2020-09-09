using SteimkeBioladen.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SteimkeBioladen.Services
{
    public interface IItemDataStore
    {
        Task<IEnumerable<Item>> GetItemsAsync(bool refresh);
        Task<bool> AddItemAsync(Item it);
        Task<bool> DeleteItemAsync(string key);
    }
}
