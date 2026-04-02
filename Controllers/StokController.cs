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
            sonuc.servisUlasmaBasari = true;
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
    }
}
