using DapperMemoryCache.DTO;
using DapperMemoryCache.IRepos;
using DapperMemoryCache.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperMemoryCache.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgrammerController : ControllerBase
    {
        private readonly IProgrammerRepos _repos;
        public ProgrammerController(IProgrammerRepos repos)
        {
            _repos = repos;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateNewProgrammer([FromBody] CreateProgrammerDto programmerDto)
        {
            var create = await _repos.CreateProgrammer(programmerDto);
            return CreatedAtAction(nameof(GetProgrammerId), new { idProgrammer = create.Id }, create);
        }
        [HttpGet("{idProgrammer}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Programmer>> GetProgrammerId(int idProgrammer)
        {
            return Ok(await _repos.GetByIdProgrammer(idProgrammer));
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteProgrammer(int id)
        {
            await _repos.Delete(id);
            return Ok();
        }
    }
}
