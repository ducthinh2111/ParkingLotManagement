using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class Staff
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ParkingLotID { get; set; }
        public double? Salary { get; set; }

        public virtual ParkingLot ParkingLot { get; set; }
    }
}
