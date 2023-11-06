using Microsoft.AspNetCore.Mvc;
using ProcessPulse.Class.Service;
using ProcessPulse.Class.ProcessPulse.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProcessPulse.Class.ProcessPulse.Models;

namespace YourApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalController : ControllerBase
    {
        private readonly ITerminalService _terminalService;

        public TerminalController(ITerminalService terminalService)
        {
            _terminalService = terminalService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TerminalMapping>>> GetTerminals()
        {
            return await _terminalService.GetTerminalsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TerminalMapping>> GetTerminal(int id)
        {
            var terminal = await _terminalService.GetTerminalAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }
            return terminal;
        }

        [HttpPost]
        public async Task<ActionResult> AddTerminal([FromBody] TerminalMapping terminal)
        {
            await _terminalService.AddTerminalAsync(terminal);
            return CreatedAtAction(nameof(GetTerminal), new { id = terminal.Id }, terminal);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTerminal(int id, [FromBody] TerminalMapping terminal)
        {
            if (id != terminal.Id)
            {
                return BadRequest();
            }
            await _terminalService.UpdateTerminalAsync(terminal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTerminal(int id)
        {
            await _terminalService.DeleteTerminalAsync(id);
            return NoContent();
        }
    }
}

