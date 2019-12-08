using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class MonthlyIncome
    {
        public double ID { get; set; }
        public string Month { get; set; }
        public double? Income { get; set; }
    }
}
