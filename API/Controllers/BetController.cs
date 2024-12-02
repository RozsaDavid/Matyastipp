using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BetController :ControllerBase {
        private readonly MatyaskertContext _dbContext;

        public BetController(MatyaskertContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet("userBets/{userId}")]
        public async Task<ActionResult<List<Bet>>> GetUserBets(int userId) {

            List<Bet> usersAllBets = new List<Bet>();
            var bets = _dbContext.Bets.ToList();

            foreach(var bet in bets) {
                if(bet.UserId == userId) {
                    Contest actualContest = new Contest();
                    Match actualMatch = new Match();

                    actualContest = await _dbContext.Contests.FindAsync(bet.ContestId);
                    actualMatch = await _dbContext.Matches.FindAsync(bet.MatchId);

                    if(actualContest.EndDate > DateTime.Now &&
                        actualMatch.Date > DateTime.Now && actualMatch.IsAvailable == 1 &&
                        actualMatch.GuestGoals == -1 && actualMatch.HomeGoals == -1) {
                        bet.Match = null;
                        bet.Contest = null;
                        bet.User = null;
                        usersAllBets.Add(bet);
                    }
                }
            }

            return Ok(usersAllBets);
        }

        [HttpPost("addBet")]
        public async Task<ActionResult> AddBet(string[] newBetData) {

            Bet newBet = new Bet();
            newBet.MatchId = Int32.Parse(newBetData[0]);
            newBet.ContestId = Int32.Parse(newBetData[1]);
            newBet.UserId = Int32.Parse(newBetData[2]);
            newBet.IsWon = SByte.Parse(newBetData[3]);
            newBet.GuestGoals = Int32.Parse(newBetData[4]);
            newBet.HomeGoals = Int32.Parse(newBetData[5]);

            Match targetedMatch = await _dbContext.Matches.FindAsync(newBet.MatchId);

            if(targetedMatch.Date < DateTime.Now || targetedMatch.IsAvailable == 0) {
                return BadRequest("Már nem lehet fogadni erre a mérkőzésre!");
            }

            User actualUser = await _dbContext.Users.FindAsync(newBet.UserId);
            Contest actualContest = await _dbContext.Contests.FindAsync(newBet.ContestId);

            newBet.Contest = actualContest;
            newBet.User = actualUser;
            newBet.Match = targetedMatch;

            _dbContext.Bets.Add(newBet);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("getBetWithId/{betId}")]
        public async Task<ActionResult<Bet>> GetBetWithId(int betId) {
            Bet resultBet = await _dbContext.Bets.FindAsync(betId);
            if(resultBet == null) {
                return NotFound("A tipp nem található.");
            }

            return Ok(resultBet);
        }

        [HttpPut]
        [Route("update/{betId}")]
        public async Task<ActionResult> UpdateBet(int betId, int[] editBetData) {
            var selectBet = await _dbContext.Bets.FindAsync(betId);
            if(selectBet == null) {
                return NotFound("A tipp nem található.");
            }

            Match targetedMatch = await _dbContext.Matches.FindAsync(selectBet.MatchId);

            if(targetedMatch.Date < DateTime.Now || targetedMatch.IsAvailable == 0) {
                return BadRequest("Már nem lehet fogadni erre a mérkőzésre!");
            } else {
                selectBet.HomeGoals = editBetData[0];
                selectBet.GuestGoals = editBetData[1];
            }

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("getBetWithAllIds/{contestId}/{userId}/{matchId}")]
        public async Task<ActionResult<Bet>> GetBetWithAllIds(int contestId, int userId, int matchId) {
            var bets = _dbContext.Bets.ToList();

            foreach(var bet in bets)
                if(contestId == bet.ContestId && userId == bet.UserId && matchId == bet.MatchId)
                    return Ok(bet);

            return BadRequest();
        }
    }
}

