using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using startup_kit_api.Models;
using startup_kit_api.Common;

namespace startup_kit_api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly DatabaseContext _context;

        public AuthController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("signin")]
        public async Task<ActionResult<User>> SignIn([FromBody] User user)
        {
            try
            {
                var userDb = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == user.Email);
                bool userExist = userDb != null;

                if (!userExist) return NotFound();

                bool credentialsOK = TokenHelper.IsCredentialsOK(user.Password, userDb.Password);
                if (!credentialsOK) return BadRequest();
                
                userDb.Token = TokenHelper.GenerateJWTToken(userDb.Id, user.RememberMe);
                return Ok(userDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("signup")]
        public async Task<ActionResult<User>> SignUp([FromBody] User user)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                User userDb = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == user.Email);
                bool userExist = userDb != null;
                if (userExist) return Conflict(); // HTTP:409

                BindUserToTenant(user);

                if (!string.IsNullOrEmpty(user.Password)) user.Password = TokenHelper.Encrypt(user.Password); // Hash password

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                user.Token = TokenHelper.GenerateJWTToken(user.Id, user.RememberMe);

                user.Role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);

                await EmailHelper.SendWelcomeEmail(user.Fullname, user.Email);

                await transaction.CommitAsync();

                return Created("", user);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex);
            }
        }

        private static void BindUserToTenant(User user)
        {
            Tenant tenant = new Tenant()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user.Tenant = tenant;
            user.CreatedAt = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
        }

        [HttpPost("send-recovery-link")]
        public async Task<IActionResult> SendRecoveryLink([FromBody] User user)
        {
            try
            {
                var dbuser = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == user.Email);
                if (dbuser != null)
                {
                    string recoveryToken = TokenHelper.GenerateJWTTokenByEmail(dbuser.Email);
                    string recoveryLink = $"{user.AppOriginUrl}/auth/reset-password/{recoveryToken}";
                    bool isSent = await EmailHelper.SendRecoveryLinkEmail(recoveryLink, dbuser.Fullname, dbuser.Email);
                    if (isSent) return Created("", new { recoveryToken, dbuser.Email });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut("reset-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] User user)
        {
            try
            {
                var dbuser = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == user.Email);
                bool userExists = dbuser != null;
                if (userExists)
                {
                    dbuser.Password = TokenHelper.Encrypt(user.Password);
                    _context.Users.Update(dbuser);
                    await _context.SaveChangesAsync();
                    return Ok(dbuser);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}