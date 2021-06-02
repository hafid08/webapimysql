using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public interface ILookupDataRetriever
    {
        OnMemoryLookupData GetLookupData();
        OnMemoryLookupData Refresh();
    }
}
