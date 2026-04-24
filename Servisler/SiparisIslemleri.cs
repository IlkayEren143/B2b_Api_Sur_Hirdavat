using B2b_Api.Models;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;

namespace B2b_Api.Servisler
{
    public class SiparisIslemleri
    {
        public Sonuc SiparisKaydet(int teklifid)
        {
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            Sonuc sonuc = new Sonuc();
            TeklifFisIslemleri tfi = new TeklifFisIslemleri(baglanti);
            TeklifHareketIslemleri thi = new TeklifHareketIslemleri(baglanti);
            StokIslemleri si = new StokIslemleri();
           
            DataTable tabloFis = tfi.TeklifFisOku($"WHERE id  = {teklifid}");
            if (tabloFis == null)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Fiş tablosu okunamadı. " + tfi.hataMesaji;
                return sonuc;
            }
            if (tabloFis.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "İlgili id'ye ait sepet bulunamadı.";
                return sonuc;
            }
            if (Convert.ToInt32(tabloFis.Rows[0]["ETAkayitDurum"]) > 0)
            {
                tfi.tfb.fistipi = 1;
                tfi.TeklifFisFisTipiGuncelle($"WHERE id  = {teklifid}");
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "İlgili id'ye ait sepet daha önce ETA'ya kaydedilmiş. Lütfen kontrol ediniz.";
                return sonuc;
            }
            decimal kurFis = si.DovizKurunuOku(tabloFis.Rows[0]["DovizKodu"].ToString(), tabloFis.Rows[0]["DovizTuru"].ToString(), DateTime.Today);
            
