using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class Parking
    {
        public string LicensePlate { get; set; }
        public string SecurityCode { get; set; }
        public DateTime? CheckInDateTime { get; set; }
        public DateTime? CheckOutDateTime { get; set; }
        public string SlotID { get; set; }

        public virtual Slot Slot { get; set; }
    }
}
