using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController :ControllerBase {
        private readonly MatyaskertContext _dbContext;

        public UserController(MatyaskertContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser() {
            var users = _dbContext.Users.ToList();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user) {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return Ok(await _dbContext.Users.ToListAsync());
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, [FromBody] User updateUser) {
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
            return Ok(await _dbContext.Users.ToListAsync());
        }

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
            return Ok(await _dbContext.Users.ToListAsync());
        }
    }
}
