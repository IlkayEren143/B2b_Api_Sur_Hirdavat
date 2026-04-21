using B2b_Api.Models;
using B2b_Api.Servisler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace B2b_Api.Controllers
{
    public class CariController : ApiController
    {
        [HttpGet]
        [Route("TumCariKartlariOku")]
        public Sonuc GetTumCariKartlariOku()
        {
            Sonuc sonuc = new Sonuc();
            CariIslemler ci = new CariIslemler();
            SayfalamaBilgileri parametre = new SayfalamaBilgileri();
            sonuc = ci.CariKartlariOku(parametre);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("CariKartlariOku")]
        public Sonuc CariKartlariOku([FromBody] SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            CariIslemler ci = new CariIslemler();
            sonuc = ci.CariKartlariOku(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("CariKartiOku")]
        public Sonuc GetCariKartiOku([FromUri] string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            CariIslemler ci = new CariIslemler();
            SayfalamaBilgileri parametre = new SayfalamaBilgileri();
            parametre.ekSorgu = $"WHERE CARKOD = '{cariKodu}'";
            sonuc = ci.CariKartlariOku(parametre);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("Login/{kod}/{sifre}")]//kod şifre prosedurden düzenleniyor 
        public Sonuc GetLogin(string kod, string sifre)
        {
            Sonuc sonuc = new Sonuc();
            CariIslemler ci = new CariIslemler();
            sonuc = ci.SifreKontrol(kod, sifre);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
    }
}
