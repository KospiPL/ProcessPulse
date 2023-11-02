using ProcessPulse.BibliotekaKlas.ProcessPulse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using NLog;

namespace ClientApp_RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        private readonly AppDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, (string, string)> _terminalProcessMap;

        public ProcessController(AppDbContext dbContext, IHttpClientFactory httpClientFactory, Dictionary<string, (string, string)> terminalProcessMap)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpClient = httpClientFactory?.CreateClient() ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _terminalProcessMap = terminalProcessMap ?? throw new ArgumentNullException(nameof(terminalProcessMap));
        }
        [HttpGet("getProcesses")]
        public async Task<IActionResult> GetProcesses(string terminalId)
        {
            var process = await _dbContext.ProcessInfos
                                          .Where(p => p.TerminalId == terminalId)
                                          .OrderByDescending(p => p.Time)
                                          .FirstOrDefaultAsync();
            if (process != null)
            {
                return Ok(process);
            }
            else
            {
                return NotFound("Nie znaleziono procesu dla danego terminalu");
            }
        }

        [HttpGet("getLastTenRecords")]
        public async Task<IActionResult> GetLastTenRecords(string terminalId)
        {
            var records = await _dbContext.ProcessInfos
                                          .Where(p => p.TerminalId == terminalId)
                                          .OrderByDescending(p => p.Time)
                                          .Take(10)
                                          .ToListAsync();
            if (records != null && records.Count > 0)
            {
                return Ok(records);
            }
            else
            {
                return NotFound("Nie znaleziono rekordów dla danego terminalu");
            }
        }


    }
}


