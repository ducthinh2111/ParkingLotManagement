using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class Slot
    {
        public Slot()
        {
            Parking = new HashSet<Parking>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string BlockID { get; set; }
        public string Status { get; set; }
        public string Availability { get; set; }

        public virtual Block Block { get; set; }
        public virtual ICollection<Parking> Parking { get; set; }
    }
}
