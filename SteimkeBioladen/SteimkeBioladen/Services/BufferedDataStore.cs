using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SteimkeBioladen.Services
{
    class BufferedDataStore : IFileDataStore
    {
        IFileDataStore dataStore1;
        IFileDataStore dataStore2;
        Dictionary<string, string> buffer;
        public BufferedDataStore(IFileDataStore data1,IFileDataStore data2 = null)
        {
            if (data1 is null) throw new ArgumentNullException(nameof(data1));
            dataStore1 = data1;
            dataStore2 = data2;
            buffer = new Dictionary<string, string>();
        }

        public async Task<bool> Exists(string path)
        {
            return await dataStore1.Exists(path) || (dataStore2 != null && await dataStore2.Exists(path));
        }

        public async Task<string> GetFile(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            if (buffer.ContainsKey(path))
                return buffer[path];

            var data = "";
            if (await dataStore1.Exists(path))
            {
                data = await dataStore1.GetFile(path);
            }else if (dataStore2 != null && await dataStore2.Exists(path))
            {
                data = await dataStore2.GetFile(path);
                await dataStore1.SaveFile(path, data);
            }
            else
            {
                throw new FileNotFoundException(path);
            }
             
            buffer.Add(path, data);
            return data;
        }

        public Task SaveFile(string path, string content)
        {
            if (dataStore2 != null)
                return dataStore2.SaveFile(path, content);
            throw new NotImplementedException();
        }

        public async Task Update()
        {
            buffer.Clear();
            await dataStore1.Update();
            if (dataStore2 != null)
                await dataStore2.Update();
        }
    }
}
