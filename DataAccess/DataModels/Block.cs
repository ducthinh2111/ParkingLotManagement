using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class Block
    {
        public Block()
        {
            Slot = new HashSet<Slot>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public double? NumberOfSlots { get; set; }
        public string ParkingLotID { get; set; }

        public virtual ParkingLot ParkingLot { get; set; }
        public virtual ICollection<Slot> Slot { get; set; }
    }
}
