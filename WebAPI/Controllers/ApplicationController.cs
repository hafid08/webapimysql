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
using WebAPI.Commands;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("demo/v1/api/")]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator mediator;
        public ApplicationController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new FailedReturnValue("Invalid UserName or Password."));
            }
            try
            {
                Login loginRequest = new Login(request, HttpContext);
                return await mediator.Send(loginRequest);
            }
            catch (Exception ex)
            {
                return Ok(new FailedReturnValue(ex.Message));
            }
        }
    }
}
