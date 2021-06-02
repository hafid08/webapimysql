using Loyalto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Commands
{
    public class LoginHandler : IRequestHandler<Login, IActionResult>
    {
        private LoyaltoContext loyaltoContext;
        private IConfiguration config;
        private readonly IMediator mediator;
        public LoginHandler(LoyaltoContext loyaltoContext, IConfiguration config, IMediator mediator)
        {
            this.mediator = mediator;
            this.loyaltoContext = loyaltoContext;
            this.config = config;
        }

        public async Task<IActionResult> Handle(Login request, CancellationToken cancellationToken)
        {
            var req = request.LoginRequest;
            var user = loyaltoContext.Pengguna.Where(x => req.puser.Equals(x.puser) && req.ppass.Equals(x.ppass)).FirstOrDefault();
            if(user != null)
            {
                if (user.pstatus == 1)
                {
                    Random rundom = new Random();
                    double tokenExpireDay = double.Parse(config.GetSection("TokenExpireDay").Value);
                    DateTime timeLogin = DateTime.Now;
                    DateTime timeExpired = timeLogin.AddDays(1);
                    string tokenValue = user.pid + "-" + timeLogin.ToString("hhmm") + RandomString(5, true);
                    Toqen toqen = new Toqen();
                    toqen.pid = user.pid;
                    toqen.token = tokenValue;
                    toqen.ttime = timeLogin;
                    toqen.texpired = timeExpired;

                    if (loyaltoContext.Toqen.Any(x => x.pid.Equals(user.pid)))
                    {
                        loyaltoContext.Toqen.Attach(toqen).State = EntityState.Modified;
                    } else
                    {
                        loyaltoContext.Toqen.Add(toqen);
                    }
                    loyaltoContext.SaveChanges();
                    var data = new
                    {
                        userId = user.pid,
                        token = tokenValue,
                        expiration = timeExpired
                    };

                    // sync configurationData on cache/memory
                    await mediator.Publish(new RefreshConfig());
                    return new OkObjectResult(new SuccessReturnValue(data));
                }
                else
                {
                    return new OkObjectResult(new FailedReturnValue { Message = "User is Unactive" });
                }
            } else
            {
                return new OkObjectResult(new FailedReturnValue { Message = "Invalid UserName or Password." });
            }
        }

        public string RandomString(int size, bool lowerCase = false)
        {
            Random _random = new Random();
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
