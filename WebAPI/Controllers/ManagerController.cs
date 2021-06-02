using Loyalto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Attributes;
using WebAPI.Commands;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("demo/v1/api/[controller]")]
    [ApiKey(Key = "manager")]
    public class ManagerController : Controller
    {
        private readonly string AUTH_NOT_PROVIDED = "AUTH was not provided";
        private readonly string AUTH = "Authorization";
        private readonly string INVALID_INPUT = "Invalid input param";
        private IManagerService managerService;
        public ManagerController(IManagerService _managerService)
        {
            this.managerService = _managerService;
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
                return await Task.Run(() =>
                {
                    var data = managerService.Invoke("GET", "", auth, "valuta");
                    return Ok(JsonConvert.DeserializeObject(data.Result));
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
                string body = JsonConvert.SerializeObject(valuta);
                return await Task.Run(() =>
                {
                    var data = managerService.Invoke("POST", body, auth, "valuta");
                    return Ok(JsonConvert.DeserializeObject(data.Result));
                });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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
                string body = JsonConvert.SerializeObject(valuta);
                return await Task.Run(() =>
                {
                    var data = managerService.Invoke("PUT", body, auth, "valuta");
                    return Ok(JsonConvert.DeserializeObject(data.Result));
                });
            }
            catch (Exception ex)
            {
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

                var data = managerService.Invoke("DELETE", "", auth, "valuta", id);
                return Ok(JsonConvert.DeserializeObject(data.Result));
            }
            catch (Exception ex)
            {
                return Ok(new FailedReturnValue(ex.Message));
            }
        }
    }
}
