using B2b_Api.Models;
using DevExpress.XtraReports.UI;
using ETA_Kayit_Islemleri;
using SQL_Genel_Islemleri;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using Sonuc = B2b_Api.Models.Sonuc;

namespace B2b_Api.Servisler
{
    public class CariIslemler
    {

        public Sonuc CariKartlariOku(SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            sonuc = CariKartlariniAl(sb);
            sonuc.ekData = CariSayisiniBul();
            return sonuc;
        }
        public Sonuc StokFiyatlariniCariListedenAl(List<StokKartBilgileri> skbListe, string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            List<ListedenStokFiyatBilgileri> fiyatListesi = CariListedenFiyatListesiniOku(cariKodu);
            foreach (StokKartBilgileri skb in skbListe)
            {
                foreach (ListedenStokFiyatBilgileri sflb in fiyatListesi)
                {
                    if (skb.grupKodu == sflb.stokKodu)
                    {
                        skb.kalemIndirim1 = sflb.kalemIndirimYuzde1;
                        skb.kalemIndirim2 = sflb.kalemIndirimYuzde2;
                        skb.kalemIndirim3 = sflb.kalemIndirimYuzde3;
                        skb.kalemIndirim4 = sflb.kalemIndirimYuzde4;
                        skb.kalemIndirim5 = sflb.kalemIndirimYuzde5;
                        skb.fiyat = sflb.fiyat;
                       
                        skb.fiyatNo = sflb.fiyatNo;
                        skb.dovizKodu = sflb.dovizKodu;
                        skb.dovizTuru = sflb.dovizTuru;
                        if (skb.fiyat == 0)
                        {
                            StokFiyatiniBul(skb);
                        }
                        decimal netFiyat = skb.fiyat;

                        netFiyat *= (1 - skb.kalemIndirim1 / 100);
                        netFiyat *= (1 - skb.kalemIndirim2 / 100);
                        netFiyat *= (1 - skb.kalemIndirim3 / 100);
                        netFiyat *= (1 - skb.kalemIndirim4 / 100);
                        netFiyat *= (1 - skb.kalemIndirim5 / 100);
                        skb.netFiyat = netFiyat;
                        break;
                    }
                }
            }
            sonuc.sonuc = true;
            sonuc.data = skbListe;
            sonuc.mesaj = "Başarılı";
            return sonuc;
        }
        /*{
  "gecerliSayfaNo": 1,
  "sayfaUzunlugu": 20,
  "siralamaTipiFlag": 0,
  "aramaTipiFlag": 0,
  "karakterDuyarTipiFlag": 0,
  "ekSorgu": "",
  "veriSorgulama": {"stokKodu":"YG D13"}
}*/
        private StokKartBilgileri StokFiyatiniBul(StokKartBilgileri skb)
        {
            StokIslemleri si = new StokIslemleri();
            DataTable tablo = si.StokFiyatiniBul(skb.stokKodu, skb.fiyatNo);
            if (tablo == null)
                return skb;
            if (tablo.Rows.Count == 0)
                return skb;
            skb.fiyat = Convert.ToDecimal(tablo.Rows[0]["STKFIYTUTAR"]);
            skb.dovizKodu = tablo.Rows[0]["STKFIYDOVKOD"].ToString();
            skb.dovizTuru = tablo.Rows[0]["STKFIYDOVTUR"].ToString();
            return skb;
        }

        public Sonuc AdresleriOku(string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            List<AdresKartBilgileri> adresListesi = new List<AdresKartBilgileri>();
            string hataMesaji = string.Empty;
            DataTable tablo = AdresKartBilgileriniOku("WHERE ADRKOD1 = '" + cariKodu + "'", ref hataMesaji);
            if (tablo == null)
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Adres bilgileri bulunamadı. " + hataMesaji;
                sonuc.veriOkuBasari = false;

                return sonuc;
            }
            foreach (DataRow dr in tablo.Rows)
            {
                AdresKartBilgileri ab = new AdresKartBilgileri();
                ab.siraNo = Convert.ToInt32(dr["ADRITEMNO"]);
                ab.adres1 = dr["ADRADRES1"].ToString();
                ab.adres2 = dr["ADRADRES2"].ToString();
                ab.adres3 = dr["ADRADRES3"].ToString();


                ab.il = dr["ADRIL"].ToString();
                ab.ilce = dr["ADRILCE"].ToString();
                ab.ulke = dr["ADRULKE"].ToString();
                adresListesi.Add(ab);
            }
            sonuc.sonuc = true;
            sonuc.veriOkuBasari = true;
            sonuc.data = adresListesi;
            sonuc.mesaj = "Başarılı";
            return sonuc;
        }

