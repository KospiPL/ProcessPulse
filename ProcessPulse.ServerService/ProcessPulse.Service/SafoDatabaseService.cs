using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;


namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class SafoDatabaseService
    {
        private readonly SafoDbContext _dbContext;
        public SafoDatabaseService(SafoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GetSydateFromSafo()
        {
            // Implementacja zapytania do bazy danych SAFO
            var result = await _dbContext.Database.ExecuteSqlRawAsync("SELECT SYSDATE FROM DUAL@ERP");
            return result.ToString();
        }
    }

}
