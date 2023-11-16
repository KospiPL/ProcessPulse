using ProcessPulse.Class.ProcessPulse.Models;
using ProcessPulse.ServerService.ProcessPulse.Dbcontext;
using System;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Microsoft.EntityFrameworkCore;

namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class FlotaService
    {
        private readonly FlotaDbContext _context;
        private readonly string _oracleConnectionString;

        public FlotaService(FlotaDbContext context, string oracleConnectionString)
        {
            _context = context;
            _oracleConnectionString = oracleConnectionString;
        }

        public async Task CheckAndStoreConnectionStatus()
        {
            bool isFlotaDateAvailable = await IsDateAvailableAsync("SELECT SYSDATE FROM DUAL");
            bool isSafoDateAvailable = await IsDateAvailableAsync("SELECT SYSDATE FROM DUAL@ERP");

            var flotaRecord = new FlotaModel
            {
                FlotaData = isFlotaDateAvailable,
                SafoData = isSafoDateAvailable,
                Date = DateTime.Now
            };

            _context.FlotaModels.Add(flotaRecord);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> IsDateAvailableAsync(string query)
        {
            try
            {
                using (var connection = new OracleConnection(_oracleConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new OracleCommand(query, connection))
                    {
                        var result = await command.ExecuteScalarAsync();
                        return result is DateTime; 
                    }
                }
            }
            catch (Exception)
            {
                return false; 
            }
        }
    }
}
