using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Commands
{
    public class RefreshConfigHandler : INotificationHandler<RefreshConfig>
    {
        private ILookupDataRetriever lookupDataRetriver;
        public RefreshConfigHandler(ILookupDataRetriever lookupDataRetriver)
        {
            this.lookupDataRetriver = lookupDataRetriver;
        }

        public Task Handle(RefreshConfig notification, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                lookupDataRetriver.Refresh();
            });
        }
    }
}
