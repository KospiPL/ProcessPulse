using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.Class.ProcessPulse.Models
{
    public class SafoModel
    {
        public int Id { get; set; }
        public bool IsConnectedSafo { get; set; }
        public bool IsConnectedNav { get; set; }
        public DateTime? Data { get; set; }
    }
}
