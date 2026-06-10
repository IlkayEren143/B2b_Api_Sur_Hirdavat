
using Horizon_Genel_İşlemleri;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace B2b_Api.Servisler
{
    public class LisansSistemi
    {        
        public static int projeNo = 20020;
        static string etkinlestirmeKodu = "";
        static DateTime basTarih = DateTime.Today;
        static DateTime bitTarih = DateTime.Today;
        static int lisansid = 0;
        public static string parametre1 = "";
        public static string parametre2 = "";
        public static string parametre3 = "";
        public string hataMesaji = "";
        SqlConnection baglanti;
        public LisansSistemi()
        {
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            baglanti = new SqlConnection(baglantistr);
        }
        //public int LisanslamaKontrolü(ref bool close)
        //{
        //    close = false;
        //    SifrelemeIslemleri2 si2 = new SifrelemeIslemleri2();
        //    SqlConnection hrz_baglanti = ayarlar.ayarBilgileri.ab_baglantiIslemleri.SQLBaglantiNesnesiniOlustur(ayarlar.ayarBilgileri.ab_horizonVeriTabani);
        //    LisanslamaIslemleri lisanslama = new LisanslamaIslemleri(hrz_baglanti);
        //    LisanslamaBilgileri lisans = new LisanslamaBilgileri();
        //    lisans.etaserino = ayarlar.ayarBilgileri.ab_etaAyarlari.eta_SetNumarasi;
        //    lisans.macadresi = "20-72-73-45-49-20";
        //    lisans.projeno = 20001;

        //    Sonuc sonuc = WebApidenOku(lisans);
        //    if (sonuc == null)
        //        return 1;
        //    if (sonuc.sonuc = false)
        //    {
        //        close = true;
        //        return -1;
        //    }
        //    return Convert.ToInt32(sonuc.data);

        //}
        public B2b_Api.Models.Sonuc LisansiKontrolEt()
        {
            B2b_Api.Models.Sonuc sonuc = new B2b_Api.Models.Sonuc();
            LisansSistemi ls = new LisansSistemi();
            int sure = ls.LisanslamaKontrolü();
            if (sure < 0)
            {
                sonuc.mesaj = ls.hataMesaji;
                sonuc.sonuc = false;
                sonuc.servisUlasmaBasari = true;
                sonuc.veriOkuBasari = true;
                return sonuc;
            }
            sonuc.sonuc = true;
            sonuc.data = sure;
            return sonuc;
        }
        public int LisanslamaKontrolü()
        {

           
            SifrelemeIslemleri2 si2 = new SifrelemeIslemleri2();
            LisanslamaIslemleri lisanslama = new LisanslamaIslemleri(baglanti);
            int kalanSure = 0;
            lisanslama.lb = new LisanslamaBilgileri();
            lisanslama.lb.projeno = projeNo;
            lisanslama.lb.etaserino = ConfigurationManager.AppSettings["Lisans_SetNo"].ToString();
            lisanslama.lb.macadresi = "20-72-73-45-49-20";
          

            Sonuc sonuc = WebApidenLisansOku(lisanslama.lb);
            if (sonuc != null)
            {
                LisanslamaBilgileri lb = JsonConvert.DeserializeObject<LisanslamaBilgileri>(sonuc.data.ToString());
                etkinlestirmeKodu = lb.etkinlestirmekodu;
                basTarih = lb.baslangictarihi;
                bitTarih = lb.bitistarihi;
                lisansid = LisansidAl("WHERE ProjeNo = " + projeNo + " AND ETASeriNo = '" + lisanslama.lb.etaserino +
                    "' AND MacAdresi = '20-72-73-45-49-20'");
                parametre1 = lb.parametre1;
                parametre2 = lb.parametre2;
                parametre3 = lb.parametre3;
                EtkinlestirmeKayitBilgileri ekb = new EtkinlestirmeKayitBilgileri(lisansid, projeNo, lisanslama.lb.etaserino, "20-72-73-45-49-20", basTarih, bitTarih, etkinlestirmeKodu);
                EtkinlestirmeKaydet(ekb);
                kalanSure = si2.LisanslamaKontrolunuYapUyarisiz(lisanslama.lb.etaserino,
               "20-72-73-45-49-20", etkinlestirmeKodu, projeNo, basTarih, bitTarih);
                return kalanSure;
            }
            else
            {
                if (etkinlestirmeKodu.Trim().Equals(""))//Bir kez veri taabanından almak için...
                {

                    string eksorgu = "WHERE ProjeNo = " + projeNo + " AND ETASeriNo = '" + lisanslama.lb.etaserino +
                        "' AND MacAdresi = '20-72-73-45-49-20'";
                    try
                    {
                        DataTable tablo = lisanslama.LisanslamaOku(eksorgu);
                        if (tablo == null)
                        {
                            //XtraMessageBox.Show("Lisans bilgisi okunamadı. Lütfen SQL bağlantınızı ve bağlantı ayarlarınızı kontrol ediniz.", "Hata",
                            //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            hataMesaji = "Lisans bilgisi okunamadı. Lütfen SQL bağlantınızı ve bağlantı ayarlarınızı kontrol ediniz.";
                            //close = true;
                            return -2;
                        }
                        if (tablo.Rows.Count > 0)
                        {
                            etkinlestirmeKodu = tablo.Rows[0]["EtkinlestirmeKodu"].ToString();
                            basTarih = Convert.ToDateTime(tablo.Rows[0]["BaslangicTarihi"]);
                            bitTarih = Convert.ToDateTime(tablo.Rows[0]["BitisTarihi"]);
                            lisansid = Convert.ToInt32(tablo.Rows[0]["LisansID"]);
                            parametre1 = tablo.Rows[0]["Parametre1"].ToString();
                            parametre2 = tablo.Rows[0]["Parametre2"].ToString();
                            parametre3 = tablo.Rows[0]["Parametre3"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //XtraMessageBox.Show("Lisans bilgisi okunamadı: " + ex.Message, "Hata",
                        //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hataMesaji = "Lisans bilgisi okunamadı: " + ex.Message;
                        //close = true;
                        return -2;
                    }
                }
                kalanSure = si2.LisanslamaKontrolunuYapUyarisiz(lisanslama.lb.etaserino,
                    "20-72-73-45-49-20", etkinlestirmeKodu, projeNo, basTarih, bitTarih);
                return kalanSure;
            }

        }
        private int LisansidAl(string eksorgu)
        {
            LisanslamaIslemleri lisanslama = new LisanslamaIslemleri(baglanti);
            DataTable tablo = lisanslama.LisanslamaOku(eksorgu);
            if (tablo == null)
                return 0;
            if (tablo.Rows.Count == 0)
                return 0;
            return Convert.ToInt32(tablo.Rows[0]["LisansID"]);
        }
        private Sonuc WebApidenOku(LisanslamaBilgileri lisans)
        {
           
            string url = ConfigurationManager.AppSettings["urlAdresi"].ToString();
            
            //string url = @"http:\\212.252.132.158/WebLis";


            //url = "http://localhost:57131/";
            if (string.IsNullOrEmpty(url))
            {                
                url = @"http:\\212.252.132.158/WebLis";
            }
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/" + "EtkinlestirmeKontrolEt");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string postData = JsonConvert.SerializeObject(lisans);

                    streamWriter.Write(postData);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Sonuc>(result);
                }
            }
            catch (Exception ex)
            {

                Sonuc sonuc = new Sonuc();
                sonuc.sonuc = false;
                sonuc.data = 1;
                sonuc.mesaj = ex.Message;
                return sonuc;

            }
            return null;
        }
        private Sonuc WebApidenLisansOku(LisanslamaBilgileri lisans)
        {
          
            string url = ConfigurationManager.AppSettings["urlAdresi"].ToString();
            //string url = @"http:\\212.252.132.158/WebLis";


            //url = "http://localhost:57131/";
            if (string.IsNullOrEmpty(url))
            {
                url = @"http:\\212.252.132.158/WebLis";
            }
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + "/" + "EtkinlestirmeAl");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string postData = JsonConvert.SerializeObject(lisans);

                    streamWriter.Write(postData);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return JsonConvert.DeserializeObject<Sonuc>(result);
                }
            }
            catch (Exception ex)
            {
               
                return null;

            }
            return null;
        }
        private bool EtkinlestirmeKaydet(EtkinlestirmeKayitBilgileri ekb)
        {
            int id = ekb.projeID;
            LisanslamaIslemleri lisanslama = new LisanslamaIslemleri(baglanti);
            lisanslama.lb.baslangictarihi = ekb.baslangicTarihi;
            lisanslama.lb.bitistarihi = ekb.bitisTarihi;
            lisanslama.lb.etaserino = ekb.firmaNo;
            lisanslama.lb.etkinlestirmekodu = ekb.etkinlestirmeDegeri;
            lisanslama.lb.macadresi = ekb.macAdresi;
            lisanslama.lb.projeno = ekb.projeNo;
            lisanslama.lb.parametre1 = ekb.parametre1;
            lisanslama.lb.parametre2 = ekb.parametre2;
            lisanslama.lb.parametre3 = ekb.parametre3;

            if (lisansid == 0)
            {
                if (!lisanslama.LisanslamaYaz())
                {
                    hataMesaji = "Lisans bilgisi kaydedilemedi. Lütfen SQL bağlantınızı ve bağlantı ayarlarınızı kontrol ediniz.";
                    return false;
                }
            }
            else
            {
                if (!lisanslama.LisanslamaDuzenle("WHERE LisansID = " + lisansid))
                {
                    hataMesaji = "Lisans bilgisi düzenlenemedi. Lütfen SQL bağlantınızı ve bağlantı ayarlarınızı kontrol ediniz.";
                    return false;
                }
            }
            return true;
        }
    }
}
