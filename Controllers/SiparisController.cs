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
    public class SiparisController : ApiController
    {
        [HttpGet]
        [Route("SiparisKaydet/{teklifid}")]
        public Sonuc GetSiparisKaydet(int teklifid)
        {
            Sonuc sonuc = new Sonuc();
            SiparisIslemleri si = new SiparisIslemleri();
            sonuc = si.SiparisKaydet(teklifid);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("BekleyenSiparislerPDFAl/{cariKodu}")]
        public Sonuc GetBekleyenSiparislerPDFAl(string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            SiparisIslemleri si = new SiparisIslemleri();
            sonuc = si.BekleyenSiparislerPDFAl(cariKodu);
            return sonuc;
        }
    }
}
