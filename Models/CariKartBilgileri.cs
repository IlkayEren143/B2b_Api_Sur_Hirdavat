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
        public int adresNo { get; set; } = 1;
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
        public string temsilci { get;  set; }
    }
    public class AdresKartBilgileri
    {
        public int siraNo { get; set; }
        public string adres1 { get; set; }
        public string adres2 { get; set; }
        public string adres3 { get; set; }
        public string ilce { get; set; }
        public string il { get; set; }
        public string ulke { get; set; }
       

        public AdresKartBilgileri()
        {
            siraNo = 0;
            adres1 = "";
            adres2 = "";
            adres3 = "";
            ilce = "";
            il = "";
            ulke = "TÜRKİYE";
        }
    }
}