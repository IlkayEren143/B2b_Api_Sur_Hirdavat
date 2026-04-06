using B2b_Api.Models;
using B2b_Api.Servisler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace B2b_Api.Controllers
{
    public class SliderController : ApiController
    {
        [Route("SliderOku")]
        public Sonuc GetSliderOku()
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            SliderIslemleri si = new SliderIslemleri(baglanti);
            DataTable tablo = si.SliderOku();
            sonuc.servisUlasmaBasari = true;

            if (tablo == null)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = si.hataMesaji;
                sonuc.veriOkuBasari = false;
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = "Slider bulunamadı.";
                sonuc.veriOkuBasari = true;
                return sonuc;
            }
            if (tablo.Rows.Count > 0)
            {
                List<SliderBilgileri> sliderListesi = new List<SliderBilgileri>();
                for (int i = 0; i < tablo.Rows.Count; ++i)
                {
                    SliderBilgileri sb = new SliderBilgileri();
                    sb.id = Convert.ToInt32(tablo.Rows[i]["Id"]);
                    sb.aktif = Convert.ToInt32(tablo.Rows[i]["Aktif"]);
                    sb.sirano = Convert.ToInt32(tablo.Rows[i]["SiraNo"]);
                    sb.linkaktif = Convert.ToInt32(tablo.Rows[i]["LinkAktif"]);
                    sb.link = tablo.Rows[i]["Link"].ToString();
                    sliderListesi.Add(sb);
                }
                sonuc.sonuc = true;
                sonuc.data = sliderListesi;
                sonuc.mesaj = "Başarılı";
                sonuc.veriOkuBasari = true;
            }
            return sonuc;
        }
        [HttpPost]
        [Route("SliderEkle")]
        public Sonuc SliderEkle([FromBody] SliderBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            SliderIslemleri si = new SliderIslemleri(baglanti);
            si.sb = sb;
            bool yazSonuc = si.SliderYaz();
            if (yazSonuc)
            {
                sonuc.sonuc = true;
                sonuc.mesaj = "Slider başarıyla eklendi.";
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = true;
            }
            else
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Slider eklenirken hata oluştu: " + si.hataMesaji;
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = false;
            }
            return sonuc;
        }
        [HttpPost]
        [Route("SliderDuzenle")]
        public Sonuc SliderDuzenle([FromBody] SliderBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            SliderIslemleri si = new SliderIslemleri(baglanti);
            si.sb = sb;
            bool duzenleSonuc = si.SliderDuzenle();
            if (duzenleSonuc)
            {
                sonuc.sonuc = true;
                sonuc.mesaj = "Slider başarıyla düzenlendi.";
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = true;
            }
            else
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Slider düzenlenirken hata oluştu: " + si.hataMesaji;
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = false;
            }
            return sonuc;
        }
        [Route("SliderSil/{id}")]
        public Sonuc GetsliderSil(int id)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            SliderIslemleri si = new SliderIslemleri(baglanti);
            bool silSonuc = si.SliderSil($"WHERE Id = '{id}'");
            if (silSonuc)
            {
                sonuc.sonuc = true;
                sonuc.mesaj = "Slider başarıyla silindi.";
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = true;
            }
            else
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Slider silinirken hata oluştu: " + si.hataMesaji;
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = false;
            }
            return sonuc;
        }
    }
}
