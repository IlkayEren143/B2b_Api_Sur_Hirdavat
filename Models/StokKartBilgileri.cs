using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class StokKartBilgileri
    {
        internal object fiyatNo;

        public string stokKodu { get; set; }
        public string stokCinsi { get; set; }
        public string stokCinsi2 { get; set; }
        public string stokCinsi3 { get; set; }
        public string birim { get; set; }
        public decimal fiyat { get; set; }
        public string dovizKodu { get; set; }
        public string dovizTuru { get; set; }
        public decimal bakiye { get; set; }
        public decimal kdvOrani { get; set; }
        public string ozelKod1 { get; set; }
        public string ozelKod2 { get; set; }
        public string ozelKod3 { get; set; }
        public string ozelKod4 { get; set; }
        public string ozelKod5 { get; set; }
        public string aciklama1 { get; set; }
        public string aciklama2 { get; set; }
        public string aciklama3 { get; set; }
        public string aciklama4 { get; set; }
        public string aciklama5 { get; set; }
        public string resimBase64 { get; set; }
        public decimal kalemIndirim1 { get; set; }
        public decimal kalemIndirim2 { get; set; }
        public decimal kalemIndirim3 { get; set; }
        public decimal kalemIndirim4 { get; set; }
        public decimal kalemIndirim5 { get; set; }
        public decimal netFiyat { get; set; }
        public string barkod { get; set; }
        public string grupKodu { get; set; }
        public decimal kur { get; set; }
    }
    public class StokFiltreBilgileri
    {
        public string stokKodu { get; set; }
        public string stokCinsi { get; set; }
        public string stokCinsi2 { get; set; }
        public string stokCinsi3 { get; set; }
        public string barkod { get; set; }
        public string ozelKod1 { get; set; }
        public string ozelKod2 { get; set; }
        public string ozelKod3 { get; set; }
        public string ozelKod4 { get; set; }
        public string ozelKod5 { get; set; }
        public string aciklama1 { get; set; }
        public string aciklama2 { get; set; }
        public string aciklama3 { get; set; }
        public string aciklama4 { get; set; }
        public string aciklama5 { get; set; }
        public string grupKodu { get; set; }
    }
    public class ListedenStokFiyatBilgileri
    {
        public string stokKodu { get; set; }
        public int fiyatNo { get; set; }
        public decimal fiyat { get; set; }
        public decimal kalemIndirimYuzde1 { get; set; }
        public decimal kalemIndirimYuzde2 { get; set; }
        public decimal kalemIndirimYuzde3 { get; set; }
        public decimal kalemIndirimYuzde4 { get; set; }
        public decimal kalemIndirimYuzde5 { get; set; }
        public string dovizKodu { get; set; }
        public string dovizTuru { get; set; }
        public decimal netFiyat { get; set; }
    }
}