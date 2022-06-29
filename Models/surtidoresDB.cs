using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class surtidoresDB
    {
        public string empID { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string Foto { get; set; }

        public surtidoresDB() { }
        public surtidoresDB(string empid, string lastname, string firstname, string middlename, string foto) {
            empID = empid;
            lastName = lastname;
            firstName = firstname;
            middleName = middlename;
            Foto = foto;

        }

    }
}