            if (kurFis < 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Kur tablosu okunamadı. ";
                return sonuc;
            }
            if (kurFis == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "İlgili kur bulunamadı.";
                return sonuc;
            }
            tabloFis.Rows[0]["Kur"] = kurFis;
            tabloFis.Columns.Add("DovizTutar", typeof(decimal));
            tabloFis.Rows[0]["DovizTutar"] = 0;
            DataTable tabloHareket = thi.TeklifHareketOku($"Where Fisid = {teklifid}");
            if (tabloHareket == null)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Hareket tablosu okunamadı. " + thi.hataMesaji;
                return sonuc;
            }
            if (tabloHareket.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "İlgili id'ye ait sepetin hareketleri okunamadı.";
                return sonuc;
            }
            foreach (DataRow satir in tabloHareket.Rows)
            {
                decimal kurHareket = si.DovizKurunuOku(satir["DovizKodu"].ToString(), satir["DovizTuru"].ToString(), DateTime.Today);
                if (kurHareket < 0)
                {
                    sonuc.sonuc = false;
                    sonuc.veriOkuBasari = false;
                    sonuc.data = null;
                    sonuc.ekData = null;
                    sonuc.mesaj = "Kur tablosu okunamadı. ";
                    return sonuc;
                }
                if (kurHareket == 0)
                {
                    sonuc.sonuc = false;
                    sonuc.veriOkuBasari = true;
                    sonuc.data = null;
                    sonuc.ekData = null;
                    sonuc.mesaj = "İlgili kur bulunamadı.";
                    return sonuc;
                }
                satir["Kur"] = kurHareket;
                decimal tutarTL = kurHareket * Convert.ToDecimal(satir["Fiyat"]);
                tutarTL = tutarTL * Convert.ToDecimal(satir["Miktar"]);
                decimal tutarDoviz = tutarTL / kurFis;
                tabloFis.Rows[0]["DovizTutar"] = tutarDoviz + Convert.ToDecimal(tabloFis.Rows[0]["DovizTutar"]);
            }
            string hataMesaji = "";
            int refno = ETAKaydet(tabloFis.Rows[0], tabloHareket, ref hataMesaji);
            if (refno <= 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.mesaj = "Evrak Kaydedilemedi. " + hataMesaji;
                sonuc.data = refno;
                sonuc.ekData = null;
            }
            else
            {
                sonuc.sonuc = true;
                sonuc.veriOkuBasari = true;
                sonuc.mesaj = "Başarılı";
                sonuc.data = refno;
                sonuc.ekData = null;
            }
            return sonuc;
        }
        public Sonuc BekleyenSiparislerPDFAl(string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            DataSet ds = new DataSet();
            DataTable tablo = BekleyenSiparisleriOku(cariKodu);
            if (tablo == null)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Sipariş hareket tablosu okunamadı.";
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Cariye ait Sipariş hareket bulunamadı.";
                return sonuc;
            }
            tablo.TableName = "BekleyenSiparisler";
            ds.Tables.Add(tablo);
            string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Dizayn");
            string dizynDosyasi = mappedPath + "\\" + "BekleyenSiparislerDizayn" + ".repx";
            XtraReport rapor = new XtraReport();
            rapor.LoadLayoutFromXml(dizynDosyasi);
            rapor.DataSource = ds;
            if (File.Exists(Path.GetDirectoryName(dizynDosyasi) + $"//BekleyenSiparis_{cariKodu}.pdf"))
                File.Delete(Path.GetDirectoryName(dizynDosyasi) + $"//BekleyenSiparis_{cariKodu}.pdf");
            rapor.ExportToPdf(Path.GetDirectoryName(dizynDosyasi) + $"//BekleyenSiparis_{cariKodu}.pdf");
            FileStream file = new FileStream(Path.GetDirectoryName(dizynDosyasi) + $"//BekleyenSiparis_{cariKodu}.pdf", FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, (int)file.Length);
            file.Close();
            string base64String = Convert.ToBase64String(bytes);
            if (bytes == null)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = "Bekleyen sipariş PDF'i okunamadı";
                return sonuc;
            }
            if (bytes.Length == 0)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = "Cariye ait bekleyen sipariş PDF'i bulunamadı";
                return sonuc;
            }
            sonuc.sonuc = true;
            sonuc.data = base64String;
            sonuc.ekData = Path.GetDirectoryName(dizynDosyasi) + $"//BekleyenSiparis_{cariKodu}.pdf";
            sonuc.mesaj = "Başarılı.";
            return sonuc;
        }

        private DataTable BekleyenSiparisleriOku(string cariKodu)
        {

            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string etaMaster = ConfigurationManager.AppSettings["MasterDBName"].ToString();

            string eksorgu = $@" WHERE SIPHARTIPI IN (SELECT SIPGENFTNO FROM {etaMaster}..SIPGENFISTIP WHERE SIPGENFTTIP = 2 AND SIPGENFTACKAPA = 1) AND SIPHARTESFLAG = 0 AND SIPHARCARKOD = '{cariKodu}'";
            SqlCommand komut = new SqlCommand();
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.CommandText = "SiparisHareketOku";
            komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
            komut.Parameters.AddWithValue("@eksorgu", eksorgu);

            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Komut_Adaptor(komut);
            return tablo;
        }
        private int ETAKaydet(DataRow fis, DataTable hareket, ref string hata)
        {
            hata = "Başarılı";
           
            ETA_Kayit_Islemleri.EvrakBaglantiParametreleri ebp = EvrakParametreleriniOlustur();
            ETA_Kayit_Islemleri.ETAKayitIslemleri eki = new ETA_Kayit_Islemleri.ETAKayitIslemleri(ebp);
            eki.ebb = new ETA_Kayit_Islemleri.EvrakBaslikBilgileri();
            eki.ebb.adresNo = 1;
            eki.ebb.aciklama1 = fis["Aciklama1"].ToString();
            eki.ebb.aciklama2 = fis["Aciklama2"].ToString();
            eki.ebb.aciklama3 = fis["Aciklama3"].ToString();
            eki.ebb.ozkod1 = fis["OzelKod1"].ToString();
            eki.ebb.ozkod2 = fis["OzelKod2"].ToString();
            eki.ebb.ozkod3 = fis["OzelKod3"].ToString();
            eki.ebb.cariAdi = fis["CariUnvani"].ToString();
            eki.ebb.cariKodu = fis["CariKodu"].ToString();
            eki.ebb.depoKodu = System.Configuration.ConfigurationManager.AppSettings["depoKodu"].ToString();
            eki.ebb.tarih = DateTime.Today;
            eki.ebb.faturaAdres1 = fis["Adres1"].ToString();
            eki.ebb.faturaAdres2 = fis["Adres2"].ToString();
            eki.ebb.faturaAdres3 = fis["Adres3"].ToString();
            eki.ebb.faturaIl = fis["Il"].ToString();
            eki.ebb.faturaIlce = fis["Ilce"].ToString();
            eki.ebb.faturaTelefon = fis["TelefonNo"].ToString();
            eki.ebb.faturaUlke = fis["Ulke"].ToString();
            eki.ebb.email = fis["EMail"].ToString();
            eki.ebb.faturaYetkili = fis["Yetkili"].ToString();
            eki.ebb.fisTipi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["fisTipi"]);
            eki.ebb.hazirlayan = System.Configuration.ConfigurationManager.AppSettings["hazirlayan"].ToString();
            eki.ebb.kdvDahilFlag = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["kdvDahilFlag"]);
            eki.ebb.kdvOrani = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["kdvOrani"]);
            eki.ebb.genelIndirim1Yuzde = Convert.ToDecimal(fis["IskontoYuzde1"]);
            eki.ebb.genelIndirim2Yuzde = Convert.ToDecimal(fis["IskontoYuzde1"]);
            eki.ebb.dovizKodu = fis["CariKodu"].ToString();
            eki.ebb.vergiDairesi = fis["VergiDairesi"].ToString();
            eki.ebb.vergiNumarasi = fis["VergiNumarasi"].ToString();
            eki.ebb.tcKimlikNo = fis["KimlikNo"].ToString();
            eki.ebb.dovizKodu = fis["DovizKodu"].ToString();
            eki.ebb.dovizTuru = fis["DovizTuru"].ToString();
            eki.ebb.dovizKuru = Convert.ToDecimal(fis["Kur"]);
            eki.ebb.dovizTutar = Convert.ToDecimal(fis["DovizTutar"]);
            eki.esbListe = new List<ETA_Kayit_Islemleri.EvrakSatirBilgileri>();
            int sayac = 0;
            decimal kdvMatrah = 0;
            foreach (DataRow satir in hareket.Rows)
            {
                //decimal miktar = StokBakiyeyiAl(sb.stokKodu, eki.ebb.depoKodu) - sb.miktar;
                //if (miktar < 0)
                //{
                //    //ilb.durum = $"{sb.stokKodu} Stok kodlu ürünün bakiyesi yeterli değildir.";
                //    //SiparisLogKaydet(ilb);
                //    hata = "Ürünün bakiyesi yeterli değildir.";
                //    return 0;
                //}

                /*
                string komutstr = @"SELECT id, Fisid, Tarih, VadeTarihi, TerminTarihi, , , , , , , , IndirimToplam, , KDVTutar, Tutar, Kur, TeklifNo, , , , , , , , , DepoKodu, ,  From TeklifHareket";*/
                ETA_Kayit_Islemleri.EvrakSatirBilgileri esb = new ETA_Kayit_Islemleri.EvrakSatirBilgileri();
                esb.aciklama = satir["Aciklama"].ToString();
                esb.aciklama1 = satir["Aciklama1"].ToString();
                esb.aciklama2 = satir["Aciklama2"].ToString();
                esb.aciklama3 = satir["Aciklama3"].ToString();
                esb.ozkod = satir["OzelKod"].ToString();
                esb.birim = satir["Birim"].ToString();
                esb.depoKod = eki.ebb.depoKodu;
                esb.dovizFiyat = Convert.ToDecimal(satir["Fiyat"]);
                esb.kalemIndirim1 = Convert.ToDecimal(satir["IndirimYuzde1"]);
                esb.kalemIndirim2 = Convert.ToDecimal(satir["IndirimYuzde2"]);
                esb.kalemIndirim3 = Convert.ToDecimal(satir["IndirimYuzde3"]);
                esb.kalemIndirim4 = Convert.ToDecimal(satir["IndirimYuzde4"]);
                esb.kalemIndirim5 = Convert.ToDecimal(satir["IndirimYuzde5"]);
                esb.kdvOrani = Convert.ToDecimal(satir["KDVYuzde"]);
                esb.kodTipi = 1;
                esb.miktar = Convert.ToDecimal(satir["Miktar"]);
                esb.satirTarih = DateTime.Today;
                esb.siraNo = sayac;
                esb.takipNo = "";
                sayac++;
                esb.stokCinsi = satir["StokCinsi"].ToString();
                esb.stokKodu = satir["StokKodu"].ToString();
                esb.dovizTutar = esb.dovizFiyat * esb.miktar;
                esb.dovKod  = satir["DovizKodu"].ToString();
                esb.dovTur = satir["DovizTuru"].ToString();
                esb.dovKur = Convert.ToDecimal(satir["Kur"]);
                esb.fiyat = esb.dovizFiyat * esb.dovKur;
                esb.tutar = esb.fiyat * esb.miktar;
  //              esb.fiyatNo = Convert.ToInt32(satir["FiyatNo"]);
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["tutarHesapla"]) == 0)
                {
                    esb.tutar = -1;
                    esb.dovizTutar = -1;
                }
                esb.vadeTarih = Convert.ToDateTime(satir["VadeTarihi"]);
                eki.esbListe.Add(esb);
            }
            //eki.ebb.kdvMatrah = kdvMatrah;
            int refno = eki.SiparisKaydet(ref hata);
            if (refno <= 0)
            {
                //ilb.durum = "Sipariş kaydedilemedi. " + hata;
                //SiparisLogKaydet(ilb);
                hata = "Sipariş kaydedilemedi. " + hata;
            }
            else
            {
                //ilb.basariflag = refno;
                //ilb.durum = $"Sipariş Fiş evrağı başarı ile kaydedildi. ( Referans No:{refno} )";
                //SiparisLogKaydet(ilb);
            }
            return refno;
        }

        private ETA_Kayit_Islemleri.EvrakBaglantiParametreleri EvrakParametreleriniOlustur()
        {
            try
            {
                ETA_Kayit_Islemleri.EvrakBaglantiParametreleri ebp = new ETA_Kayit_Islemleri.EvrakBaglantiParametreleri();
                ebp.ServerName = ConfigurationManager.AppSettings["ServerName"].ToString();
                ebp.ServerUserName = ConfigurationManager.AppSettings["ServerUserName"].ToString();
                ebp.ServerUserPsw = ConfigurationManager.AppSettings["ServerUserPsw"].ToString();
                ebp.MasterDBName = ConfigurationManager.AppSettings["MasterDBName"].ToString();
                ebp.KullaniciKodu = ConfigurationManager.AppSettings["KullaniciKodu"].ToString();
                ebp.KullaniciSifre = ConfigurationManager.AppSettings["KullaniciSifre"].ToString();
                ebp.SirketKodu = ConfigurationManager.AppSettings["SirketKodu"].ToString();
                ebp.SirketDonem = Convert.ToInt32(ConfigurationManager.AppSettings["SirketDonem"]);
                ebp.IsyeriKodu = ConfigurationManager.AppSettings["IsyeriKodu"].ToString();
                ebp.programTipi = ConfigurationManager.AppSettings["programTipi"].ToString();
                ebp.Lisans_Firma = ConfigurationManager.AppSettings["Lisans_Firma"].ToString();
                ebp.Lisans_SetNo = ConfigurationManager.AppSettings["Lisans_SetNo"].ToString();
                ebp.Lisans_UrunNo = ConfigurationManager.AppSettings["Lisans_UrunNo"].ToString();

                return ebp;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return null;
            }
        }
    }
}