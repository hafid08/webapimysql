using Loyalto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class OnMemoryLookupData
    {

        private readonly DbContextOptions<LoyaltoContext> dbContextOptions;
        private Dictionary<int, Toqen> Toqens;
        public OnMemoryLookupData(DbContextOptions<LoyaltoContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
            InitializeData();
        }
        public void InitializeData()
        {
            using (LoyaltoContext loyaltoContext = new LoyaltoContext(dbContextOptions))
            {
                Toqens = loyaltoContext.Toqen.ToList().Select((s, i) => new { s, i }).ToDictionary(x => x.s.pid, x => x.s);
            }
        }
        public Toqen GetToqen(int pid)
        {
            return Toqens[pid];
        }
    }
}
