using ProcessPulse.BibliotekaKlas.ProcessPulse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using NLog;
using ProcessPulse.Api.Service;

namespace ClientApp_RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessRepository _processRepository;

        public ProcessController(IProcessRepository processRepository)
        {
            _processRepository = processRepository;
        }

        [HttpGet("getProcesses")]
        public async Task<IActionResult> GetProcesses(string terminalId)
        {
            var process = await _processRepository.GetProcessByTerminalIdAsync(terminalId);

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
            var records = await _processRepository.GetLastTenRecordsByTerminalIdAsync(terminalId);

            if (records.Any())
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


