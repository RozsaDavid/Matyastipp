using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController :ControllerBase {
        private readonly MatyaskertContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserController(MatyaskertContext dbContext, IConfiguration configuration) {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("getUserWithToken")]
        public async Task<ActionResult<User>> GetUserWithToken() {
            User resultUser = new User();

            var users = _dbContext.Users.ToList();

            foreach(var user in users)
                if(user.Username == User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString())
                    resultUser = user;

            return Ok(resultUser);
        }

        [Authorize]
        [HttpGet("getUserWithId/{userId}")]
        public async Task<ActionResult<User>> GetUserWithId(int userId) {
            User resultUser = await _dbContext.Users.FindAsync(userId);
            if(resultUser == null) {
                return NotFound("A felhasználó nem található.");
            }

            return Ok(resultUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(string[] userData) {
            string username = userData[0];
            string password = userData[1];

            var selectUser = await _dbContext.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            if(selectUser is null)
                return Unauthorized("Hibás felhasználónév és/vagy jelszó.");
            if(selectUser.IsActive != 1)
                return Unauthorized("Inaktív felhasználó.\nLépjen kapcsolatba a rendszergazdával.");
            if(password == selectUser.Password) {
                var authClaim = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub,selectUser.Username!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:Expire"]!)),
                    claims: authClaim,
                    signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                    ));
                return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized("Hibás felhasználónév és/vagy jelszó.");
        }

        [HttpPost("registration")]
        public async Task<ActionResult> AddUser(User newUser) {
            var users = _dbContext.Users.ToList();

            foreach(var user in users) {
                if(newUser.Username == user.Username) {
                    return BadRequest("Foglalt felhasználónév!");
                } else if(newUser.Email == user.Email) {
                    return BadRequest("Foglalt e-mailcím!");
                }
            }

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User updateUser) {
            var users = _dbContext.Users.ToList();

            foreach(var user in users) {
                if(user.Id != id) {
                    if(updateUser.Username == user.Username) {
                        return BadRequest("Foglalt felhasználónév!");
                    } else if(updateUser.Email == user.Email) {
                        return BadRequest("Foglalt e-mailcím!");
                    }
                }
            }

            var selectUser = await _dbContext.Users.FindAsync(id);
            if(selectUser == null) {
                return NotFound("A felhasználó nem található.");
            } else {
                selectUser.Username = updateUser.Username;
                selectUser.RealName = updateUser.RealName;
                selectUser.Email = updateUser.Email;
                selectUser.Password = updateUser.Password;
            }
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("inactivate/{id}")]
        public async Task<ActionResult<List<User>>> InactivateUser(int id, [FromBody] User updateUser) {
            var selectUser = await _dbContext.Users.FindAsync(id);
            if(selectUser == null) {
                return NotFound("A felhasználó nem található.");
            } else {
                selectUser.IsActive = 0;
            }
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
