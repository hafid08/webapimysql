using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Commands
{
    public class DeleteValuta : IRequest<Object>
    {
        public int Id { get; private set; }
        public DeleteValuta(int id)
        {
            Id = id;
        }
    }
}
