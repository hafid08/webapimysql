using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Commands
{
    public class Login : IRequest<IActionResult>
    {
        public LoginRequest LoginRequest { get; private set; }
        public HttpContext HttpContext { get; set; }
        public Login(LoginRequest request, HttpContext httpContext)
        {
            this.HttpContext = httpContext;
            LoginRequest = request;
        }
    }
}
