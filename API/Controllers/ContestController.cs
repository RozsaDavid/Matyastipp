using API.DAL.Data;
using API.DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContestController :ControllerBase {
        private readonly MatyaskertContext _dbContext;

        public ContestController(MatyaskertContext dbContext) {
            _dbContext = dbContext;
        }

        [HttpGet("getContestWithId/{contestId}")]
        public async Task<ActionResult<List<Contest>>> GetContestsWithId(int contestId) {
            var actualContest = await _dbContext.Contests.FindAsync(contestId);

            return Ok(actualContest);
        }

        [HttpGet("userContests/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetUserContests(int userId) {
            List<Contest> usersAllContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(usersAllContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id))
                    usersAllContests.Add(contest);
            }

            return Ok(usersAllContests);
        }

        [HttpGet("userOngoingContests/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetUserOngoingContests(int userId) {
            List<Contest> usersAllContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(usersAllContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id) &&
                    contest.StartDate < DateTime.Now && contest.EndDate > DateTime.Now)
                    usersAllContests.Add(contest);
            }

            return Ok(usersAllContests);
        }

        [HttpGet("availableUpcomingContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetAvailableUpcomingContests(int userId) {
            List<Contest> upcomingContests = new List<Contest>();

            List<int> availableContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            var contests = _dbContext.Contests.ToList();

            foreach(var contest in contests) {
                if(contest.StartDate > DateTime.Now && contest.IsOpened == 1)
                    availableContestsIds.Add(contest.Id);
            }

            foreach(var standing in standings) {
                if(standing.UserId == userId && availableContestsIds.Contains(standing.ContestId))
                    availableContestsIds.Remove(standing.ContestId);
            }

            if(availableContestsIds.Count == 0)
                return Ok(upcomingContests);


            foreach(var contest in contests) {
                if(availableContestsIds.Contains(contest.Id))
                    upcomingContests.Add(contest);
            }

            return Ok(upcomingContests);
        }

        [HttpGet("upcomingContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetUpcomingContests(int userId) {
            List<Contest> upcomingContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(upcomingContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id))
                    if(contest.StartDate > DateTime.Now)
                        upcomingContests.Add(contest);
            }

            return Ok(upcomingContests);
        }

        [HttpGet("availableOngoingContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetAvailableOngoingContests(int userId) {
            List<Contest> upcomingContests = new List<Contest>();

            List<int> availableContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            var contests = _dbContext.Contests.ToList();

            foreach(var contest in contests) {
                if(contest.StartDate < DateTime.Now && contest.IsOpened == 1)
                    availableContestsIds.Add(contest.Id);
            }

            foreach(var standing in standings) {
                if(standing.UserId == userId && availableContestsIds.Contains(standing.ContestId))
                    availableContestsIds.Remove(standing.ContestId);
            }

            if(availableContestsIds.Count == 0)
                return Ok(upcomingContests);


            foreach(var contest in contests) {
                if(availableContestsIds.Contains(contest.Id))
                    upcomingContests.Add(contest);
            }

            return Ok(upcomingContests);
        }

        [HttpGet("ongoingContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetOngoingContests(int userId) {
            List<Contest> ongoingContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(ongoingContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id))
                    if(contest.StartDate < DateTime.Now && contest.EndDate > DateTime.Now)
                        ongoingContests.Add(contest);
            }

            return Ok(ongoingContests);
        }

        [HttpGet("pastContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetPastContests(int userId) {
            List<Contest> pastContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(pastContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id))
                    if(contest.EndDate < DateTime.Now)
                        pastContests.Add(contest);
            }

            return Ok(pastContests);
        }

        [HttpGet("standing/{contestId}")]
        public async Task<ActionResult<List<string>>> GetContestStanding(int contestId) {
            List<string> contestsStandingsList = new List<string>();

            var standings = _dbContext.Standings.ToList();
            var users = _dbContext.Users.ToList();
            foreach(var standing in standings)
                if(standing.ContestId == contestId)
                    foreach(var user in users)
                        if(standing.UserId == user.Id)
                            contestsStandingsList.Add(user.Username + ";" + standing.Points.ToString());


            if(contestsStandingsList.Count() == 0)
                return BadRequest();

            return Ok(contestsStandingsList);
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

                    if(actualMatch.IsAvailable == 1) {
                        bool isThereAUserBet = false;

                        foreach(var bet in bets) {
                            if(bet.MatchId == actualMatch.Id && bet.UserId == userId)
                                isThereAUserBet = true;
                        }

                        if(!isThereAUserBet)
                            allOpenedMatchesForUserInContest.Add(actualMatch);
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

        [HttpPost("signContest")]
        public async Task<ActionResult> SignContest(string[] signOnData) {
            Standing standing = new Standing();
            standing.UserId = Int32.Parse(signOnData[0]);
            standing.ContestId = Int32.Parse(signOnData[1]);
            standing.Points = 0;
            standing.User = await _dbContext.Users.FindAsync(standing.UserId);
            standing.Contest = await _dbContext.Contests.FindAsync(standing.ContestId);

            if(standing.Contest.IsOpened == 0)
                return BadRequest("A tippverseny nincs nyitva.");

            var standings = _dbContext.Standings.ToList();

            foreach(var standingFor in standings) {
                if(standingFor.ContestId == standing.ContestId && standingFor.UserId == standing.UserId)
                    return BadRequest("A felhasználó már nevezett a tippversenyre.");
            }

            _dbContext.Standings.Add(standing);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
