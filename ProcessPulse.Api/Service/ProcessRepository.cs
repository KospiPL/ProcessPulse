using ProcessPulse.BibliotekaKlas.ProcessPulse.Models;
using Microsoft.EntityFrameworkCore;

namespace ProcessPulse.Api.Service
{
    public interface IProcessRepository
    {
        Task<ProcessInfo> GetProcessByTerminalIdAsync(string terminalId);
        Task<List<ProcessInfo>> GetLastTenRecordsByTerminalIdAsync(string terminalId);
    }

    public class ProcessRepository : IProcessRepository
    {
        private readonly AppDbContext _dbContext;

        public ProcessRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProcessInfo> GetProcessByTerminalIdAsync(string terminalId)
        {
            return await _dbContext.ProcessInfos
                .Where(p => p.TerminalId == terminalId)
                .OrderByDescending(p => p.Time)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProcessInfo>> GetLastTenRecordsByTerminalIdAsync(string terminalId)
        {
            return await _dbContext.ProcessInfos
                .Where(p => p.TerminalId == terminalId)
                .OrderByDescending(p => p.Time)
                .Take(10)
                .ToListAsync();
        }
    }
}
