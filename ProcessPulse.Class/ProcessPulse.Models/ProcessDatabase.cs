using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ProcessPulse.BibliotekaKlas.ProcessPulse.Models;

namespace ProcessPulse.BibliotekaKlas.ProcessPulse.Models
{
    public interface IProcessDatabase
    {
        Task<string> GetProcessNameByTerminalIdAsync(string terminalId);
    }

    public class ProcessDatabase : IProcessDatabase
    {
        private readonly AppDbContext _dbContext;

        public ProcessDatabase(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetProcessNameByTerminalIdAsync(string terminalId)
        {
            var terminalMapping = await _dbContext.TerminalMapping.FirstOrDefaultAsync(t => t.TerminalId == terminalId);
            return terminalMapping?.Name;
        }
    }
}