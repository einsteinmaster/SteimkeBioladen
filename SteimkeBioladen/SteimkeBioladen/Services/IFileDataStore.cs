using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteimkeBioladen.Services
{
    public interface IFileDataStore
    {
        Task<string> GetFile(string path);
        Task Update();
        Task<bool> Exists(string path);
        Task SaveFile(string path, string content);
    }
}
