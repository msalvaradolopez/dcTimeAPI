using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class tablaSurtidoresExt
    {
        public string EMPID { get; set; }
        public string LASTNAME { get; set; }
        public string FISRTNAME { get; set; }
        public string MIDDLENAME { get; set; }
        public string ESTATUS { get; set; }

        public tablaSurtidoresExt() { }
        public tablaSurtidoresExt(string empid, string lastname, string firstname, string middlename, string estatus)
        {
            EMPID= empid;
            LASTNAME = lastname;
            FISRTNAME = firstname;
            MIDDLENAME = middlename;
            ESTATUS = estatus;

        }

    }
}