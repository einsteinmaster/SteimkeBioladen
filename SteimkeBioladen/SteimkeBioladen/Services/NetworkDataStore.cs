using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteimkeBioladen.Services
{
    class NetworkDataStore : IFileDataStore
    {
        const string baseUrl = "http://rkp.intecelektro.de/steimkebioladen/";
        public Task<bool> Exists(string path)
        {
            return Task.FromResult(true);
        }

        private string EncodePath(string path)
        {
            path.Replace("%", "%25");
            path.Replace(" ", "%20");
            return path;
        }

        public async Task<string> GetFile(string path)
        {
            path = EncodePath(path);
            path = baseUrl + path;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    byte[] barr = await response.Content.ReadAsByteArrayAsync();
                    return Encoding.UTF8.GetString(barr);
                }
                else
                {
                    Debug.WriteLine("No Successcode response");
                    Debug.WriteLine(response.ToString());
                    throw new FileNotFoundException(path);
                }
            }
        }

        public Task Update()
        {
            return Task.FromResult(0);
        }

        public Task SaveFile(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}
