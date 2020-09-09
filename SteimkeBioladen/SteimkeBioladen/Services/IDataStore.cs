using System;
using System.Collections.Generic;
using System.Text;

namespace SteimkeBioladen.Services
{
    public interface IDataStore : IFileDataStore,IItemDataStore
    {
    }
}
