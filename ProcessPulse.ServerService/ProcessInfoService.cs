﻿using Newtonsoft.Json;
using ProcessPulse.Class.ProcessPulse.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

public class ProcessInfoService
{
    string hostName = Environment.MachineName; // Pobranie nazwy hosta
    private readonly AppDbContext _dbContext;

    public ProcessInfoService(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<string> GetProcessResourceDataByNameAsync(string processName)
    {
        try
        {
            if (string.IsNullOrEmpty(processName))
            {
                return JsonConvert.SerializeObject(new { Error = "Nazwa procesu nie może być pusta." });
            }

            DateTime currentTime = DateTime.Now;
            var processInfos = await GetProcessInfosAsync(processName, currentTime);

            if (processInfos == null || processInfos.Count == 0)
            {
                return JsonConvert.SerializeObject(new List<ProcessInfo>());
            }

            foreach (var processInfo in processInfos)
            {
                await SaveProcessInfoToDatabaseAsync(processInfo);
            }

            return JsonConvert.SerializeObject(processInfos);
        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject(new { Error = $"Błąd podczas pobierania danych: {ex.Message}" });
        }
    }

    public async Task SaveProcessInfoToDatabaseAsync(ProcessInfo processInfo)
    {
        if (processInfo == null)
        {
            throw new ArgumentNullException(nameof(processInfo));
        }

        try
        {
            _dbContext.ProcessInfos.Add(processInfo);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Logowanie wyjątku
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public async Task<string> GetProcessNameByHostNameAsync(string hostName)
    {
        // Znajdź mapowanie terminala za pomocą nazwy hosta
        var terminalMapping = await _dbContext.TerminalMapping
                                              .FirstOrDefaultAsync(t => t.TerminalId == hostName);

        // Jeśli mapowanie istnieje, zwróć nazwę procesu
        return terminalMapping?.Name;
    }



    private async Task<List<ProcessInfo>> GetProcessInfosAsync(string processName, DateTime currentTime)
    {
        return await Task.Run(() =>
        {
            List<ProcessInfo> processInfos = new List<ProcessInfo>();

            foreach (var process in Process.GetProcessesByName(processName))
            {
                var processInfo = new ProcessInfo
                {
                    Name = process.ProcessName,
                    Id_Process = process.Id,
                    CpuUsage = GetCpuUsageForProcess(process),
                    RamUsage = process.WorkingSet64 / 1024, // Convert to KB
                    Path = process.MainModule?.FileName,
                    NetworkUsage = GetNetworkUsageForProcess(process.Id),
                    Time = currentTime,
                    TerminalId = hostName
                };

                processInfos.Add(processInfo);
            }

            return processInfos;
        });
    }

    private float GetCpuUsageForProcess(Process process)
    {
        var counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName, readOnly: true);
        counter.NextValue(); // Call once to initialize counter
        System.Threading.Thread.Sleep(1000); // Wait a second to get a valid reading
        return counter.NextValue() / Environment.ProcessorCount;
    }

    private float GetNetworkUsageForProcess(int processId)
    {
        return 0;
    }

    private string GetProcessInstanceNameFromId(int processId)
    {
        var process = Process.GetProcessById(processId);
        string processName = process.ProcessName;
        var cat = new PerformanceCounterCategory("Process");
        var instances = cat.GetInstanceNames();

        foreach (var instance in instances)
        {
            using (var perfCounter = new PerformanceCounter("Process", "ID Process", instance, readOnly: true))
            {
                if ((int)perfCounter.RawValue == processId)
                {
                    return instance;
                }
            }
        }
        return null;
    }
}
