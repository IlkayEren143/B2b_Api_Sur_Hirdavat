using B2b_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace B2b_Api.Servisler
{
    public class StokIslemleri
    {
        public Sonuc StokKartlariniOku(SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            sonuc = StokKartlariniAl(sb);
            int sayac = StokSayisiniBul(sb.ekSorgu);
            sonuc.ekData = sayac;
            return sonuc;
        }
        public decimal DovizKurunuOku(string dovizKodu, string dovizTuru, DateTime tarih)
        {
            string eksorgu = $"WHERE DOVHARKOD = '{dovizKodu}' AND DOVHARTUR = '{dovizTuru}' AND DOVHARTAR = '{tarih.ToString("yyyyMMdd")}'";
            DataTable tablo = DovizKuruAl(eksorgu);
            if (tablo == null)
                return -1;
            if (tablo.Rows.Count == 0)
                return 0;
            return Convert.ToDecimal(tablo.Rows[0][0]);
        }
        public List<StokKartBilgileri> StokKartlariniListeOku(SayfalamaBilgileri sb)
        {
            List<StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
            
            skbListe = (List<StokKartBilgileri>)StokKartlariniAl(sb).data;
            return skbListe;
        }
        public DataTable StokGrupListesiniAl(string grupKodu, int fiyatNo)
        {
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $" SELECT STKFIYSTKKOD as [Stok Kodu], STKFIYNO as [Fiyat No], STKFIYTUTAR as Fiyat, STKFIYISKYUZ1 as [İskonto Yüzde 1], STKFIYISKYUZ2 as [İskonto Yüzde 2], STKFIYISKYUZ3 as [İskonto Yüzde 3], STKFIYISKYUZ4 as [İskonto Yüzde 4], STKFIYISKYUZ5 as [İskonto Yüzde 5], STKFIYDOVKOD as [Döviz Kodu], STKFIYDOVTUR as [Döviz Türü] FROM {etaVeriTabani}..STKFIYAT LEFT JOIN (SELECT STKKOD, STKGRUPKOD FROM {etaVeriTabani}..STKKART) SK ON STKKOD = STKFIYSTKKOD WHERE STKFIYNO = {fiyatNo} AND STKGRUPKOD = '{grupKodu}'";
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Komut_Adaptor(new SqlCommand(komutstr));
            return tablo;
        }
        public DataTable ListeStokKartListesiniAl(string stokKodu, int fiyatNo)
        {
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $" SELECT STKFIYSTKKOD as [Stok Kodu], STKFIYNO as [Fiyat No], STKFIYTUTAR as Fiyat, STKFIYISKYUZ1 as [İskonto Yüzde 1], STKFIYISKYUZ2 as [İskonto Yüzde 2], STKFIYISKYUZ3 as [İskonto Yüzde 3], STKFIYISKYUZ4 as [İskonto Yüzde 4], STKFIYISKYUZ5 as [İskonto Yüzde 5], STKFIYDOVKOD as [Döviz Kodu], STKFIYDOVTUR as [Döviz Türü] FROM {etaVeriTabani}..STKFIYAT LEFT JOIN (SELECT STKKOD, STKGRUPKOD FROM {etaVeriTabani}..STKKART) SK ON STKKOD = STKFIYSTKKOD WHERE STKFIYNO = {fiyatNo} AND SK.STKKOD = '{stokKodu}'";
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Komut_Adaptor(new SqlCommand(komutstr));
            return tablo;
        }
        private Sonuc StokKartlariniAl(SayfalamaBilgileri sb, bool kriterCalissin = true)
        {
            Sonuc sonuc = new Sonuc();
            //Sonuc ayarSonuc = AyarBilgileriniAl();
            //AyarBilgileri ab = (AyarBilgileri)ayarSonuc.data;
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            int kdvFlag = Convert.ToInt32(ConfigurationManager.AppSettings["kdvDahilFlag"]);
            string depoKodu = ConfigurationManager.AppSettings["depoKodu"].ToString();
            string eksorgu = "";
            if (kriterCalissin)
                eksorgu = StokKartKriterleriniOlustur(sb);
            else
                eksorgu = sb.ekSorgu;
            SqlCommand komut = new SqlCommand();
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.CommandText = "StokKartlariniOku";
            komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
            komut.Parameters.AddWithValue("@depoKodu", depoKodu);
            komut.Parameters.AddWithValue("@eksorgu", eksorgu);
            komut.Parameters.AddWithValue("@fiyatNo", Convert.ToInt32(ConfigurationManager.AppSettings["fiyatNo"]));
            komut.Parameters.AddWithValue("@etaMasterAdi", ConfigurationManager.AppSettings["etaMaster"].ToString());

            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Komut_Adaptor(komut);
            if (tablo == null)
            {
                sonuc.data = null;
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.mesaj = "Stok Kart tablosu okunamadı." + asi.hataMesaji;
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.data = null;
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.mesaj = "Kriterleri uyan Stok Kartları bulunamadı." + asi.hataMesaji;
                return sonuc;
            }
            List<StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
            for (int i = 0; i < tablo.Rows.Count; ++i)
            {

                StokKartBilgileri skb = new StokKartBilgileri();

                skb.aciklama1 = tablo.Rows[i]["STKACIK1"].ToString();
                skb.aciklama2 = tablo.Rows[i]["STKACIK2"].ToString();
                skb.aciklama3 = tablo.Rows[i]["STKACIK3"].ToString();
                skb.aciklama4 = tablo.Rows[i]["STKACIK4"].ToString();
                skb.aciklama5 = tablo.Rows[i]["STKACIK5"].ToString();
                skb.bakiye = Convert.ToDecimal(tablo.Rows[i]["STKBAKIYE"]);
                skb.birim = tablo.Rows[i]["STKBIRIM"].ToString();
                skb.fiyat = Convert.ToDecimal(tablo.Rows[i]["STKFIYAT"]);
                skb.dovizKodu = tablo.Rows[i]["STKDOVKOD"].ToString();
                skb.dovizTuru = tablo.Rows[i]["STKDOVTUR"].ToString();
                skb.kalemIndirim1 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ1"]);
                skb.kalemIndirim2 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ2"]);
                skb.kalemIndirim3 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ3"]);
                skb.kalemIndirim4 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ4"]);
                skb.kalemIndirim5 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ5"]);
                decimal kdvsizFiyat = skb.fiyat;
                if (kdvFlag == 1)
                {
                    kdvsizFiyat = skb.fiyat * 100 / 120;
                }
                skb.dovizKodu = tablo.Rows[i]["STKDOVKOD"].ToString();
                skb.dovizTuru = tablo.Rows[i]["STKDOVTUR"].ToString();
                skb.fiyat = Convert.ToDecimal(tablo.Rows[i]["STKFIYAT"]);
                decimal netFiyat = skb.fiyat;

                netFiyat *= (1 - skb.kalemIndirim1 / 100);
                netFiyat *= (1 - skb.kalemIndirim2 / 100);
                netFiyat *= (1 - skb.kalemIndirim3 / 100);
                netFiyat *= (1 - skb.kalemIndirim4 / 100);
                netFiyat *= (1 - skb.kalemIndirim5 / 100);
                skb.netFiyat = netFiyat;
                skb.kdvOrani = Convert.ToDecimal(tablo.Rows[i]["STKKDVORAN"]);
                skb.ozelKod1 = tablo.Rows[i]["STKOZKOD1"].ToString();
                skb.ozelKod2 = tablo.Rows[i]["STKOZKOD2"].ToString();
                skb.ozelKod3 = tablo.Rows[i]["STKOZKOD3"].ToString();
                skb.ozelKod4 = tablo.Rows[i]["STKOZKOD4"].ToString();
                skb.ozelKod5 = tablo.Rows[i]["STKOZKOD5"].ToString();
                skb.resimBase64 = tablo.Rows[i]["STKRESIMPATH"].ToString();
                skb.stokCinsi = tablo.Rows[i]["STKCINSI"].ToString();
                skb.stokCinsi2 = tablo.Rows[i]["STKCINSI2"].ToString();
                skb.stokCinsi3 = tablo.Rows[i]["STKCINSI3"].ToString();
                skb.stokKodu = tablo.Rows[i]["STKKOD"].ToString();
                skb.barkod = tablo.Rows[i]["STKBARKOD"].ToString();
                skb.grupKodu = tablo.Rows[i]["STKGRUPKOD"].ToString();
                skb.kur = 50;
                skb.resimBase64 = "";
                string dataTipi = ResminUzantisiniAl(tablo.Rows[i]["STKRESIMPATH"].ToString());
                string dosya = tablo.Rows[i]["STKRESIMPATH"].ToString().Trim();
                if (File.Exists(dosya))
                {
                    byte[] bytes = File.ReadAllBytes(tablo.Rows[i]["STKRESIMPATH"].ToString());
                    skb.resimBase64 = Convert.ToBase64String(bytes);
                    skb.resimBase64 = $"data:{dataTipi};base64," + skb.resimBase64;
                }
                skbListe.Add(skb);
            }
            sonuc.sonuc = true;
            sonuc.data = skbListe;
            sonuc.veriOkuBasari = true;
            sonuc.mesaj = "Başarılı";
            return sonuc;
        }
        //private List<StokKartBilgileri StokKartlariniListeAl(SayfalamaBilgileri sb, bool kriterCalissin = true)
        //{
        //    Sonuc sonuc = new Sonuc();
        //    //Sonuc ayarSonuc = AyarBilgileriniAl();
        //    //AyarBilgileri ab = (AyarBilgileri)ayarSonuc.data;
        //    string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
        //    SqlConnection baglanti = new SqlConnection(baglantistr);
        //    string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
        //    int kdvFlag = Convert.ToInt32(ConfigurationManager.AppSettings["kdvDahilFlag"]);
        //    string depoKodu = ConfigurationManager.AppSettings["depoKodu"].ToString();
        //    string eksorgu = "";
        //    if (kriterCalissin)
        //        eksorgu = StokKartKriterleriniOlustur(sb);
        //    else
        //        eksorgu = sb.ekSorgu;
        //    SqlCommand komut = new SqlCommand();
        //    komut.CommandType = System.Data.CommandType.StoredProcedure;
        //    komut.CommandText = "StokKartlariniOku";
        //    komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
        //    komut.Parameters.AddWithValue("@depoKodu", depoKodu);
        //    komut.Parameters.AddWithValue("@eksorgu", eksorgu);

        //    SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
        //    DataTable tablo = asi.Komut_Adaptor(komut);
        //    if (tablo == null)
        //    {
        //        sonuc.data = null;
        //        sonuc.sonuc = false;
        //        sonuc.veriOkuBasari = false;
        //        sonuc.mesaj = "Stok Kart tablosu okunamadı." + asi.hataMesaji;
        //        return sonuc;
        //    }
        //    if (tablo.Rows.Count == 0)
        //    {
        //        sonuc.data = null;
        //        sonuc.sonuc = false;
        //        sonuc.veriOkuBasari = true;
        //        sonuc.mesaj = "Kriterleri uyan Stok Kartları bulunamadı." + asi.hataMesaji;
        //        return sonuc;
        //    }
        //    List<StokKartBilgileri> skbListe = new List<StokKartBilgileri>();
        //    for (int i = 0; i < tablo.Rows.Count; ++i)
        //    {

        //        StokKartBilgileri skb = new StokKartBilgileri();

        //        skb.aciklama1 = tablo.Rows[i]["STKACIK1"].ToString();
        //        skb.aciklama2 = tablo.Rows[i]["STKACIK2"].ToString();
        //        skb.aciklama3 = tablo.Rows[i]["STKACIK3"].ToString();
        //        skb.aciklama4 = tablo.Rows[i]["STKACIK4"].ToString();
        //        skb.aciklama5 = tablo.Rows[i]["STKACIK5"].ToString();
        //        skb.bakiye = Convert.ToDecimal(tablo.Rows[i]["STKBAKIYE"]);
        //        skb.birim = tablo.Rows[i]["STKBIRIM"].ToString();
        //        skb.fiyat = Convert.ToDecimal(tablo.Rows[i]["STKFIYAT"]);
        //        skb.dovizKodu = tablo.Rows[i]["STKDOVKOD"].ToString();
        //        skb.dovizTuru = tablo.Rows[i]["STKDOVTUR"].ToString();
        //        skb.kalemIndirim1 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ1"]);
        //        skb.kalemIndirim2 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ2"]);
        //        skb.kalemIndirim3 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ3"]);
        //        skb.kalemIndirim4 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ4"]);
        //        skb.kalemIndirim5 = Convert.ToDecimal(tablo.Rows[i]["STKISKYUZ5"]);
        //        decimal kdvsizFiyat = skb.fiyat;
        //        if (kdvFlag == 1)
        //        {
        //            kdvsizFiyat = skb.fiyat * 100 / 120;
        //        }
        //        decimal indirim1 = kdvsizFiyat * skb.kalemIndirim1 / 100;
        //        decimal indirim2 = (kdvsizFiyat - indirim1) * skb.kalemIndirim2 / 100;
        //        //çalışılacak
        //        skb.netFiyat = kdvsizFiyat - indirim1 - indirim2;
        //        skb.kdvOrani = Convert.ToDecimal(tablo.Rows[i]["STKKDVORAN"]);
        //        skb.ozelKod1 = tablo.Rows[i]["STKOZKOD1"].ToString();
        //        skb.ozelKod2 = tablo.Rows[i]["STKOZKOD2"].ToString();
        //        skb.ozelKod3 = tablo.Rows[i]["STKOZKOD3"].ToString();
        //        skb.ozelKod4 = tablo.Rows[i]["STKOZKOD4"].ToString();
        //        skb.ozelKod5 = tablo.Rows[i]["STKOZKOD5"].ToString();
        //        skb.resimBase64 = tablo.Rows[i]["STKRESIMPATH"].ToString();
        //        skb.stokCinsi = tablo.Rows[i]["STKCINSI"].ToString();
        //        skb.stokCinsi2 = tablo.Rows[i]["STKCINSI2"].ToString();
        //        skb.stokCinsi3 = tablo.Rows[i]["STKCINSI3"].ToString();
        //        skb.stokKodu = tablo.Rows[i]["STKKOD"].ToString();
        //        skb.barkod = tablo.Rows[i]["STKBARKOD"].ToString();
        //        skb.grupKodu = tablo.Rows[i]["STKGRUPKOD"].ToString();
        //        skb.kur = 50;
        //        skb.resimBase64 = "";
        //        string dataTipi = ResminUzantisiniAl(tablo.Rows[i]["STKRESIMPATH"].ToString());
        //        string dosya = tablo.Rows[i]["STKRESIMPATH"].ToString().Trim();
        //        if (File.Exists(dosya))
        //        {
        //            byte[] bytes = File.ReadAllBytes(tablo.Rows[i]["STKRESIMPATH"].ToString());
        //            skb.resimBase64 = Convert.ToBase64String(bytes);
        //            skb.resimBase64 = $"data:{dataTipi};base64," + skb.resimBase64;
        //        }
        //        skbListe.Add(skb);
        //    }
        //    sonuc.sonuc = true;
        //    sonuc.data = skbListe;
        //    sonuc.veriOkuBasari = true;
        //    sonuc.mesaj = "Başarılı";
        //    return sonuc;
        //}
        private string StokKartKriterleriniOlustur(SayfalamaBilgileri sb)
        {
            string eksorgu = "WHERE 1 = 1";
            string eksorgu1 = "";
            string eksorgu2 = "";
            if (sb == null)
                return eksorgu;
            if (!string.IsNullOrEmpty(sb.ekSorgu))
            {
                eksorgu = sb.ekSorgu;
            }
            else
            {
                if (sb.veriSorgulama != null)
                {
                    string json = sb.veriSorgulama.ToString();

                    StokFiltreBilgileri sfb = Newtonsoft.Json.JsonConvert.DeserializeObject<StokFiltreBilgileri>(json);
                    switch (sb.aramaTipiFlag)
                    {
                        case 0:
                            if (!string.IsNullOrEmpty(sfb.stokKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKKOD = '{sfb.stokKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKKOD = '{sfb.stokKodu}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.stokCinsi))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKCINSI = '{sfb.stokCinsi}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKCINSI = '{sfb.stokCinsi}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.barkod))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKBARKOD = '{sfb.barkod}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKBARKOD = '{sfb.barkod}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD1 = '{sfb.ozelKod1}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD1 = '{sfb.ozelKod1}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD2 = '{sfb.ozelKod2}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD2 = '{sfb.ozelKod2}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD3 = '{sfb.ozelKod3}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD3 = '{sfb.ozelKod3}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD4 = '{sfb.ozelKod4}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD4 = '{sfb.ozelKod4}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD5 = '{sfb.ozelKod5}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD5 = '{sfb.ozelKod5}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK1 = '{sfb.aciklama1}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK1 = '{sfb.aciklama1}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK2 = '{sfb.aciklama2}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK2 = '{sfb.aciklama2}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK3 = '{sfb.aciklama3}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK3 = '{sfb.aciklama3}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK4 = '{sfb.aciklama4}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK4 = '{sfb.aciklama4}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK5 = '{sfb.aciklama5}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK5 = '{sfb.aciklama5}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.grupKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKGRUPKOD = '{sfb.grupKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKGRUPKOD = '{sfb.grupKodu}'";
                            }
                            break;
                        case 1:
                            if (!string.IsNullOrEmpty(sfb.stokKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKKOD LIKE '{sfb.stokKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKKOD LIKE '{sfb.stokKodu}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.stokCinsi))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKCINSI LIKE '{sfb.stokCinsi}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKCINSI LIKE '{sfb.stokCinsi}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.barkod))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKBARKOD LIKE '{sfb.barkod}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKBARKOD LIKE '{sfb.barkod}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD1  LIKE '{sfb.ozelKod1}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD1 LIKE '{sfb.ozelKod1}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD2 LIKE '{sfb.ozelKod2}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD2  LIKE '{sfb.ozelKod2}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD3  LIKE '{sfb.ozelKod3}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD3  LIKE '{sfb.ozelKod3}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD4  LIKE '{sfb.ozelKod4}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD4  LIKE '{sfb.ozelKod4}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD5  LIKE '{sfb.ozelKod5}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD5  LIKE '{sfb.ozelKod5}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK1  LIKE '{sfb.aciklama1}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK1  LIKE '{sfb.aciklama1}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK2  LIKE '{sfb.aciklama2}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK2  LIKE '{sfb.aciklama2}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK3  LIKE '{sfb.aciklama3}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK3  LIKE '{sfb.aciklama3}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK4  LIKE '{sfb.aciklama4}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK4  LIKE '{sfb.aciklama4}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK5  LIKE '{sfb.aciklama5}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK5  LIKE '{sfb.aciklama5}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.grupKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKGRUPKOD  LIKE '{sfb.grupKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKGRUPKOD  LIKE '{sfb.grupKodu}%'";
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(sfb.stokKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKKOD  LIKE '%{sfb.stokKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKKOD  LIKE '%{sfb.stokKodu}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.stokCinsi))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKCINSI  LIKE '%{sfb.stokCinsi}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKCINSI  LIKE '%{sfb.stokCinsi}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.barkod))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKBARKOD  LIKE '%{sfb.barkod}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKBARKOD  LIKE '%{sfb.barkod}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD1  LIKE '%{sfb.ozelKod1}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD1  LIKE '%{sfb.ozelKod1}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD2  LIKE '%{sfb.ozelKod2}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD2  LIKE '%{sfb.ozelKod2}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD3  LIKE '%{sfb.ozelKod3}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD3  LIKE '%{sfb.ozelKod3}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD4  LIKE '%{sfb.ozelKod4}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD4  LIKE '%{sfb.ozelKod4}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD5  LIKE '%{sfb.ozelKod5}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD5  LIKE '%{sfb.ozelKod5}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK1  LIKE '%{sfb.aciklama1}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK1  LIKE '%{sfb.aciklama1}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK2  LIKE '%{sfb.aciklama2}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK2  LIKE '%{sfb.aciklama2}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK3  LIKE '%{sfb.aciklama3}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK3  LIKE '%{sfb.aciklama3}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK4  LIKE '%{sfb.aciklama4}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK4  LIKE '%{sfb.aciklama4}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK5  LIKE '%{sfb.aciklama5}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK5  LIKE '%{sfb.aciklama5}'";
                            }
                            if (!string.IsNullOrEmpty(sfb.grupKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKGRUPKOD  LIKE '%{sfb.grupKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKGRUPKOD  LIKE '%{sfb.grupKodu}'";
                            }
                            break;
                        case 3:
                            if (!string.IsNullOrEmpty(sfb.stokKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKKOD  LIKE '%{sfb.stokKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKKOD  LIKE '%{sfb.stokKodu}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.stokCinsi))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKCINSI  LIKE '%{sfb.stokCinsi}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKCINSI  LIKE '%{sfb.stokCinsi}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.barkod))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKBARKOD  LIKE '%{sfb.barkod}%'"; eksorgu1 = "var";}
                                else
                                    eksorgu2 += $" OR STKBARKOD  LIKE '%{sfb.barkod}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD1  LIKE '%{sfb.ozelKod1}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD1  LIKE '%{sfb.ozelKod1}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD2  LIKE '%{sfb.ozelKod2}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD2  LIKE '%{sfb.ozelKod2}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD3  LIKE '%{sfb.ozelKod3}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD3  LIKE '%{sfb.ozelKod3}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD4  LIKE '%{sfb.ozelKod4}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD4  LIKE '%{sfb.ozelKod4}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.ozelKod5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKOZKOD5  LIKE '%{sfb.ozelKod5}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKOZKOD5  LIKE '%{sfb.ozelKod5}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama1))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK1  LIKE '%{sfb.aciklama1}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK1  LIKE '%{sfb.aciklama1}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama2))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK2  LIKE '%{sfb.aciklama2}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK2  LIKE '%{sfb.aciklama2}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama3))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK3  LIKE '%{sfb.aciklama3}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK3  LIKE '%{sfb.aciklama3}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama4))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK4  LIKE '%{sfb.aciklama4}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK4  LIKE '%{sfb.aciklama4}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.aciklama5))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKACIK5  LIKE '%{sfb.aciklama5}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKACIK5  LIKE '%{sfb.aciklama5}%'";
                            }
                            if (!string.IsNullOrEmpty(sfb.grupKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" STKGRUPKOD  LIKE '%{sfb.grupKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR STKGRUPKOD  LIKE '%{sfb.grupKodu}%'";
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(eksorgu2))
                    {
                        eksorgu += " AND ( " + eksorgu2 + " )";
                    }
                }
            }
            sb.ekSorgu = eksorgu;
            if (sb.sayfaUzunlugu > 0)
            {
                switch (sb.siralamaTipiFlag)
                {
                    case 0:
                        eksorgu += $" ORDER BY STKKOD OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 1:
                        eksorgu += $" ORDER BY STKCINSI OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 2:
                        eksorgu += $" ORDER BY STKCINSI DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 3:
                        eksorgu += $" ORDER BY STKFIYAT OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 4:
                        eksorgu += $" ORDER BY STKFIYAT DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;                   
                }
            }
            return eksorgu;
        }
        private int StokSayisiniBul(String ekSorgu)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $"SELECT COUNT(STKKOD) FROM {etaVeriTabani}..STKKART";
            komutstr += " " + ekSorgu;
            SqlCommand komut = new SqlCommand(komutstr);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Komut_Adaptor(komut);
            try
            {
                return Convert.ToInt32(tablo.Rows[0][0]);
            }
            catch
            {
                return 0;
            }
        }
        private string ResminUzantisiniAl(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();

            switch (ext)
            {
                case ".png":
                    return "image/png";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                case ".pdf":
                    return "application/pdf";
                default:
                    return "application/octet-stream";
            }
        }
        private DataTable DovizKuruAl(string ekSorgu = "")
        {
           
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string masterVeriTabani = ConfigurationManager.AppSettings["MasterDBName"].ToString();
            string komutstr = $"SELECT DOVHARTUT FROM {masterVeriTabani}..DOVHAR";
            komutstr += " " + ekSorgu;
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tablo = asi.Sorgu_Adaptor(komutstr);
            return tablo;
        }
    }
}