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
    public class SafoController : ControllerBase
    {
        private readonly SafoDbContext _context;

        public SafoController(SafoDbContext context)
        {
            _context = context;
        }

        // GET: api/safo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SafoModel>>> GetSafoData()
        {
            return await _context.SafoModels.ToListAsync();
        }
    }

}
