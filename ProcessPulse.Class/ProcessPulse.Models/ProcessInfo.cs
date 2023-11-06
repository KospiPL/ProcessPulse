using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProcessPulse.Class.ProcessPulse.Models
{
    public class ProcessInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Id_Process { get; set; }
        public string? Name { get; set; }
        public double CpuUsage { get; set; }
        public double RamUsage { get; set; }
        public string? Path { get; set; }
        public double NetworkUsage { get; set; }
        public string? TerminalId { get; set; }
        public DateTime Time { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public ProcessInfo? ParentProcess { get; set; }
        public List<ProcessInfo> ChildProcesses { get; set; } = new List<ProcessInfo>();
    }
}
