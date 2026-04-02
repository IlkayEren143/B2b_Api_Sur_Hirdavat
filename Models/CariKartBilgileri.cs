using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class CariKartBilgileri
    {
        public string cariKodu { get; set; }
        public string cariUnvani { get; set; }
        public decimal bakiye { get; set; }
        public decimal iskonto { get; set; }
    }
}