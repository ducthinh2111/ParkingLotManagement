using System;
using System.Collections.Generic;

namespace DataAccess.DataModels
{
    public partial class AdminAccount
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
