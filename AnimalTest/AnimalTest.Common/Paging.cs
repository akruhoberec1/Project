﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalTest.Common
{
    public class Paging
    {
        public int? PageSize { get; set; } = 5; 
        public int? TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize ?? 3);
        public int TotalCount { get; set; }
        public int? PageNumber { get; set; } = 1;
    }
}
