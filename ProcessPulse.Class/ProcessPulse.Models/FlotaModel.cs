﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.Class.ProcessPulse.Models
{
    public class FlotaModel
    {
        public int Id { get; set; }
        public DateTime?  FlotaData { get; set; }
        public DateTime? SafoData { get; set; }
    }

}