        public DataRow CariAdresBilgileriniAl(string cariKodu, int adresNo)
        {
            DataTable tablo = new DataTable();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $@"SELECT CARVERDAIRE, CARVERHESNO, ISNULL(ADRADRES1, '') AS ADRADRES1, ISNULL(ADRADRES2, '') AS ADRADRES2, ISNULL(ADRADRES3, '') AS ADRADRES3, ISNULL(ADREMAIL1, '') AS ADREMAIL1, ISNULL(ADREMAIL2, '') AS ADREMAIL2, ISNULL(ADREMAIL3, '') AS ADREMAIL3, ISNULL(ADRIL, '') AS ADRIL, ISNULL(ADRILCE, '') AS ADRILCE, ISNULL(ADRULKE, '') AS ADRULKE, ADRTEL1, ISNULL(KIMMERNISNO, '') AS KIMMERNISNO FROM {etaVeriTabani}..CARKART LEFT JOIN(SELECT ADRADRES1, ADRADRES2, ADRADRES3, ADREMAIL1, ADREMAIL2, ADREMAIL3, ADRITEMNO, ADRIL, ADRILCE, ADRULKE, ADRKOD1, ADRTEL1 FROM  {etaVeriTabani}..ADRESLER) ADRESLER ON ADRESLER.ADRKOD1 = CARKOD AND ADRITEMNO = {adresNo} LEFT JOIN (select KIMKOD, KIMMERNISNO from {etaVeriTabani}..KIMLIKLER) KIMLIKLER ON KIMLIKLER.KIMKOD = CARKOD WHERE CARKOD = '{cariKodu}'";
            SqlCommand komut = new SqlCommand(komutstr);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            tablo = asi.Komut_Adaptor(komut);
            if (tablo != null && tablo.Rows.Count > 0)
            {
                return tablo.Rows[0];
            }
            return null;


        }
        private List<ListedenStokFiyatBilgileri> CariListedenFiyatListesiniOku(string cariKodu)
        {
            List<ListedenStokFiyatBilgileri> fiyatListesi = new List<ListedenStokFiyatBilgileri>();
            DataTable tabloCariFiyat = CariFiyatListesiniAl(cariKodu);
            DataTable tabloStokFiyatPaket = new DataTable();
            DataTable tabloStokGrup = new DataTable();
            DataTable tabloStokKart = new DataTable();
            if (tabloCariFiyat != null && tabloCariFiyat.Rows.Count > 0)
            {
                for (int i = 0; i < tabloCariFiyat.Rows.Count; ++i)
                {
                    if (tabloCariFiyat.Rows[i]["Stok Tipi"].ToString() == "3")
                    {
                        decimal kalemIndirimYuzde1 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 1"]);
                        decimal kalemIndirimYuzde2 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 2"]);
                        decimal kalemIndirimYuzde3 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 3"]);
                        decimal kalemIndirimYuzde4 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 4"]);
                        decimal kalemIndirimYuzde5 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 5"]);
                        decimal fiyatNo = 0;
                        try
                        {
                            fiyatNo = Convert.ToInt32(tabloCariFiyat.Rows[i]["Fiyat No"]);
                        }
                        catch (Exception)
                        { }
                        tabloStokFiyatPaket = StokFiyatPaketleriniOku(cariKodu);
                        if (tabloStokFiyatPaket != null && tabloStokFiyatPaket.Rows.Count > 0)
                        {
                            for (int j = 0; j < tabloStokFiyatPaket.Rows.Count; ++j)
                            {
                                tabloStokFiyatPaket.Rows[j]["Fiyat No"] = fiyatNo > 0 ? fiyatNo : tabloStokFiyatPaket.Rows[j]["Fiyat No"];
                                tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 1"] = kalemIndirimYuzde1 > 0 ? kalemIndirimYuzde1 : tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 1"];
                                tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 2"] = kalemIndirimYuzde2 > 0 ? kalemIndirimYuzde2 : tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 2"];
                                tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 3"] = kalemIndirimYuzde3 > 0 ? kalemIndirimYuzde3 : tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 3"];
                                tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 4"] = kalemIndirimYuzde4 > 0 ? kalemIndirimYuzde4 : tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 4"];
                                tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 5"] = kalemIndirimYuzde5 > 0 ? kalemIndirimYuzde5 : tabloStokFiyatPaket.Rows[j]["İskonto Yüzde 5"];
                            }
                        }
                    }
                    if (tabloCariFiyat.Rows[i]["Stok Tipi"].ToString() == "2")
                    {
                        decimal kalemIndirimYuzde1 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 1"]);
                        decimal kalemIndirimYuzde2 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 2"]);
                        decimal kalemIndirimYuzde3 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 3"]);
                        decimal kalemIndirimYuzde4 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 4"]);
                        decimal kalemIndirimYuzde5 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 5"]);
                        int fiyatNo = 0;
                        try
                        {
                            fiyatNo = Convert.ToInt32(tabloCariFiyat.Rows[i]["Fiyat No"]);
                        }
                        catch (Exception)
                        { }
                        StokIslemleri si = new StokIslemleri();
                        tabloStokGrup = si.StokGrupListesiniAl(tabloCariFiyat.Rows[i]["Stok Kodu"].ToString(), fiyatNo);
                        if (tabloStokGrup != null && tabloStokGrup.Rows.Count > 0)
                        {
                            for (int k = 0; k < tabloStokGrup.Rows.Count; ++k)
                            {
                                tabloStokGrup.Rows[k]["Fiyat No"] = fiyatNo > 0 ? fiyatNo : tabloStokGrup.Rows[k]["Fiyat No"];
                                tabloStokGrup.Rows[k]["İskonto Yüzde 1"] = kalemIndirimYuzde1 > 0 ? kalemIndirimYuzde1 : tabloStokGrup.Rows[k]["İskonto Yüzde 1"];
                                tabloStokGrup.Rows[k]["İskonto Yüzde 2"] = kalemIndirimYuzde2 > 0 ? kalemIndirimYuzde2 : tabloStokGrup.Rows[k]["İskonto Yüzde 2"];
                                tabloStokGrup.Rows[k]["İskonto Yüzde 3"] = kalemIndirimYuzde3 > 0 ? kalemIndirimYuzde3 : tabloStokGrup.Rows[k]["İskonto Yüzde 3"];
                                tabloStokGrup.Rows[k]["İskonto Yüzde 4"] = kalemIndirimYuzde4 > 0 ? kalemIndirimYuzde4 : tabloStokGrup.Rows[k]["İskonto Yüzde 4"];
                                tabloStokGrup.Rows[k]["İskonto Yüzde 5"] = kalemIndirimYuzde5 > 0 ? kalemIndirimYuzde5 : tabloStokGrup.Rows[k]["İskonto Yüzde 5"];
                            }
                        }
                    }
                }
                tabloCariFiyat.DefaultView.RowFilter = "[Stok Tipi] = '1'";
                tabloStokKart = tabloCariFiyat.DefaultView.ToTable();
                tabloStokKart = ListedenGelenStokKartinaFiyatDovizBilgileriniEkleme(tabloStokKart);

                for (int i = 0; i < tabloStokFiyatPaket.Rows.Count; ++i)
                {
                    ListedenStokFiyatBilgileri lsfb = new ListedenStokFiyatBilgileri();
                    lsfb.stokKodu = tabloStokFiyatPaket.Rows[i]["Stok Kodu"].ToString();
                    lsfb.fiyatNo = Convert.ToInt32(tabloStokFiyatPaket.Rows[i]["Fiyat No"]);
                    lsfb.kalemIndirimYuzde1 = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["İskonto Yüzde 1"]);
                    lsfb.kalemIndirimYuzde2 = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["İskonto Yüzde 2"]);
                    lsfb.kalemIndirimYuzde3 = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["İskonto Yüzde 3"]);
                    lsfb.kalemIndirimYuzde4 = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["İskonto Yüzde 4"]);
                    lsfb.kalemIndirimYuzde5 = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["İskonto Yüzde 5"]);
                    lsfb.dovizKodu = tabloStokFiyatPaket.Rows[i]["Döviz Kodu"].ToString();
                    lsfb.dovizTuru = tabloStokFiyatPaket.Rows[i]["Döviz Türü"].ToString();
                    lsfb.fiyat = Convert.ToDecimal(tabloStokFiyatPaket.Rows[i]["Fiyat"]);
                    decimal netFiyat = lsfb.fiyat; ;

                    netFiyat *= (1 - lsfb.kalemIndirimYuzde1 / 100);
                    netFiyat *= (1 - lsfb.kalemIndirimYuzde2 / 100);
                    netFiyat *= (1 - lsfb.kalemIndirimYuzde3 / 100);
                    netFiyat *= (1 - lsfb.kalemIndirimYuzde4 / 100);
                    netFiyat *= (1 - lsfb.kalemIndirimYuzde5 / 100);
                    lsfb.netFiyat = netFiyat;
                    fiyatListesi.Add(lsfb);
                }
                for (int i = 0; i < tabloStokGrup.Rows.Count; ++i)
                {
                    bool kontrol = false;
                    foreach (ListedenStokFiyatBilgileri bilgi in fiyatListesi)
                    {
                        if (bilgi.stokKodu == tabloStokGrup.Rows[i]["Stok Kodu"].ToString())
                        {
                            bilgi.fiyatNo = Convert.ToInt32(tabloStokGrup.Rows[i]["Fiyat No"]);
                            bilgi.kalemIndirimYuzde1 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 1"]);
                            bilgi.kalemIndirimYuzde2 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 2"]);
                            bilgi.kalemIndirimYuzde3 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 3"]);
                            bilgi.kalemIndirimYuzde4 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 4"]);
                            bilgi.kalemIndirimYuzde5 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 5"]);
                            bilgi.dovizKodu = tabloStokGrup.Rows[i]["Döviz Kodu"].ToString();
                            bilgi.dovizTuru = tabloStokGrup.Rows[i]["Döviz Türü"].ToString();
                            bilgi.fiyat = Convert.ToDecimal(tabloStokGrup.Rows[i]["Fiyat"]);
                            decimal netFiyat = bilgi.fiyat; ;

                            netFiyat *= (1 - bilgi.kalemIndirimYuzde1 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde2 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde3 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde4 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde5 / 100);
                            bilgi.netFiyat = netFiyat;
                            kontrol = true;
                        }
                    }
                    if (!kontrol)
                    {
                        ListedenStokFiyatBilgileri lsfb = new ListedenStokFiyatBilgileri();
                        lsfb.stokKodu = tabloStokGrup.Rows[i]["Stok Kodu"].ToString();
                        lsfb.fiyatNo = Convert.ToInt32(tabloStokGrup.Rows[i]["Fiyat No"]);
                        lsfb.kalemIndirimYuzde1 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 1"]);
                        lsfb.kalemIndirimYuzde2 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 2"]);
                        lsfb.kalemIndirimYuzde3 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 3"]);
                        lsfb.kalemIndirimYuzde4 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 4"]);
                        lsfb.kalemIndirimYuzde5 = Convert.ToDecimal(tabloStokGrup.Rows[i]["İskonto Yüzde 5"]);
                        lsfb.dovizKodu = tabloStokGrup.Rows[i]["Döviz Kodu"].ToString();
                        lsfb.dovizTuru = tabloStokGrup.Rows[i]["Döviz Türü"].ToString();
                        lsfb.fiyat = Convert.ToDecimal(tabloStokGrup.Rows[i]["Fiyat"]);
                        decimal netFiyat = lsfb.fiyat; ;

                        netFiyat *= (1 - lsfb.kalemIndirimYuzde1 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde2 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde3 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde4 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde5 / 100);
                        lsfb.netFiyat = netFiyat;
                        fiyatListesi.Add(lsfb);
                    }
                }
                for (int i = 0; i < tabloStokKart.Rows.Count; ++i)
                {
                    bool kontrol = false;
                    foreach (ListedenStokFiyatBilgileri bilgi in fiyatListesi)
                    {
                        if (bilgi.stokKodu == tabloStokKart.Rows[i]["Stok Kodu"].ToString())
                        {
                            bilgi.fiyatNo = Convert.ToInt32(tabloStokKart.Rows[i]["Fiyat No"]);
                            bilgi.kalemIndirimYuzde1 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 1"]);
                            bilgi.kalemIndirimYuzde2 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 2"]);
                            bilgi.kalemIndirimYuzde3 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 3"]);
                            bilgi.kalemIndirimYuzde4 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 4"]);
                            bilgi.kalemIndirimYuzde5 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 5"]);
                            bilgi.dovizKodu = tabloStokKart.Rows[i]["Döviz Kodu"].ToString();
                            bilgi.dovizTuru = tabloStokKart.Rows[i]["Döviz Türü"].ToString();
                            bilgi.fiyat = Convert.ToDecimal(tabloStokKart.Rows[i]["Fiyat"]);
                            decimal netFiyat = bilgi.fiyat; ;

                            netFiyat *= (1 - bilgi.kalemIndirimYuzde1 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde2 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde3 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde4 / 100);
                            netFiyat *= (1 - bilgi.kalemIndirimYuzde5 / 100);
                            bilgi.netFiyat = netFiyat;
                            kontrol = true;
                        }
                    }
                    if (!kontrol)
                    {
                        ListedenStokFiyatBilgileri lsfb = new ListedenStokFiyatBilgileri();
                        lsfb.stokKodu = tabloStokKart.Rows[i]["Stok Kodu"].ToString();
                        lsfb.fiyatNo = Convert.ToInt32(tabloStokKart.Rows[i]["Fiyat No"]);
                        lsfb.kalemIndirimYuzde1 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 1"]);
                        lsfb.kalemIndirimYuzde2 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 2"]);
                        lsfb.kalemIndirimYuzde3 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 3"]);
                        lsfb.kalemIndirimYuzde4 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 4"]);
                        lsfb.kalemIndirimYuzde5 = Convert.ToDecimal(tabloStokKart.Rows[i]["İskonto Yüzde 5"]);
                        lsfb.dovizKodu = tabloStokKart.Rows[i]["Döviz Kodu"].ToString();
                        lsfb.dovizTuru = tabloStokKart.Rows[i]["Döviz Türü"].ToString();
                        lsfb.fiyat = Convert.ToDecimal(tabloStokKart.Rows[i]["Fiyat"]);
                        decimal netFiyat = lsfb.fiyat; ;

                        netFiyat *= (1 - lsfb.kalemIndirimYuzde1 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde2 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde3 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde4 / 100);
                        netFiyat *= (1 - lsfb.kalemIndirimYuzde5 / 100);
                        lsfb.netFiyat = netFiyat;
                        fiyatListesi.Add(lsfb);
                    }
                }
            }
            foreach (ListedenStokFiyatBilgileri bilgi in fiyatListesi)
            {
                if (bilgi.fiyatNo == 0)
                {
                    bilgi.fiyatNo = Convert.ToInt32(ConfigurationManager.AppSettings["fiyatNo"]);
                }
               
            }
            return fiyatListesi;
        }
        private List<ListedenStokFiyatBilgileri> CariListedenFiyatListesiOku2(string cariKodu)
        {
            List<ListedenStokFiyatBilgileri> fiyatListesi = new List<ListedenStokFiyatBilgileri>();
            DataTable tabloCariFiyat = CariFiyatListesiniAl(cariKodu);
            if (tabloCariFiyat != null && tabloCariFiyat.Rows.Count > 0)
            {
                for (int i = 0; i < tabloCariFiyat.Rows.Count; ++i)
                {
                    ListedenStokFiyatBilgileri lsfb = new ListedenStokFiyatBilgileri();
                    lsfb.stokKodu = tabloCariFiyat.Rows[i]["Stok Kodu"].ToString();
                    lsfb.fiyatNo = Convert.ToInt32(tabloCariFiyat.Rows[i]["Fiyat No"]);
                    lsfb.kalemIndirimYuzde1 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 1"]);
                    lsfb.kalemIndirimYuzde2 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 2"]);
                    lsfb.kalemIndirimYuzde3 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 3"]);
                    lsfb.kalemIndirimYuzde4 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 4"]);
                    lsfb.kalemIndirimYuzde5 = Convert.ToDecimal(tabloCariFiyat.Rows[i]["İskonto Yüzde 5"]);
                    fiyatListesi.Add(lsfb);
                }
            }
            return fiyatListesi;
        }   
        private DataTable CariFiyatListesiniAl(string cariKodu)
        {
            DataTable tablo = new DataTable();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $"select CARFIYSTKTIP as [Stok Tipi], CARFIYSTKKOD as [Stok Kodu], CARFIYNO as [Fiyat No], CARFIYISKYUZ1 as [İskonto Yüzde 1], CARFIYISKYUZ2 as [İskonto Yüzde 2], CARFIYISKYUZ3 as [İskonto Yüzde 3], CARFIYISKYUZ4 as [İskonto Yüzde 4], CARFIYISKYUZ5 as [İskonto Yüzde 5] from {etaVeriTabani}..CARFIYAT WHERE CARFIYKOD = (SELECT CARLISFIYNO FROM {etaVeriTabani}..CARKART WHERE CARKOD = '{cariKodu}') AND CARFIYKODTIP = 1 AND CARFIYITEMNO > 0 ";
            SqlCommand komut = new SqlCommand(komutstr);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            return asi.Komut_Adaptor(komut);
        }
        public DataTable StokFiyatPaketleriniOku(string cariKodu)
        {
            DataTable tablo = new DataTable();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $"SELECT CARFIYSTKTIP AS [Stok Tipi], CARFIYSTKKOD AS [Stok Kodu], CARFIYNO AS [Fiyat No], ISNULL(STKFIYTUTAR, 0) as Fiyat, ISNULL(STKFIYDOVKOD, '') AS [Döviz Kodu], ISNULL(STKFIYDOVTUR, '') as [Döviz Türü], CARFIYISKYUZ1 AS [İskonto Yüzde 1], CARFIYISKYUZ2 AS [İskonto Yüzde 2], CARFIYISKYUZ3 AS [İskonto Yüzde 3], CARFIYISKYUZ4 as [İskonto Yüzde 4], CARFIYISKYUZ5 as [İskonto Yüzde 5] FROM {etaVeriTabani}..CARFIYAT LEFT JOIN (SELECT STKFIYSTKKOD, STKFIYNO, STKFIYTUTAR, STKFIYDOVKOD, STKFIYDOVTUR FROM {etaVeriTabani}..STKFIYAT) sf ON STKFIYSTKKOD = CARFIYSTKKOD AND STKFIYNO = CARFIYNO WHERE CARFIYKOD = (SELECT CARFIYSTKKOD FROM {etaVeriTabani}..CARFIYAT WHERE CARFIYKOD  = (SELECT CARLISFIYNO FROM {etaVeriTabani}..CARKART WHERE CARKOD = '{cariKodu}') AND CARFIYSTKTIP = 3) AND CARFIYKODTIP = 2 AND CARFIYITEMNO > 0";
            /*seLECT CARFIYSTKTIP AS [Stok Tipi], CARFIYSTKKOD AS [Stok Kodu], CARFIYNO AS [Fiyat No], ISNULL(STKFIYTUTAR, 0) as Fiyat, ISNULL(STKFIYDOVKOD, '') AS [Döviz Kodu], 
ISNULL(STKFIYDOVTUR, '') as [Döviz Türü] , CARFIYISKYUZ1 AS [İskonto Yüzde 1], CARFIYISKYUZ2 AS [İskonto Yüzde 2], 
CARFIYISKYUZ3 AS [İskonto Yüzde 3], CARFIYISKYUZ4 as [İskonto Yüzde 4], CARFIYISKYUZ5 as [İskonto Yüzde 5] 
FROM CARFIYAT 
LEFT JOIN (SELECT STKFIYSTKKOD, STKFIYNO, STKFIYTUTAR, STKFIYDOVKOD, STKFIYDOVTUR FROM STKFIYAT) sf ON STKFIYSTKKOD = CARFIYSTKKOD AND STKFIYNO = CARFIYNO
WHERE CARFIYKOD = (SELECT CARLISFIYNO FROM  CARKART WHERE CARKOD = '120 01 001') AND CARFIYKODTIP = 2 AND CARFIYITEMNO > 0*/
        SqlCommand komut = new SqlCommand(komutstr);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            return asi.Komut_Adaptor(komut);
        }
        //(SELECT CARFIYSTKKOD FROM CARFIYAT WHERE CARFIYKOD  = (SELECT CARLISFIYNO FROM CARKART WHERE CARKOD = '120 01 O00001') AND CARFIYSTKTIP = 3)
        private Sonuc CariKartlariniAl(SayfalamaBilgileri sb)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string eksorgu = CariKartKriterleriniAl(sb);
            SqlCommand komut = new SqlCommand();
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.CommandText = "CariKartlariniOku";
            komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
            komut.Parameters.AddWithValue("@eksorgu", eksorgu);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tabloCari = asi.Komut_Adaptor(komut);
            if (tabloCari == null)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.ekData = 0;
                if (tabloCari == null)
                    sonuc.mesaj = asi.hataMesaji;
                if (tabloCari.Rows.Count == 0)
                {
                    sonuc.mesaj = "İlgili kritere ait kart bulunamadı.";
                }
                return sonuc;
            }
            List<CariKartBilgileri> ckbListe = new List<CariKartBilgileri>();
            for (int i = 0; i < tabloCari.Rows.Count; ++i)
            {
                CariKartBilgileri ckb = new CariKartBilgileri();
                ckb.bakiye = Convert.ToDecimal(tabloCari.Rows[i]["Bakiye"]);
                ckb.cariKodu = tabloCari.Rows[i]["Cari Kodu"].ToString();
                ckb.cariUnvani = tabloCari.Rows[i]["Cari Ünvanı"].ToString();
                ckb.iskonto = Convert.ToDecimal(tabloCari.Rows[i]["İskonto"]);
                ckb.vergiDairesi = tabloCari.Rows[i]["Vergi Dairesi"].ToString();
                ckb.vergiNumarasi = tabloCari.Rows[i]["Vergi Numarası"].ToString();
                ckb.kimlikNo = tabloCari.Rows[i]["Kimlik Numarası"].ToString();
                ckb.yetkili = tabloCari.Rows[i]["Yetkili"].ToString();
                ckb.adres1 = tabloCari.Rows[i]["Adres1"].ToString();
                ckb.adres2 = tabloCari.Rows[i]["Adres2"].ToString();
                ckb.adres3 = tabloCari.Rows[i]["Adres3"].ToString();
                ckb.il = tabloCari.Rows[i]["İl"].ToString();
                ckb.ilce = tabloCari.Rows[i]["İlçe"].ToString();
                ckb.ulke = tabloCari.Rows[i]["Ülke"].ToString();
                ckb.telefon = tabloCari.Rows[i]["Telefon"].ToString();
                ckb.email = tabloCari.Rows[i]["Email"].ToString();
                ckb.temsilci = tabloCari.Rows[i]["Temsilci"].ToString();

                ckbListe.Add(ckb);
            }
            sonuc.sonuc = true;
            sonuc.data = ckbListe;
            sonuc.mesaj = "Başarılı";
            return sonuc;
        }

        private int CariSayisiniBul()
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            string komutstr = $"SELECT COUNT(CARKOD) FROM {etaVeriTabani}..CARKART";
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
        private string CariKartKriterleriniAl(SayfalamaBilgileri sb)
        {
            string eksorgu = "WHERE 1 = 1";
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
                    CariKartBilgileri ckb = Newtonsoft.Json.JsonConvert.DeserializeObject<CariKartBilgileri>(json);
                    int sayac = 0;
                    if (!string.IsNullOrEmpty(ckb.cariKodu))
                    {
                        sayac++;
                    }
                    if (!string.IsNullOrEmpty(ckb.cariUnvani))
                    {
                        sayac++;
                    }
                    if (sayac > 1)
                    {
                        int flag = 0;
                        eksorgu += " AND (";
                        if (!string.IsNullOrEmpty(ckb.cariKodu))
                        {
                            eksorgu += $" CARKOD LIKE '%{ckb.cariKodu}%'";
                            flag = 1;
                        }
                        if (!string.IsNullOrEmpty(ckb.cariUnvani))
                        {
                            if (flag == 1)
                                eksorgu += " OR ";
                            eksorgu += $" CARUNVAN LIKE '%{ckb.cariUnvani}%'";
                            flag = 1;
                        }
                    }
                    else
                    {
                        int flag = 0;
                        eksorgu += " AND (";
                        if (!string.IsNullOrEmpty(ckb.cariKodu))
                        {
                            eksorgu += $" CARKOD = '{ckb.cariKodu}'";
                            flag = 1;
                        }
                        if (!string.IsNullOrEmpty(ckb.cariUnvani))
                        {
                            if (flag == 1)
                                eksorgu += " AND ";
                            eksorgu += $" CARUNVAN = '{ckb.cariUnvani}'";
                            flag = 1;
                        }
                    }
                    eksorgu += " )";
                }
            }
            if (sb.sayfaUzunlugu > 0)
            {
                switch (sb.siralamaTipiFlag)
                {
                    case 0:
                        eksorgu += $" ORDER BY CARKOD OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 1:
                        eksorgu += $" ORDER BY CARUNVAN OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 2:
                        eksorgu += $" ORDER BY CARUNVAN DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 3:
                        eksorgu += $" ORDER BY CARBAKIYE OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 4:
                        eksorgu += $" ORDER BY CARBAKIYE DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                }
            }

            return eksorgu;
        }
        private DataTable ListedenGelenStokKartinaFiyatDovizBilgileriniEkleme(DataTable tablo)
        {
            tablo.Columns.Add("Fiyat", typeof(decimal));
            tablo.Columns.Add("Döviz Kodu");
            tablo.Columns.Add("Döviz Türü");
            StokIslemleri si = new StokIslemleri();
            foreach (DataRow satir in tablo.Rows)
            {
                DataTable tabloListeStok = si.ListeStokKartListesiniAl(satir["Stok Kodu"].ToString(), Convert.ToInt32(satir["Fiyat No"]));
                satir["Döviz Kodu"] = "";
                satir["Döviz Türü"] = "";
                satir["Fiyat"] = 0;
                if (tabloListeStok != null && tabloListeStok.Rows.Count > 0)
                {
                    satir["Döviz Kodu"] = tabloListeStok.Rows[0]["Döviz Kodu"].ToString();
                    satir["Döviz Türü"] = tabloListeStok.Rows[0]["Döviz Türü"].ToString();
                    satir["Fiyat"] = Convert.ToDecimal(tabloListeStok.Rows[0]["Fiyat"]);
                }
            }
            return tablo;
        }
        public Sonuc SifreKontrol(string kod, string sifre)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            SqlCommand komut = new SqlCommand();
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.CommandText = "SifreKontrol";
            komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
            komut.Parameters.AddWithValue("@kod", kod);
            komut.Parameters.AddWithValue("@sifre", sifre);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            DataTable tabloCari = asi.Komut_Adaptor(komut);
            if (tabloCari == null)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = 0;
                sonuc.mesaj = asi.hataMesaji;

                return sonuc;
            }
            if (tabloCari.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = new List<CariKartBilgileri>();
                sonuc.ekData = 0;
                sonuc.mesaj = asi.hataMesaji;

                return sonuc;
            }
            List<CariKartBilgileri> ckbListe = new List<CariKartBilgileri>();
            for (int i = 0; i < tabloCari.Rows.Count; ++i)
            {
                CariKartBilgileri ckb = new CariKartBilgileri();
                ckb.bakiye = Convert.ToDecimal(tabloCari.Rows[i]["Bakiye"]);
                ckb.cariKodu = tabloCari.Rows[i]["Cari Kodu"].ToString();
                ckb.cariUnvani = tabloCari.Rows[i]["Cari Ünvanı"].ToString();
                ckb.iskonto = Convert.ToDecimal(tabloCari.Rows[i]["İskonto"]);
                ckb.vergiDairesi = tabloCari.Rows[i]["Vergi Dairesi"].ToString();
                ckb.vergiNumarasi = tabloCari.Rows[i]["Vergi Numarası"].ToString();
                ckb.kimlikNo = tabloCari.Rows[i]["Kimlik Numarası"].ToString();
                ckb.yetkili = tabloCari.Rows[i]["Yetkili"].ToString();
                ckb.adres1 = tabloCari.Rows[i]["Adres1"].ToString();
                ckb.adres2 = tabloCari.Rows[i]["Adres2"].ToString();
                ckb.adres3 = tabloCari.Rows[i]["Adres3"].ToString();
                ckb.il = tabloCari.Rows[i]["İl"].ToString();
                ckb.ilce = tabloCari.Rows[i]["İlçe"].ToString();
                ckb.ulke = tabloCari.Rows[i]["Ülke"].ToString();
                ckb.telefon = tabloCari.Rows[i]["Telefon"].ToString();
                ckb.email = tabloCari.Rows[i]["Email"].ToString();
                ckb.temsilci = tabloCari.Rows[i]["Temsilci"].ToString();
                ckbListe.Add(ckb);
            }
            sonuc.sonuc = true;
            sonuc.veriOkuBasari = true;
            sonuc.data = ckbListe;
            sonuc.mesaj = "Başarılı";
            return sonuc;
        }
        public Sonuc SifreKaydet(string kod, string sifre)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            SqlCommand komut = new SqlCommand();
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.CommandText = "SifreKaydet";
            komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
            komut.Parameters.AddWithValue("@kod", kod);
            komut.Parameters.AddWithValue("@sifre", sifre);
            SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
            if (!asi.Komut_ExecuteNonQuery(komut))
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Şifre kaydedilemedi." + asi.hataMesaji;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;

            }
            else
            {
                sonuc.sonuc = true;
                sonuc.mesaj = "Şifre başarı ile kaydedildi.";
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
            }
            return sonuc;
        }
        public Sonuc CariEkstrePDFAl(string basTarih, string bitTarih, string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            DataSet ds = new DataSet();
            DataTable tablo = CariEkstreOku(basTarih, bitTarih, cariKodu);
            if (tablo == null)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = false;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Cari Hareket tablosu okunamadı.";
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.sonuc = false;
                sonuc.veriOkuBasari = true;
                sonuc.data = null;
                sonuc.ekData = null;
                sonuc.mesaj = "Cariye ait hareket bulunamadı.";
                return sonuc;
            }
            tablo.TableName = "CariEkstre";
            ds.Tables.Add(tablo);
            string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Dizayn");
            string dizynDosyasi = mappedPath + "\\" + "CariEkstreDizayn" + ".repx";
            XtraReport rapor = new XtraReport();
            rapor.LoadLayoutFromXml(dizynDosyasi);
            rapor.DataSource = ds;
            rapor.ExportToPdf(Path.GetDirectoryName(dizynDosyasi) + $"//CariEkstre_{cariKodu}.pdf");
            FileStream file = new FileStream(Path.GetDirectoryName(dizynDosyasi) + $"//CariEkstre_{cariKodu}.pdf", FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[file.Length];
            file.Read(bytes, 0, (int)file.Length);
            file.Close();
            string base64String = Convert.ToBase64String(bytes);
            if (bytes == null)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = "Carei ekstre PDF'i okunamadı";
                return sonuc;
            }
            if (bytes.Length == 0)
            {
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.mesaj = "Cari Ekstre PDF'i bulunamadı";
                return sonuc;
            }
            sonuc.sonuc = true;
            sonuc.data = base64String;
            sonuc.ekData = Path.GetDirectoryName(dizynDosyasi) + $"//CariEkstre_{cariKodu}.pdf";
            sonuc.mesaj = "Başarılı.";
            return sonuc;
        }
        private DataTable CariEkstreOku(string basTarih, string bitTarih, string cariKodu)
        {
            DataTable sonuc = new DataTable();
            int ilkYil = Convert.ToInt32(basTarih.Substring(0, 4));
            int ilkAy = Convert.ToInt32(basTarih.Substring(4, 2));
            int ilkGun = Convert.ToInt32(basTarih.Substring(6, 2));
            DateTime ilkTarih = new DateTime(ilkYil, ilkAy, ilkGun);
            int sonYil = Convert.ToInt32(bitTarih.Substring(0, 4));
            int sonAy = Convert.ToInt32(bitTarih.Substring(4, 2));
            int sonGun = Convert.ToInt32(bitTarih.Substring(6, 2));
            DateTime sonTarih = new DateTime(sonYil, sonAy, sonGun);
            sonuc = CariEkstreyiOku(ilkTarih, sonTarih, cariKodu);

            return sonuc;
        }
        private DataTable CariEkstreyiOku(DateTime ilkTarih, DateTime sonTarih, string cariKodu)
        {

            try
            {

                string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
                SqlConnection baglanti = new SqlConnection(baglantistr);
                string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
                SqlCommand komut = new SqlCommand();
                komut.CommandType = System.Data.CommandType.StoredProcedure;
                komut.CommandText = "CariEkstreOku";
                komut.Parameters.AddWithValue("@veriTabaniAdi", etaVeriTabani);
                komut.Parameters.AddWithValue("@sonTarih", sonTarih);
                komut.Parameters.AddWithValue("@cariKodu", cariKodu);
                SQL_Genel_Islemleri.Ana_SQL_Islemleri asi = new SQL_Genel_Islemleri.Ana_SQL_Islemleri(baglanti);
                DataSet ds = asi.KomutDS_Adaptor(komut);
                DataTable tablo = EkstreTablosunuOlustur(ilkTarih, sonTarih, cariKodu, ds);
                return tablo;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private DataTable EkstreTablosunuOlustur(DateTime ilkTarih, DateTime sonTarih, string cariKodu, DataSet dsHam)
        {
            DataTable tablo = dsHam.Tables[0].Copy();
            tablo.DefaultView.Sort = "Tarih, [Evrak No]";
            DataTable ekstreTablo = tablo.Clone();
            DataTable tabloOnce = tablo.Copy();
            DataTable tabloSonra = tablo.Copy();

            //tabloOnce.DefaultView.RowFilter = string.Format("[Tarih] < '#{0}# '", ilkTarih.ToString("dd/MM/yyyy"));
            //tabloOnce = tabloOnce.DefaultView.ToTable();
            //tabloSonra.DefaultView.RowFilter = string.Format("[Tarih] >= '#{0}# '", ilkTarih.ToString("dd/MM/yyyy"));
            //tabloSonra = tabloSonra.DefaultView.ToTable();
            var onceQuery = tabloOnce.AsEnumerable().Where(x => x.Field<DateTime>("Tarih") < ilkTarih);

            tabloOnce = onceQuery.Any() ? onceQuery.CopyToDataTable() : tabloOnce.Clone();


            var sonraQuery = tabloSonra.AsEnumerable().Where(x => x.Field<DateTime>("Tarih") >= ilkTarih);

            tabloSonra = sonraQuery.Any() ? sonraQuery.CopyToDataTable() : tabloSonra.Clone();
            DataRow ilkSatir = ekstreTablo.NewRow();
            ilkSatir["Cari Kodu"] = cariKodu;
            ilkSatir["Tarih"] = ilkTarih.AddDays(-1);
            ilkSatir["Tip Kodu"] = "";
            ilkSatir["Evrak No"] = "";
            ilkSatir["Açıklama"] = "NAKLİ YEKÜN";
            ilkSatir["Vade"] = Convert.ToDateTime("01.01.1900");

            decimal toplamBakiye = 0;
            decimal toplamBorc = 0;
            decimal toplamAlacak = 0;
            for (int i = 0; i < tabloOnce.Rows.Count; ++i)
            {
                toplamBorc += Convert.ToDecimal(tabloOnce.Rows[i]["Borç"]);
                toplamAlacak += Convert.ToDecimal(tabloOnce.Rows[i]["Alacak"]);
                // toplamBakiye += Convert.ToDecimal(tabloOnce.Rows[i]["Borç"]) - Convert.ToDecimal(tabloOnce.Rows[i]["Alacak"]);
            }
            toplamBakiye = toplamBorc - toplamAlacak;
            ilkSatir["Borç"] = toplamBorc;
            ilkSatir["Alacak"] = toplamAlacak;
            ilkSatir["Bakiye"] = toplamBakiye;
            ekstreTablo.Rows.Add(ilkSatir);
            for (int i = 0; i < tabloSonra.Rows.Count; ++i)
            {
                toplamBakiye += Convert.ToDecimal(tabloSonra.Rows[i]["Borç"]) - Convert.ToDecimal(tabloSonra.Rows[i]["Alacak"]);
                tabloSonra.Rows[i]["Bakiye"] = toplamBakiye;
                ekstreTablo.ImportRow(tabloSonra.Rows[i]);
            }

            return ekstreTablo;
        }
        public DataTable AdresKartBilgileriniOku(string eksorgu, ref string hataMesaji)
        {
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string etaVeriTabani = ConfigurationManager.AppSettings["etaVeriTabani"].ToString();
            try
            {

                string komutstr = @"Select ADRITEMNO, ADRADRES1, ADRADRES2, ADRADRES3, ADRILCE, ADRIL,  ADRULKE from " + etaVeriTabani + @"..ADRESLER";
                komutstr += " " + eksorgu;
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(baglanti);
                DataTable tablo =  asi.Sorgu_Adaptor(komutstr, false, "Adres Kart Bilgilerini Oku");
                hataMesaji = asi.hataMesaji;
                return tablo;
            }
            catch (Exception ex)
            {
              hataMesaji = ex.Message;
                return null;
            }
        }

    }
}