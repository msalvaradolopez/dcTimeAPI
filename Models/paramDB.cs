using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class paramDB
    {
        public string video { get; set; }

        public paramDB() { }

        public paramDB(string pVideo) {
            video = pVideo;

    }
    }
}