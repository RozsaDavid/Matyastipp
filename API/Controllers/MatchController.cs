using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController :ControllerBase {
        private readonly MatyaskertContext _dbContext;

        public MatchController(MatyaskertContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet("getMatchWithId/{matchId}")]
        public async Task<ActionResult<Match>> GetMatchWithId(int matchId) {
            Match resultMatch = await _dbContext.Matches.FindAsync(matchId);
            if(resultMatch == null) {
                return NotFound("A mérkőzés nem található.");
            }

            return Ok(resultMatch);
        }
    }
}
