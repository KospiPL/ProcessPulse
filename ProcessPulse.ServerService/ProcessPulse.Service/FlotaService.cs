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
            // Pobieranie daty dla Floty
            var dateFlota = await GetDateFromOracleAsync("SELECT SYSDATE FROM DUAL");

            // Pobieranie daty dla SAFO
            var dateSafo = await GetDateFromOracleAsync("SELECT SYSDATE FROM DUAL@ERP");

            var flotaRecord = new FlotaModel
            {
                FlotaData = dateFlota,
                SafoData = dateSafo
            };

            _context.FlotaModels.Add(flotaRecord);
            await _context.SaveChangesAsync();
        }

        private async Task<DateTime?> GetDateFromOracleAsync(string query)
        {
            using (var connection = new OracleConnection(_oracleConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new OracleCommand(query, connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    return result as DateTime?;
                }
            }
        }
    }
}
