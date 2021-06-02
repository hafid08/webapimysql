using Loyalto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.Commands;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("demo/v1/api/[controller]")]
    [ApiKey(Key = "valuta")]
    [ApiController]
    public class ValutaController : ControllerBase
    {
        private DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions;
        private readonly IMediator mediator;
        private OnMemoryLookupData lookup;
        private readonly string AUTH_NOT_PROVIDED = "AUTH was not provided";
        private readonly string AUTH = "Authorization";
        private readonly string INVALID_INPUT = "Invalid input param";
        public ValutaController(IMediator mediator, DbContextOptions<LoyaltoContext> dbLoyaltoContextOptions, ILookupDataRetriever lookupDataRetriver)
        {
            lookup = lookupDataRetriver.GetLookupData();
            this.dbLoyaltoContextOptions = dbLoyaltoContextOptions;
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                if (!HttpContext.Request.Headers.TryGetValue(AUTH, out var auth))
                {
                    return Ok(new FailedReturnValue(AUTH_NOT_PROVIDED));
                }
                if(!new AuthAttribute(lookup).CheckingAuth(auth, out var reason))
                {
                    return Ok(new FailedReturnValue(reason));
                }

                return await Task.Run(() =>
                {
                    LoyaltoContext messagingContext = new LoyaltoContext(dbLoyaltoContextOptions);
                    var valuta = messagingContext.Valuta.ToList();
                    return Ok(new SuccessReturnValue { Data = valuta });
                });
            }
            catch (Exception ex)
            {
                return Ok(new FailedReturnValue(ex.Message));
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Valuta valuta)
        {
            if (valuta == null)
            {
                return Ok(new FailedReturnValue(INVALID_INPUT));
            }
            try
            {
                if (!HttpContext.Request.Headers.TryGetValue(AUTH, out var auth))
                {
                    return Ok(new FailedReturnValue(AUTH_NOT_PROVIDED));
                }
                if (!new AuthAttribute(lookup).CheckingAuth(auth, out var reason))
                {
                    return Ok(new FailedReturnValue(reason));
                }
                SaveValuta saved = new SaveValuta(valuta);
                dynamic data = await mediator.Send(saved);
                return Ok(new SuccessReturnValue { Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new FailedReturnValue(ex.Message));
            }
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Valuta valuta)
        {
            if (valuta == null)
            {
                return Ok(new FailedReturnValue(INVALID_INPUT));
            }
            try
            {
                if (!HttpContext.Request.Headers.TryGetValue(AUTH, out var auth))
                {
                    return Ok(new FailedReturnValue(AUTH_NOT_PROVIDED));
                }
                if (!new AuthAttribute(lookup).CheckingAuth(auth, out var reason))
                {
                    return Ok(new FailedReturnValue(reason));
                }

                LoyaltoContext messagingContext = new LoyaltoContext(dbLoyaltoContextOptions);
                if (!messagingContext.Valuta.Any(x => x.vid.Equals(valuta.vid)))
                {
                    return Ok(new FailedReturnValue { Message = "Data not found." });
                }
                SaveValuta saved = new SaveValuta(valuta);
                dynamic data = await mediator.Send(saved);
                return Ok(new SuccessReturnValue { Data = data });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return Ok(new FailedReturnValue(ex.Message));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (id == 0)
            {
                return Ok(new FailedReturnValue(INVALID_INPUT));
            }
            try
            {
                if (!HttpContext.Request.Headers.TryGetValue(AUTH, out var auth))
                {
                    return Ok(new FailedReturnValue(AUTH_NOT_PROVIDED));
                }
                if (!new AuthAttribute(lookup).CheckingAuth(auth, out var reason))
                {
                    return Ok(new FailedReturnValue(reason));
                }

                DeleteValuta deleted = new DeleteValuta(id);
                dynamic data = await mediator.Send(deleted);
                return Ok(new SuccessReturnValue { Data = data });
            }
            catch (Exception ex)
            {
                return Ok(new FailedReturnValue(ex.Message));
            }
        }
    }
}
