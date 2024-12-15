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

        [HttpGet("userPastContests/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetPastContests(int userId) {
            List<Contest> usersPastContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(usersPastContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id))
                    if(contest.EndDate < DateTime.Now)
                        usersPastContests.Add(contest);
            }

            return Ok(usersPastContests);
        }

        [HttpGet("userOngoingContests/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetUserOngoingContests(int userId) {
            List<Contest> usersOngoingContests = new List<Contest>();

            List<int> usersContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            foreach(var standing in standings) {
                if(standing.UserId == userId)
                    usersContestsIds.Add(standing.ContestId);
            }

            if(usersContestsIds.Count == 0)
                return Ok(usersOngoingContests);

            var contests = _dbContext.Contests.ToList();
            foreach(var contest in contests) {
                if(usersContestsIds.Contains(contest.Id) &&
                    contest.StartDate < DateTime.Now && contest.EndDate > DateTime.Now)
                    usersOngoingContests.Add(contest);
            }

            return Ok(usersOngoingContests);
        }

        [HttpGet("userUpcomingContests/{userId}")]
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

        [HttpGet("availableOngoingContest/{userId}")]
        public async Task<ActionResult<List<Contest>>> GetAvailableOngoingContests(int userId) {
            List<Contest> upcomingContests = new List<Contest>();

            List<int> availableContestsIds = new List<int>();

            var standings = _dbContext.Standings.ToList();
            var contests = _dbContext.Contests.ToList();

            foreach(var contest in contests) {
                if(contest.StartDate < DateTime.Now &&
                    contest.EndDate > DateTime.Now && contest.IsOpened == 1)
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

        [HttpPost("signContest")]
        public async Task<ActionResult> SignContest(string[] signUpData) {
            Standing standing = new Standing();
            standing.UserId = Int32.Parse(signUpData[0]);
            standing.ContestId = Int32.Parse(signUpData[1]);
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
