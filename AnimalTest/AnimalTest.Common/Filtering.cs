﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalTest.Common
{
    public class Filtering
    {
          public string SearchQuery { get; set; }
          public decimal? MinSalary { get; set; } = 0;
          public decimal? MaxSalary { get; set;  } = decimal.MaxValue;
    }
}

// where searchQuery = Neshto and datetime bla bla and 
// || || || || koliko god uspordebi želio i joinamo što želimo i izi pizi