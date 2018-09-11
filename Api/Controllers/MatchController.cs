namespace Api.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using App.Features.Matches;
    using App.Features.Rankings;
    using Core.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using Paramore.Darker;

    [Produces("application/json")]
    [Route("api/match")]
    [ApiController]
    public class MatchController : Controller
    {
        private readonly IQueryProcessor query;
        private readonly IAmACommandProcessor command;

        public MatchController(IQueryProcessor query, IAmACommandProcessor command)
        {
            this.query = query;
            this.command = command;
        }

        /// <summary>
        /// Get all matches by query
        /// </summary>
        [HttpGet]
        public IActionResult GetMatches([FromQuery]GetMatches.Query query)
        {
            return Ok(this.query.Execute(query));
        }

        /// <summary>
        /// Add or update result of matches
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddUpdateMatches([FromBody]List<Match> matches)
        {
            this.command.Send(new AddUpdateMatches.Command(matches));
            return Ok(this.query.Execute(new GetRankings.Query()));
        }
    }
}