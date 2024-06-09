using DapperMemoryCache.DTO;
using DapperMemoryCache.IRepos;
using DapperMemoryCache.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperMemoryCache.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgrammerController : ControllerBase
    {
        private readonly IProgrammerRepos repos;
        public ProgrammerController(IProgrammerRepos repos)
        {
            this.repos = repos;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Programmer>> CreateNewProgrammer(CreateProgrammerDto programmerDto)
        {
            return Ok(await this.repos.CreateProgrammer(programmerDto));
        }
    }
}
