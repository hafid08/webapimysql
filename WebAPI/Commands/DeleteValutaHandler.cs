using Loyalto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public class DeleteValutaHandler : IRequestHandler<DeleteValuta, Object>
    {
        private readonly DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public DeleteValutaHandler(DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions, IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.dbLoyaltoContextOptions = dbLoyaltoContextOptions;
            this.configuration = configuration;
        }

        public async Task<object> Handle(DeleteValuta request, CancellationToken cancellationToken)
        {
            using (LoyaltoContext loyaltoContext = new LoyaltoContext(dbLoyaltoContextOptions))
            {
                var valuta = loyaltoContext.Valuta.Where(c => request.Id.Equals(c.vid)).FirstOrDefault();
                loyaltoContext.Valuta.Remove(valuta);
                await loyaltoContext.SaveChangesAsync();

                // sync configurationData on cache/memory
                await mediator.Publish(new RefreshConfig());

                return new { Id = valuta.vid };
            }
        }
    }
}
