using Microsoft.EntityFrameworkCore;
using ProcessPulse.Class.ProcessPulse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.Class.Service
{
    public interface ITerminalService
    {
        Task<List<TerminalMapping>> GetTerminalsAsync();
        Task AddTerminalAsync(TerminalMapping terminal);
        Task UpdateTerminalAsync(TerminalMapping terminal);
        Task<TerminalMapping> GetTerminalAsync(int id);
        Task DeleteTerminalAsync(int id);
    }

    public class TerminalService : ITerminalService
    {
        private readonly AppDbContext _dbContext;

        public TerminalService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TerminalMapping> GetTerminalAsync(int id)
        {
            return await _dbContext.TerminalMapping.FindAsync(id);
        }
        public async Task<List<TerminalMapping>> GetTerminalsAsync()
        {
            return await _dbContext.TerminalMapping.ToListAsync();
        }

        public async Task AddTerminalAsync(TerminalMapping terminal)
        {
            await _dbContext.TerminalMapping.AddAsync(terminal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTerminalAsync(TerminalMapping terminal)
        {
            _dbContext.TerminalMapping.Update(terminal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTerminalAsync(int id)
        {
            var terminal = await _dbContext.TerminalMapping.FindAsync(id);
            if (terminal != null)
            {
                _dbContext.TerminalMapping.Remove(terminal);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}
