using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;

namespace WebAPI.Attributes
{
    public class AuthAttribute
    {
        private OnMemoryLookupData lookup;
        public AuthAttribute(OnMemoryLookupData data)
        {
            lookup = data;
        }
        public bool CheckingAuth(string auth, out string reason)
        {
            
            int pid = 0;
            if(!int.TryParse(auth.ToString().Split('-')[0], out pid))
            {
                reason = "Token is not valid";
                return false;
            } else if (pid <= 0)
            {
                reason = "Token is not valid";
                return false;
            }
            var toqen = lookup.GetToqen(pid);
            if (toqen == null)
            {
                reason = "Token is not found";
                return false;
            }
            else if (!toqen.token.Equals(auth))
            {
                reason = "Token is not valid";
                return false;
            }
            else if (toqen.texpired < DateTime.Now)
            {
                reason = "Token is expired";
                return false;
            }
            reason = "Token is active";
            return true;
        }
    }
}
