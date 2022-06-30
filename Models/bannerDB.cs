using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dcTimeAPI.Models
{
    public class bannerDB
    {
        public string bannerID { get; set; }
        public string texto { get; set; }

        public bannerDB() { }

        public bannerDB(string pBannerID, string pTexto) {
            bannerID = pBannerID;
            texto = pTexto;
        }
    }
}