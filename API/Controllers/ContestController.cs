using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController :ControllerBase {
        private readonly MatyaskertContext _dbContext;

        public ContestController(MatyaskertContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Contest>>> GetAllContest() {
            var contests = _dbContext.Contests.ToList();

            return Ok(contests);
        }
    }
}
