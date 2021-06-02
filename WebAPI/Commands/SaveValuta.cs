using Loyalto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public class SaveValuta : IRequest<Object>
    {
        public Valuta Valuta { get; private set; }

        public SaveValuta(Valuta valuta)
        {
            Valuta = valuta;
        }
    }
}
