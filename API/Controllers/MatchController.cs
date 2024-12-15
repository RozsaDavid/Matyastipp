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

        [HttpGet("openedMatches/{contestId}/{userId}")]
        public async Task<ActionResult<List<Match>>> GetOpenedMatches(int contestId, int userId) {

            var matches = _dbContext.Matches.ToList();
            var bets = _dbContext.Bets.ToList();
            var inContests = _dbContext.Incontests.ToList();

            var allOpenedMatchesForUserInContest = new List<Match>();

            foreach(var inContest in inContests)
                if(inContest.ContestId == contestId) {
                    Match actualMatch = new Match();
                    actualMatch = await _dbContext.Matches.FindAsync(inContest.MatchId);

                    if(actualMatch.IsAvailable == 1 && actualMatch.Date > DateTime.Now
                        && actualMatch.GuestGoals == -1 && actualMatch.HomeGoals == -1) {
                        bool isThereAUserBet = false;

                        foreach(var bet in bets) {
                            if(bet.MatchId == actualMatch.Id && bet.UserId == userId)
                                isThereAUserBet = true;
                        }

                        if(!isThereAUserBet) {
                            actualMatch.Bets = null;
                            allOpenedMatchesForUserInContest.Add(actualMatch);
                        }
                    }
                }

            return Ok(allOpenedMatchesForUserInContest);
        }

        [HttpGet("closedMatches/{contestId}")]
        public async Task<ActionResult<List<Match>>> GetClosedMatches(int contestId) {
            List<Match> closedMatchesInContest = new List<Match>();
            var incontests = _dbContext.Incontests.ToList();

            foreach(var incontest in incontests)
                if(incontest.ContestId == contestId) {
                    var actualMatch = await _dbContext.Matches.FindAsync(incontest.MatchId);
                    if(actualMatch != null && actualMatch.HomeGoals != -1 && actualMatch.GuestGoals != -1)
                        closedMatchesInContest.Add(actualMatch);
                }
            return Ok(closedMatchesInContest);
        }
    }
}
