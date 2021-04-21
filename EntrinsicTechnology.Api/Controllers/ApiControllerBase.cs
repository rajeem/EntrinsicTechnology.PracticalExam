using EntrinsicTechnology.Data;
using EntrinsicTechnology.Data.Domains;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EntrinsicTechnology.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected BankContext _context;
        public ApiControllerBase(BankContext context)
        {
            _context = context;
        }

        protected bool IsAdmin()
        {
            return Request?.Headers["Username"].ToString() == "Admin";
        }

        protected User GetAuthenticatedUser()
        {
            if (Request?.Headers?.ContainsKey("Username") == true)
            {
                return _context.Users.SingleOrDefault(x => x.UserName == Request.Headers["Username"].ToString());
            }

            return null;
        }

        protected IActionResult HandleException(Exception ex)
        {
            string refNum = DateTime.Now.ToString("yyyyMMddThhmmss.ff");

            return new ObjectResult(ex.Message) { StatusCode = 500, Value = $"Reference Number: {refNum}" };
        }
    }
}
