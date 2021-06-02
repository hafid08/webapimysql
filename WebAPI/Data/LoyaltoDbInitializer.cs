using Loyalto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class LoyaltoDbInitializer : ILoyaltoDbInitializer
    {
        private readonly LoyaltoContext loyaltoContext;

        public LoyaltoDbInitializer(LoyaltoContext context)
        {
            loyaltoContext = context;
        }
        public void InitializeAsync()
        {
            Console.WriteLine("Loyalto DB Initilization...");
            // Make sure database created
            loyaltoContext.Database.CanConnect();

            //loyaltoContext.Database.EnsureDeleted();

            //loyaltoContext.Database.Migrate();

            //seed comment by altro
            //SeedConfigData().Wait();
        }
    }
}
