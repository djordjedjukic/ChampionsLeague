namespace Api.Controllers
{
    using App.Features.Rankings;
    using Microsoft.AspNetCore.Mvc;
    using Paramore.Brighter;
    using Paramore.Darker;

    [Route("api/[table]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly IQueryProcessor query;
        private readonly IAmACommandProcessor command;

        public TableController(IQueryProcessor query, IAmACommandProcessor command)
        {
            this.query = query;
            this.command = command;
        }

        /// <summary>
        /// Get league table and standings by query
        /// </summary>
        [HttpGet]
        public IActionResult GetTables(GetRankings.Query query)
        {
            return Ok(this.query.Execute(query));
        }
    }
}