using Loyalto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public interface IManagerService
    {
        Task<string> Invoke(string Method, string Body, string Token, string Key, int Id = 0);
    }
}
