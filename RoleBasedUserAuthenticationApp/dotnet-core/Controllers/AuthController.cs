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
                User userDb = await GetUserDbByEmail(user.Email);
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
                User userDb = await GetUserDbByEmail(user.Email);

                bool userExist = userDb != null;
                if (userExist) return Conflict("user already exists"); // HTTP:409

                BindUserToTenant(user);

                bool userHasPassword = !string.IsNullOrEmpty(user.Password);
                if (userHasPassword) user.Password = TokenHelper.Encrypt(user.Password); // Hash password

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                user.Token = TokenHelper.GenerateJWTToken(user.Id, user.RememberMe);

                user.Role = await GetUserRoleById(user.RoleId);

                EmailHelper.SendWelcomeEmail(user.Fullname, user.Email);

                await transaction.CommitAsync();

                return Created("", user);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex);
            }
        }

        private async Task<Role> GetUserRoleById(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
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
                User userDb = await GetUserDbByEmail(user.Email);
                if (userDb == null) return NotFound();

                string recoveryToken = TokenHelper.GenerateJWTTokenByEmail(userDb.Email);
                string recoveryLink = $"{user.AppOriginUrl}/auth/reset-password/{recoveryToken}";

                bool isSent = EmailHelper.SendRecoveryLinkEmail(recoveryLink, userDb.Fullname, userDb.Email);
                if (!isSent) return StatusCode(500, "mail could not be sent");

                return Created("", new { recoveryToken, userDb.Email });
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
                User userDb = await GetUserDbByEmail(user.Email);
                bool userExists = userDb != null;
                if (!userExists) return BadRequest();

                userDb.Password = TokenHelper.Encrypt(user.Password);
                _context.Users.Update(userDb);
                await _context.SaveChangesAsync();
                return Ok(userDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<User> GetUserDbByEmail(string email)
        {
            return await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}