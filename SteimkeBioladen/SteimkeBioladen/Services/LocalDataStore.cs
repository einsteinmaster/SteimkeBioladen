using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SteimkeBioladen.Services
{
    class LocalDataStore : IFileDataStore
    {
        List<string> filePaths;
        public LocalDataStore()
        {
            filePaths = new List<string>();
        }
        private string GetFQP(string path)
        {
            return Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, path);
        }
        public Task SaveFile(string path, string content)
        {
            var newPath = GetFQP(path);
            return Task.Run(() =>
            {
                for (int cnt = 0; cnt < 100; cnt++)
                {
                    try
                    {
                        File.WriteAllText(newPath, content);
                        filePaths.Add(path);
                        return;
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                    }
                }
            });
        }
        public Task<string> GetFile(string path)
        {
            var newPath = GetFQP(path);
            return Task.Run<string>(() =>
            {
                const int MAX_TRY = 100;
                for (int cnt = 0; cnt < MAX_TRY; cnt++)
                {
                    try
                    {
                        return (File.ReadAllText(newPath));
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                        if (cnt >= MAX_TRY - 1)
                            throw;
                    }
                }
                throw new InvalidOperationException("did not return value");
            });
        }

        public Task Update()
        {
            return Task.Run(() =>
            {
                const int MAX_TRY = 100;
                for (int cnt = 0; cnt < MAX_TRY; cnt++)
                {
                    try
                    {
                        foreach (var path in filePaths)
                        {
                            string fqp = GetFQP(path);
                            if (File.Exists(fqp))
                                File.Delete(fqp);
                        }
                        filePaths.Clear();
                        return;
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc);
                        if (cnt >= MAX_TRY - 1)
                            throw;
                    }
                }
                throw new InvalidOperationException("did not return");
            });
        }

        public Task<bool> Exists(string path)
        {
            string fqp = GetFQP(path);
            bool exists = File.Exists(fqp);
            if (exists && !filePaths.Contains(path)) 
                filePaths.Add(path);
            return Task.FromResult(exists);
        }
    }
}
