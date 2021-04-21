using EntrinsicTechnology.Data;
using EntrinsicTechnology.Data.Domains;
using EntrinsicTechnology.Data.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;

namespace EntrinsicTechnology.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiControllerBase
    {
        public AccountController(BankContext context) : base(context)
        {
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Account acct)
        {
            try
            {
                if (IsAdmin())
                {
                    _context.Accounts.Add(acct);
                    _context.SaveChanges();

                    return Ok(acct);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                User user = GetAuthenticatedUser();
                if (user != null)
                {
                    _context.Entry(user).Collection(x => x.Accounts).Load();
                    return new JsonResult(user.Accounts);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody]TransactionDto dto)
        {
            try
            {
                if (dto.Amount <= 0) return BadRequest();

                User user = GetAuthenticatedUser();
                if (user != null)
                {
                    _context.Entry(user).Collection(x => x.Accounts).Load();
                    Account account = user.Accounts.SingleOrDefault(x => x.Id == dto.AccountId);

                    if (account != null)
                    {
                        account.Balance += dto.Amount;
                        _context.SaveChanges();

                        return Ok(account);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody]TransactionDto dto)
        {
            try
            {
                if (dto.Amount <= 0) return BadRequest();

                User user = GetAuthenticatedUser();
                if (user != null)
                {
                    _context.Entry(user).Collection(x => x.Accounts).Load();
                    Account account = user.Accounts.SingleOrDefault(x => x.Id == dto.AccountId);

                    if (account != null)
                    {
                        if (account.Balance > dto.Amount)
                        {
                            account.Balance -= dto.Amount;
                            _context.SaveChanges();
                            return Ok(account);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

        }
    }
}
