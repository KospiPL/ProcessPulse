using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using ProcessPulse.Class.ProcessPulse.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProcessPulse.Api.ProcessPulse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlotaController : ControllerBase
    {
        private readonly FlotaDbContext _context;

        public FlotaController(FlotaDbContext context)
        {
            _context = context;
        }

        // GET: api/flota
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlotaModel>>> GetFlotaData()
        {
            return await _context.FlotaModels.ToListAsync();
        }
    }

}
