using System;
using System.Collections.Generic;

namespace DataPrimer.Storage
{
    public partial class Persons
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Flsah { get; set; }
        public decimal AverageIncome { get; set; }
    }
}
