using B2b_Api.Models;
using B2b_Api.Servisler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static B2b_Api.Models.Evrak;

namespace B2b_Api.Controllers
{
    public class TeklifController : ApiController
    {
        [HttpGet]
        [Route("TumTeklifleriOku")]
        public Sonuc GetTumTeklifleriOku()
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            SayfalamaBilgileri sb = new SayfalamaBilgileri();
            sonuc = ti.TeklifFisListesiniAl(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;

        }
        [HttpPost]
        [Route("TeklifleriOku")]
        public Sonuc TeklifleriOku([FromBody] SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.TeklifFisListesiniAl(sb);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("TeklifHareketleriniOku/{teklifNo}")]
        public Sonuc GetTeklifHareketleriniOku([FromUri] string teklifNo)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.TeklifHareketListesiniAl(teklifNo);
            sonuc.servisUlasmaBasari = true;
            return sonuc;

        }
        [HttpGet]
        [Route("SepetOku/{cariKodu}")]
        public Sonuc GetSepetOku([FromUri] string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.SepetOku(cariKodu);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("TeklifEkle")]
        public Sonuc TeklifEkle([FromBody] TeklifEvrakBilgileri evrak)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.TeklifEkle(evrak);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpPost]
        [Route("SepetEkle")]
        public Sonuc SepetEkle([FromBody] TeklifEvrakBilgileri evrak)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.SepetEkle(evrak);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("SepetiSil/{cariKodu}")]
        public Sonuc GetSepetiSil(string cariKodu)
        {

            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.SepetSil(cariKodu);
            sonuc.servisUlasmaBasari = true;
            return sonuc;
        }
        [HttpGet]
        [Route("SepetPDFAl/{teklifId}")]

        public Sonuc GetSepetPDFAl([FromUri] int teklifId)
        {
            Sonuc sonuc = new Sonuc();
            TeklifIslemleri ti = new TeklifIslemleri();
            sonuc = ti.SepetPDFAl(teklifId);
            sonuc.servisUlasmaBasari = true;
            return sonuc;

        }
    }
}