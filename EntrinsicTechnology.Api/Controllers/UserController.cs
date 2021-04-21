using EntrinsicTechnology.Data;
using EntrinsicTechnology.Data.Domains;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace EntrinsicTechnology.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        public UserController(BankContext context) : base(context)
        {
        }

        public IActionResult Post([FromBody]User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
