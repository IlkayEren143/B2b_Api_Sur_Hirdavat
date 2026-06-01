using B2b_Api.Models;
using B2b_Api.Servisler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace B2b_Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StokController : ApiController
    {
        [HttpGet]
        [Route("TumStokKartlariniOku")]
        public Sonuc GetTumStokKartlariniOku()
        {
            Sonuc sonuc = new Sonuc();

            StokIslemleri si = new StokIslemleri();
            SayfalamaBilgileri sb = new SayfalamaBilgileri();
            sonuc = si.StokKartlariniOku(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("StokKartlariniOku")]
        public Sonuc StokKartlariniOku([FromBody] SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
           
            StokIslemleri si = new StokIslemleri();
            sonuc = si.StokKartlariniOku(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("StokKartiniOku")]
        public Sonuc GetStokKartlariniOku([FromUri] string stokKodu)
        {
            Sonuc sonuc = new Sonuc();
            sonuc.servisUlasmaBasari = true;
            StokIslemleri si = new StokIslemleri();
            SayfalamaBilgileri sb = new SayfalamaBilgileri();
            sb.ekSorgu = $"WHERE STKKOD = '{stokKodu}'";
            sonuc = si.StokKartlariniOku(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("CariListedenStokKartlariniOku/{cariKodu}")]
        public Sonuc GetCariListedenStokKartlariniOku([FromUri] string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            List<StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
            
            StokIslemleri si = new StokIslemleri();
            SayfalamaBilgileri sb = new SayfalamaBilgileri();
            skbListe = si.StokKartlariniListeOku(sb);
            if (skbListe == null)
            {
                sonuc.mesaj = "Stok kartları okunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            if (skbListe.Count == 0)
            {
                sonuc.mesaj = "Stok kartları bulunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            CariIslemler ci = new CariIslemler();
           sonuc = ci.StokFiyatlariniCariListedenAl(skbListe, cariKodu);

            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("CariListedenStokKartlariniOku")]
        public Sonuc CariListedenStokKartlariniOku([FromBody] SayfalamaBilgileri sb, string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            List<StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
            StokIslemleri si = new StokIslemleri();
            skbListe = si.StokKartlariniListeOku(sb);
            if (skbListe == null)
            {
                sonuc.mesaj = "Stok kartları okunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            if (skbListe.Count == 0)
            {
                sonuc.mesaj = "Stok kartları bulunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            CariIslemler ci = new CariIslemler();
            sonuc = ci.StokFiyatlariniCariListedenAl(skbListe, cariKodu);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("AramadanStokkartlariniOku")]
        public Sonuc AramadanStokkartlariniOku([FromBody] SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            StokIslemleri si = new StokIslemleri();
            sonuc = si.AramaStokKartlariniBul(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("CariListedenAramadanStokkartlariniOku")]
        public Sonuc AramadanStokkartlariniOku([FromBody] SayfalamaBilgileri sb, string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            StokIslemleri si = new StokIslemleri();
            sonuc = si.AramaStokKartlariniBul(sb);
            int sayac = sonuc.ekData != null ? Convert.ToInt32(sonuc.ekData) : 0;
            List <StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
            skbListe = sonuc.data as List<StokKartBilgileri>;
            if (skbListe == null)
            {
                sonuc.mesaj = "Stok kartları okunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            if (skbListe.Count == 0)
            {
                sonuc.mesaj = "Stok kartları bulunamadı.";
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.servisUlasmaBasari = true;
                return sonuc;
            }
            CariIslemler ci = new CariIslemler();
            sonuc = ci.StokFiyatlariniCariListedenAl(skbListe, cariKodu);
            sonuc.ekData = sayac;
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
    }
}
