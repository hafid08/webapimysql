using Loyalto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public class SaveValutaHandler : IRequestHandler<SaveValuta, Object>
    {
        private readonly DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions;
        private readonly IMediator mediator;

        public SaveValutaHandler(DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions, IMediator mediator)
        {
            this.mediator = mediator;
            this.dbLoyaltoContextOptions = dbLoyaltoContextOptions;
        }

        public async Task<object> Handle(SaveValuta request, CancellationToken cancellationToken)
        {
            using (LoyaltoContext messagingContext = new LoyaltoContext(dbLoyaltoContextOptions))
            {
                if (request.Valuta.vid <= 0)
                {
                    messagingContext.Valuta.Add(request.Valuta);
                }
                else
                {
                    messagingContext.Valuta.Attach(request.Valuta).State = EntityState.Modified;
                }
                await messagingContext.SaveChangesAsync();

                // sync configurationData on cache/memory
                await mediator.Publish(new RefreshConfig());
                return messagingContext.Valuta.Where(c => c.vid == request.Valuta.vid).FirstOrDefault();
            }
        }
    }
}
