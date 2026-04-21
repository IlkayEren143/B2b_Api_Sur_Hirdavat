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
        public string adres1 { get; set; }
        public string adres2 { get; set; }
        public string adres3 { get; set; }
        public string il { get; set; }
        public string ilce { get; set; }
        public string ulke { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string vergiDairesi { get; set; }
        public string vergiNumarasi { get; set; }
        public string kimlikNo { get; set; }
        public decimal bakiye { get; set; }
        public decimal iskonto { get; set; }
        public string yetkili { get;  set; }
    }

}