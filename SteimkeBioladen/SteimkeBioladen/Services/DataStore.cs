using SteimkeBioladen.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SteimkeBioladen.Services
{
    public class DataStore : IDataStore
    {
        BufferedDataStore bds;
        NetworkDataStore nds;
        LocalDataStore lds;
        MockDataStore mds;

        public const string FILE_LIST_7P = "Elkershausen Preisliste Excel 7% 2.18.csv";
        public const string FILE_LIST_19P = "Elkershausen Preisliste Excel 19% 2.18.csv";

        public DataStore()
        {
            nds = new NetworkDataStore();
            lds = new LocalDataStore();
            bds = new BufferedDataStore(lds,nds);
            mds = new MockDataStore();
        }

        public Task<bool> AddItemAsync(Item it)
        {
            return mds.AddItemAsync(it);
        }

        public Task<bool> DeleteItemAsync(string key)
        {
            return mds.DeleteItemAsync(key);
        }

        public Task<bool> Exists(string path)
        {
            return bds.Exists(path);
        }

        public Task<string> GetFile(string path)
        {
            return bds.GetFile(path);
        }

        public Task<IEnumerable<Item>> GetItemsAsync(bool refresh)
        {
            return mds.GetItemsAsync(refresh);
        }

        public Task SaveFile(string path, string content)
        {
            return lds.SaveFile(path, content);
        }

        public async Task Update()
        {
            // to make shure it deletes these lists
            await lds.Exists(FILE_LIST_19P);
            await lds.Exists(FILE_LIST_7P);
            await bds.Update();
        }
    }
}
