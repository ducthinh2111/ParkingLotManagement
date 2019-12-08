using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class ParkingLot
    {
        public ParkingLot()
        {
            Block = new HashSet<Block>();
            Customer = new HashSet<Customer>();
            Staff = new HashSet<Staff>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? DayPrice { get; set; }
        public double? NightPrice { get; set; }
        public double? VIPDiscount { get; set; }
        public double? VIPRequiredTime { get; set; }

        public virtual ICollection<Block> Block { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
    }
}
