using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class TeklifFisBilgileri
    {
        public int id { get; set; }
        public int fistipi { get; set; }
        public int kdvflag { get; set; }
        public int etakayitdurum { get; set; }
        public DateTime tarih { get; set; }
        public DateTime etakayittarih { get; set; }
        public decimal maltoplam { get; set; }
        public decimal iskontoyuzde1 { get; set; }
        public decimal iskontoyuzde2 { get; set; }
        public decimal iskontotoplam { get; set; }
        public decimal aratoplam { get; set; }
        public decimal kdvtoplam { get; set; }
        public decimal geneltoplam { get; set; }
        public decimal kur { get; set; }
        public string teklifno { get; set; }
        public string carikodu { get; set; }
        public string cariunvani { get; set; }
        public string yetkili { get; set; }
        public string adres1 { get; set; }
        public string adres2 { get; set; }
        public string dovizkodu { get; set; }
        public string dovizturu { get; set; }
        public string etasirketadi { get; set; }
        public string aciklama1 { get; set; }
        public string aciklama2 { get; set; }
        public string aciklama3 { get; set; }
        public string ozelkod1 { get; set; }
        public string ozelkod2 { get; set; }
        public string ozelkod3 { get; set; }
        public string adres3 { get; set; }
        public string ilce { get; set; }
        public string il { get; set; }
        public string ulke { get; set; }
        public string vergidairesi { get; set; }
        public string verginumarasi { get; set; }
        public object kimlikno { get; internal set; }
        public object email { get; internal set; }
        public object telefonno { get; internal set; }
        public int adresNo { get; set; }
        public string sevkadres1 { get; set; }
        public string sevkadres2 { get; set; }
        public string sevkadres3 { get; set; }
        public string sevkilce { get; set; }
        public string sevkil { get; set; }
        public string sevkulke { get; set; }
       

        public TeklifFisBilgileri()
        {
            id = 0;
            fistipi = 0;
            kdvflag = 0;
            etakayitdurum = 0;
            tarih = Convert.ToDateTime("01.01.1900");
            etakayittarih = Convert.ToDateTime("01.01.1900");
            maltoplam = 0;
            iskontoyuzde1 = 0;
            iskontoyuzde2 = 0;
            iskontotoplam = 0;
            aratoplam = 0;
            kdvtoplam = 0;
            geneltoplam = 0;
            kur = 0;
            teklifno = "";
            carikodu = "";
            cariunvani = "";
            yetkili = "";
            adres1 = "";
            adres2 = "";
            dovizkodu = "";
            dovizturu = "";
            etasirketadi = "";
            aciklama1 = "";
            aciklama2 = "";
            aciklama3 = "";
            ozelkod1 = "";
            ozelkod2 = "";
            ozelkod3 = "";
            adres3 = "";
            ilce = "";
            il = "";
            ulke = "";
            vergidairesi = "";
            verginumarasi = "";
            adresNo = 0;
            sevkadres1= "";
            sevkadres2 = "";
            sevkadres3 = "";    
            sevkilce = ""; 
            sevkil = "";
            sevkulke = "";
        }
    }

    public class TeklifHareketBilgileri
    {
        public int id { get; set; }
        public int fisid { get; set; }
        public DateTime tarih { get; set; }
        public DateTime vadetarihi { get; set; }
        public DateTime termintarihi { get; set; }
        public decimal miktar { get; set; }
        public decimal fiyat { get; set; }
        public decimal indirimyuzde1 { get; set; }
        public decimal indirimyuzde2 { get; set; }
        public decimal indirimyuzde3 { get; set; }
        public decimal indirimyuzde4 { get; set; }
        public decimal indirimyuzde5 { get; set; }
        public decimal indirimtoplam { get; set; }
        public decimal kdvyuzde { get; set; }
        public decimal kdvtutar { get; set; }
        public decimal tutar { get; set; }
        public decimal kur { get; set; }
        public string teklifno { get; set; }
        public string stokkodu { get; set; }
        public string stokcinsi { get; set; }
        public string aciklama { get; set; }
        public string aciklama1 { get; set; }
        public string aciklama2 { get; set; }
        public string aciklama3 { get; set; }
        public string ozelkod { get; set; }
        public string birim { get; set; }
        public string depokodu { get; set; }
        public string dovizkodu { get; set; }
        public string dovizturu { get; set; }
        public decimal netTutar { get; set; }


        public TeklifHareketBilgileri()
        {
            id = 0;
            fisid = 0;
            tarih = Convert.ToDateTime("01.01.1900");
            vadetarihi = Convert.ToDateTime("01.01.1900");
            termintarihi = Convert.ToDateTime("01.01.1900");
            miktar = 0;
            fiyat = 0;
            indirimyuzde1 = 0;
            indirimyuzde2 = 0;
            indirimyuzde3 = 0;
            indirimyuzde4 = 0;
            indirimyuzde5 = 0;
            indirimtoplam = 0;
            kdvyuzde = 0;
            kdvtutar = 0;
            tutar = 0;
            kur = 0;
            teklifno = "";
            stokkodu = "";
            stokcinsi = "";
            aciklama = "";
            aciklama1 = "";
            aciklama2 = "";
            aciklama3 = "";
            ozelkod = "";
            birim = "";
            depokodu = "";
            dovizkodu = "";
            dovizturu = "";
            netTutar = 0;
        }
    }

    public class TeklifFiltreBilgileri
    {
        public string cariKodu { get; set; }
        public string cariUnvani { get; set; }
        public string teklifNo { get; set; }
        public string baslangicTarihi { get; set; }
        public string bitisTarihi { get; set; }
        public TeklifFiltreBilgileri()
        {
            cariKodu = "";
            cariUnvani = "";
            teklifNo = "";
            baslangicTarihi = "19000101";
            bitisTarihi = "19000101";
        }
    }
}