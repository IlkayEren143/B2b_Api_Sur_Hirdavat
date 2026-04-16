using B2b_Api.Models;
using SQL_Genel_Islemleri;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Util;
using static B2b_Api.Models.Evrak;
using Sonuc = B2b_Api.Models.Sonuc;


namespace B2b_Api.Servisler
{
    public class TeklifFisIslemleri
    {
        public SqlConnection tfi_baglanti = null;
        public SqlTransaction tfi_transaction = null;
        public int tfi_komutCalismaSuresi = 30;
        public TeklifFisBilgileri tfb = new TeklifFisBilgileri();
        public string hataMesaji = "";
        public TeklifFisIslemleri(SqlConnection baglanti, SqlTransaction transaction = null, int calismaSuresi = 30, string hataDosyasi = "")
        {
            tfi_baglanti = baglanti;
            tfi_komutCalismaSuresi = calismaSuresi;
            tfi_transaction = transaction;
        }
        public DataTable TeklifFisOku(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT id, FisTipi, KDVFlag, ETAKayitDurum, Tarih, ETAKayitTarih, MalToplam, IskontoYuzde1, IskontoYuzde2, IskontoToplam, AraToplam, KDVToplam, GenelToplam, Kur, TeklifNo, CariKodu, CariUnvani, Yetkili, Adres1, Adres2, DovizKodu, DovizTuru, ETASirketAdi, Aciklama1, Aciklama2, Aciklama3, OzelKod1, OzelKod2, OzelKod3, Adres3, Ilce, Il, Ulke, VergiDairesi, VergiNumarasi From TeklifFis";
                komutstr += " " + eksorgu;
                DataTable tablo = new DataTable();
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                if (tfi_transaction == null)
                {
                    tablo = asi.Sorgu_Adaptor(komutstr, hatagoster, "TeklifFisIslemleri : TeklifFisOku");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    tablo = asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "TeklifFisIslemleri : TeklifFisOku");
                    hataMesaji = asi.hataMesaji;
                }
                return tablo;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
        }
        public DataTable TeklifMaxIDAl(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT MAX(id) FROM TeklifFis";
                komutstr += " " + eksorgu;
                DataTable tablo = new DataTable();
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                if (tfi_transaction == null)
                {
                    tablo = asi.Sorgu_Adaptor(komutstr, hatagoster, "TeklifFisIslemleri : TeklifFisOku");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    tablo = asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "TeklifFisIslemleri : TeklifFisOku");
                    hataMesaji = asi.hataMesaji;
                }
                return tablo;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
        }
        public string TeklifMaxTeklifNoAl(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT MAX(TeklifNo) FROM TeklifFis";
                komutstr += " " + eksorgu;
                DataTable tablo = new DataTable();
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                if (tfi_transaction == null)
                {
                    tablo = asi.Sorgu_Adaptor(komutstr, hatagoster, "TeklifFisIslemleri : TeklifMaxTeklifNoAl");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    tablo = asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "TeklifFisIslemleri : TeklifMaxTeklifNoAl");
                    hataMesaji = asi.hataMesaji;
                }
                if (tablo == null || tablo.Rows.Count == 0 || string.IsNullOrEmpty(tablo.Rows[0][0].ToString()))
                    return "0";
                else
                    return tablo.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return "0";
            }
        }
        public bool TeklifFisYaz(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"INSERT INTO TeklifFis(FisTipi, KDVFlag, ETAKayitDurum, Tarih, ETAKayitTarih, MalToplam, IskontoYuzde1, IskontoYuzde2, IskontoToplam, AraToplam, KDVToplam, GenelToplam, Kur, TeklifNo, CariKodu, CariUnvani, Yetkili, Adres1, Adres2, DovizKodu, DovizTuru, ETASirketAdi, Aciklama1, Aciklama2, Aciklama3, OzelKod1, OzelKod2, OzelKod3, Adres3, Ilce, Il, Ulke, VergiDairesi, VergiNumarasi) values (@FisTipi, @KDVFlag, @ETAKayitDurum, @Tarih, @ETAKayitTarih, @MalToplam, @IskontoYuzde1, @IskontoYuzde2, @IskontoToplam, @AraToplam, @KDVToplam, @GenelToplam, @Kur, @TeklifNo, @CariKodu, @CariUnvani, @Yetkili, @Adres1, @Adres2, @DovizKodu, @DovizTuru, @ETASirketAdi, @Aciklama1, @Aciklama2, @Aciklama3, @OzelKod1, @OzelKod2, @OzelKod3, @Adres3, @Ilce, @Il, @Ulke, @VergiDairesi, @VergiNumarasi)";
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@FisTipi", tfb.fistipi);
                komut.Parameters.AddWithValue("@KDVFlag", tfb.kdvflag);
                komut.Parameters.AddWithValue("@ETAKayitDurum", tfb.etakayitdurum);
                komut.Parameters.AddWithValue("@Tarih", tfb.tarih);
                komut.Parameters.AddWithValue("@ETAKayitTarih", tfb.etakayittarih);
                komut.Parameters.AddWithValue("@MalToplam", tfb.maltoplam);
                komut.Parameters.AddWithValue("@IskontoYuzde1", tfb.iskontoyuzde1);
                komut.Parameters.AddWithValue("@IskontoYuzde2", tfb.iskontoyuzde2);
                komut.Parameters.AddWithValue("@IskontoToplam", tfb.iskontotoplam);
                komut.Parameters.AddWithValue("@AraToplam", tfb.aratoplam);
                komut.Parameters.AddWithValue("@KDVToplam", tfb.kdvtoplam);
                komut.Parameters.AddWithValue("@GenelToplam", tfb.geneltoplam);
                komut.Parameters.AddWithValue("@Kur", tfb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", tfb.teklifno);
                komut.Parameters.AddWithValue("@CariKodu", tfb.carikodu);
                komut.Parameters.AddWithValue("@CariUnvani", tfb.cariunvani);
                komut.Parameters.AddWithValue("@Yetkili", tfb.yetkili);
                komut.Parameters.AddWithValue("@Adres1", tfb.adres1);
                komut.Parameters.AddWithValue("@Adres2", tfb.adres2);
                komut.Parameters.AddWithValue("@DovizKodu", tfb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", tfb.dovizturu);
                komut.Parameters.AddWithValue("@ETASirketAdi", tfb.etasirketadi);
                komut.Parameters.AddWithValue("@Aciklama1", tfb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", tfb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", tfb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod1", tfb.ozelkod1);
                komut.Parameters.AddWithValue("@OzelKod2", tfb.ozelkod2);
                komut.Parameters.AddWithValue("@OzelKod3", tfb.ozelkod3);
                komut.Parameters.AddWithValue("@Adres3", tfb.adres3);
                komut.Parameters.AddWithValue("@Ilce", tfb.ilce);
                komut.Parameters.AddWithValue("@Il", tfb.il);
                komut.Parameters.AddWithValue("@Ulke", tfb.ulke);
                komut.Parameters.AddWithValue("@VergiDairesi", tfb.vergidairesi);
                komut.Parameters.AddWithValue("@VergiNumarasi", tfb.verginumarasi);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                bool sonuc = false;
                if (tfi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifFisIslemleri : TeklifFisYaz"); hataMesaji = asi.hataMesaji; }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifFisIslemleri : TeklifFisYaz"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public int TeklifFisYazveIDAl(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"
        INSERT INTO TeklifFis(FisTipi, KDVFlag, ETAKayitDurum, Tarih, ETAKayitTarih, MalToplam, IskontoYuzde1, IskontoYuzde2, IskontoToplam, AraToplam, KDVToplam, GenelToplam, Kur, TeklifNo, CariKodu, CariUnvani, Yetkili, 
            Adres1, Adres2, DovizKodu, DovizTuru, ETASirketAdi, Aciklama1, Aciklama2, Aciklama3, OzelKod1, OzelKod2, OzelKod3, Adres3, Ilce, Il, Ulke, VergiDairesi, VergiNumarasi) OUTPUT INSERTED.id
        VALUES ( @FisTipi, @KDVFlag, @ETAKayitDurum, @Tarih, @ETAKayitTarih, @MalToplam, @IskontoYuzde1, @IskontoYuzde2, @IskontoToplam, @AraToplam, @KDVToplam, @GenelToplam, @Kur, @TeklifNo, @CariKodu, @CariUnvani, @Yetkili, @Adres1, @Adres2, @DovizKodu, @DovizTuru, @ETASirketAdi, @Aciklama1, @Aciklama2, @Aciklama3, @OzelKod1, @OzelKod2, @OzelKod3, @Adres3, @Ilce, @Il, @Ulke, @VergiDairesi, @VergiNumarasi)";

                SqlCommand komut = new SqlCommand(komutstr);

                komut.Parameters.AddWithValue("@FisTipi", tfb.fistipi);
                komut.Parameters.AddWithValue("@KDVFlag", tfb.kdvflag);
                komut.Parameters.AddWithValue("@ETAKayitDurum", tfb.etakayitdurum);
                komut.Parameters.AddWithValue("@Tarih", tfb.tarih);
                komut.Parameters.AddWithValue("@ETAKayitTarih", tfb.etakayittarih);
                komut.Parameters.AddWithValue("@MalToplam", tfb.maltoplam);
                komut.Parameters.AddWithValue("@IskontoYuzde1", tfb.iskontoyuzde1);
                komut.Parameters.AddWithValue("@IskontoYuzde2", tfb.iskontoyuzde2);
                komut.Parameters.AddWithValue("@IskontoToplam", tfb.iskontotoplam);
                komut.Parameters.AddWithValue("@AraToplam", tfb.aratoplam);
                komut.Parameters.AddWithValue("@KDVToplam", tfb.kdvtoplam);
                komut.Parameters.AddWithValue("@GenelToplam", tfb.geneltoplam);
                komut.Parameters.AddWithValue("@Kur", tfb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", tfb.teklifno);
                komut.Parameters.AddWithValue("@CariKodu", tfb.carikodu);
                komut.Parameters.AddWithValue("@CariUnvani", tfb.cariunvani);
                komut.Parameters.AddWithValue("@Yetkili", tfb.yetkili);
                komut.Parameters.AddWithValue("@Adres1", tfb.adres1);
                komut.Parameters.AddWithValue("@Adres2", tfb.adres2);
                komut.Parameters.AddWithValue("@DovizKodu", tfb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", tfb.dovizturu);
                komut.Parameters.AddWithValue("@ETASirketAdi", tfb.etasirketadi);
                komut.Parameters.AddWithValue("@Aciklama1", tfb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", tfb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", tfb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod1", tfb.ozelkod1);
                komut.Parameters.AddWithValue("@OzelKod2", tfb.ozelkod2);
                komut.Parameters.AddWithValue("@OzelKod3", tfb.ozelkod3);
                komut.Parameters.AddWithValue("@Adres3", tfb.adres3);
                komut.Parameters.AddWithValue("@Ilce", tfb.ilce);
                komut.Parameters.AddWithValue("@Il", tfb.il);
                komut.Parameters.AddWithValue("@Ulke", tfb.ulke);
                komut.Parameters.AddWithValue("@VergiDairesi", tfb.vergidairesi);
                komut.Parameters.AddWithValue("@VergiNumarasi", tfb.verginumarasi);

                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);

                object result;

                if (tfi_transaction == null)
                { result = asi.Komut_ExecuteScalar_Int(komut, hatagoster, "TeklifFisIslemleri : TeklifFisYaz"); hataMesaji = asi.hataMesaji; }
                else
                { result = asi.Komut_ExecuteScalar_Int_Transaction(komut, hatagoster, "TeklifFisIslemleri : TeklifFisYaz"); hataMesaji = asi.hataMesaji; }

                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);

                return -1; // başarısız
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return -1;
            }
        }
        public bool TeklifFisDuzenle(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"UPDATE TeklifFis SET FisTipi = @FisTipi, KDVFlag = @KDVFlag, ETAKayitDurum = @ETAKayitDurum, Tarih = @Tarih, ETAKayitTarih = @ETAKayitTarih, MalToplam = @MalToplam, IskontoYuzde1 = @IskontoYuzde1, IskontoYuzde2 = @IskontoYuzde2, IskontoToplam = @IskontoToplam, AraToplam = @AraToplam, KDVToplam = @KDVToplam, GenelToplam = @GenelToplam, Kur = @Kur, TeklifNo = @TeklifNo, CariKodu = @CariKodu, CariUnvani = @CariUnvani, Yetkili = @Yetkili, Adres1 = @Adres1, Adres2 = @Adres2, DovizKodu = @DovizKodu, DovizTuru = @DovizTuru, ETASirketAdi = @ETASirketAdi, Aciklama1 = @Aciklama1, Aciklama2 = @Aciklama2, Aciklama3 = @Aciklama3, OzelKod1 = @OzelKod1, OzelKod2 = @OzelKod2, OzelKod3 = @OzelKod3, Adres3 = @Adres3, Ilce = @Ilce, Il = @Il, Ulke = @Ulke, VergiDairesi = @VergiDairesi, VergiNumarasi = @VergiNumarasi";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@FisTipi", tfb.fistipi);
                komut.Parameters.AddWithValue("@KDVFlag", tfb.kdvflag);
                komut.Parameters.AddWithValue("@ETAKayitDurum", tfb.etakayitdurum);
                komut.Parameters.AddWithValue("@Tarih", tfb.tarih);
                komut.Parameters.AddWithValue("@ETAKayitTarih", tfb.etakayittarih);
                komut.Parameters.AddWithValue("@MalToplam", tfb.maltoplam);
                komut.Parameters.AddWithValue("@IskontoYuzde1", tfb.iskontoyuzde1);
                komut.Parameters.AddWithValue("@IskontoYuzde2", tfb.iskontoyuzde2);
                komut.Parameters.AddWithValue("@IskontoToplam", tfb.iskontotoplam);
                komut.Parameters.AddWithValue("@AraToplam", tfb.aratoplam);
                komut.Parameters.AddWithValue("@KDVTutar", tfb.kdvtoplam);
                komut.Parameters.AddWithValue("@GenelToplam", tfb.geneltoplam);
                komut.Parameters.AddWithValue("@Kur", tfb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", tfb.teklifno);
                komut.Parameters.AddWithValue("@CariKodu", tfb.carikodu);
                komut.Parameters.AddWithValue("@CariUnvani", tfb.cariunvani);
                komut.Parameters.AddWithValue("@Yetkili", tfb.yetkili);
                komut.Parameters.AddWithValue("@Adres1", tfb.adres1);
                komut.Parameters.AddWithValue("@Adres2", tfb.adres2);
                komut.Parameters.AddWithValue("@DovizKodu", tfb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", tfb.dovizturu);
                komut.Parameters.AddWithValue("@ETASirketAdi", tfb.etasirketadi);
                komut.Parameters.AddWithValue("@Aciklama1", tfb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", tfb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", tfb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod1", tfb.ozelkod1);
                komut.Parameters.AddWithValue("@OzelKod2", tfb.ozelkod2);
                komut.Parameters.AddWithValue("@OzelKod3", tfb.ozelkod3);
                komut.Parameters.AddWithValue("@Adres3", tfb.adres3);
                komut.Parameters.AddWithValue("@Ilce", tfb.ilce);
                komut.Parameters.AddWithValue("@Il", tfb.il);
                komut.Parameters.AddWithValue("@Ulke", tfb.ulke);
                komut.Parameters.AddWithValue("@VergiDairesi", tfb.vergidairesi);
                komut.Parameters.AddWithValue("@VergiNumarasi", tfb.verginumarasi);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                bool sonuc = false;
                if (tfi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifFisIslemleri : TeklifFisDuzenle"); hataMesaji = asi.hataMesaji; }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifFisIslemleri : TeklifFisDuzenle"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public bool TeklifFisSil(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"DELETE FROM TeklifFis ";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(tfi_baglanti, tfi_transaction, tfi_komutCalismaSuresi);
                bool sonuc = false;
                if (tfi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifFisIslemleri : TeklifFisSil"); hataMesaji = asi.hataMesaji; }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifFisIslemleri : TeklifFisSil"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
    }

    public class TeklifHareketIslemleri
    {
        public SqlConnection thi_baglanti = null;
        public SqlTransaction thi_transaction = null;
        public int thi_komutCalismaSuresi = 30;
        public TeklifHareketBilgileri thb = new TeklifHareketBilgileri();
        public string hataMesaji = "";
        public TeklifHareketIslemleri(SqlConnection baglanti, SqlTransaction transaction = null, int calismaSuresi = 30, string hataDosyasi = "")
        {
            thi_baglanti = baglanti;
            thi_komutCalismaSuresi = calismaSuresi;
            thi_transaction = transaction;
        }
        public DataTable TeklifHareketOku(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT id, Fisid, Tarih, VadeTarihi, TerminTarihi, Miktar, Fiyat, IndirimYuzde1, IndirimYuzde2, IndirimYuzde3, IndirimYuzde4, IndirimYuzde5, IndirimToplam, KDVYuzde, KDVTutar, Tutar, Kur, TeklifNo, StokKodu, StokCinsi, Aciklama, Aciklama1, Aciklama2, Aciklama3, OzelKod, Birim, DepoKodu, DovizKodu, DovizTuru From TeklifHareket";
                komutstr += " " + eksorgu;
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(thi_baglanti, thi_transaction, thi_komutCalismaSuresi, hataMesaji);
                DataTable tablo = new DataTable();
                if (thi_transaction == null)
                { tablo = asi.Sorgu_Adaptor(komutstr, hatagoster, "TeklifHareketIslemleri : TeklifHareketOku"); hataMesaji = asi.hataMesaji; }
                else
                { tablo = asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "TeklifHareketIslemleri : TeklifHareketOku"); hataMesaji = asi.hataMesaji; }
                return tablo;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
        }

        public bool TeklifHareketYaz(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"INSERT INTO TeklifHareket(Fisid, Tarih, VadeTarihi, TerminTarihi, Miktar, Fiyat, IndirimYuzde1, IndirimYuzde2, IndirimYuzde3, IndirimYuzde4, IndirimYuzde5, IndirimToplam, KDVYuzde, KDVTutar, Tutar, Kur, TeklifNo, StokKodu, StokCinsi, Aciklama, Aciklama1, Aciklama2, Aciklama3, OzelKod, Birim, DepoKodu, DovizKodu, DovizTuru) values (@Fisid, @Tarih, @VadeTarihi, @TerminTarihi, @Miktar, @Fiyat, @IndirimYuzde1, @IndirimYuzde2, @IndirimYuzde3, @IndirimYuzde4, @IndirimYuzde5, @IndirimToplam, @KDVYuzde, @KDVTutar, @Tutar, @Kur, @TeklifNo, @StokKodu, @StokCinsi, @Aciklama, @Aciklama1, @Aciklama2, @Aciklama3, @OzelKod, @Birim, @DepoKodu, @DovizKodu, @DovizTuru)";
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@Fisid", thb.fisid);
                komut.Parameters.AddWithValue("@Tarih", thb.tarih);
                komut.Parameters.AddWithValue("@VadeTarihi", thb.vadetarihi);
                komut.Parameters.AddWithValue("@TerminTarihi", thb.termintarihi);
                komut.Parameters.AddWithValue("@Miktar", thb.miktar);
                komut.Parameters.AddWithValue("@Fiyat", thb.fiyat);
                komut.Parameters.AddWithValue("@IndirimYuzde1", thb.indirimyuzde1);
                komut.Parameters.AddWithValue("@IndirimYuzde2", thb.indirimyuzde2);
                komut.Parameters.AddWithValue("@IndirimYuzde3", thb.indirimyuzde3);
                komut.Parameters.AddWithValue("@IndirimYuzde4", thb.indirimyuzde4);
                komut.Parameters.AddWithValue("@IndirimYuzde5", thb.indirimyuzde5);
                komut.Parameters.AddWithValue("@IndirimToplam", thb.indirimtoplam);
                komut.Parameters.AddWithValue("@KDVYuzde", thb.kdvyuzde);
                komut.Parameters.AddWithValue("@KDVTutar", thb.kdvtutar);
                komut.Parameters.AddWithValue("@Tutar", thb.tutar);
                komut.Parameters.AddWithValue("@Kur", thb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", thb.teklifno);
                komut.Parameters.AddWithValue("@StokKodu", thb.stokkodu);
                komut.Parameters.AddWithValue("@StokCinsi", thb.stokcinsi);
                komut.Parameters.AddWithValue("@Aciklama", thb.aciklama);
                komut.Parameters.AddWithValue("@Aciklama1", thb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", thb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", thb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod", thb.ozelkod);
                komut.Parameters.AddWithValue("@Birim", thb.birim);
                komut.Parameters.AddWithValue("@DepoKodu", thb.depokodu);
                komut.Parameters.AddWithValue("@DovizKodu", thb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", thb.dovizturu);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(thi_baglanti, thi_transaction, thi_komutCalismaSuresi);
                bool sonuc = false;
                if (thi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketYaz"); hataMesaji = asi.hataMesaji; }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketYaz"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public int TeklifHareketYazveIDAl(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"INSERT INTO TeklifHareket(Fisid, Tarih, VadeTarihi, TerminTarihi, Miktar, Fiyat, IndirimYuzde1, IndirimYuzde2, IndirimYuzde3, IndirimYuzde4, IndirimYuzde5, IndirimToplam, KDVYuzde, KDVTutar, Tutar, Kur, TeklifNo, StokKodu, StokCinsi, Aciklama, Aciklama1, Aciklama2, Aciklama3, OzelKod, Birim, DepoKodu, DovizKodu, DovizTuru) output inserted.id values(@Fisid, @Tarih, @VadeTarihi, @TerminTarihi, @Miktar, @Fiyat, @IndirimYuzde1, @IndirimYuzde2, @IndirimYuzde3, @IndirimYuzde4, @IndirimYuzde5, @IndirimToplam, @KDVYuzde, @KDVTutar, @Tutar, @Kur, @TeklifNo, @StokKodu, @StokCinsi, @Aciklama, @Aciklama1, @Aciklama2, @Aciklama3, @OzelKod, @Birim, @DepoKodu, @DovizKodu, @DovizTuru)";
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@Fisid", thb.fisid);
                komut.Parameters.AddWithValue("@Tarih", thb.tarih);
                komut.Parameters.AddWithValue("@VadeTarihi", thb.vadetarihi);
                komut.Parameters.AddWithValue("@TerminTarihi", thb.termintarihi);
                komut.Parameters.AddWithValue("@Miktar", thb.miktar);
                komut.Parameters.AddWithValue("@Fiyat", thb.fiyat);
                komut.Parameters.AddWithValue("@IndirimYuzde1", thb.indirimyuzde1);
                komut.Parameters.AddWithValue("@IndirimYuzde2", thb.indirimyuzde2);
                komut.Parameters.AddWithValue("@IndirimYuzde3", thb.indirimyuzde3);
                komut.Parameters.AddWithValue("@IndirimYuzde4", thb.indirimyuzde4);
                komut.Parameters.AddWithValue("@IndirimYuzde5", thb.indirimyuzde5);
                komut.Parameters.AddWithValue("@IndirimToplam", thb.indirimtoplam);
                komut.Parameters.AddWithValue("@KDVYuzde", thb.kdvyuzde);
                komut.Parameters.AddWithValue("@KDVTutar", thb.kdvtutar);
                komut.Parameters.AddWithValue("@Tutar", thb.tutar);
                komut.Parameters.AddWithValue("@Kur", thb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", thb.teklifno);
                komut.Parameters.AddWithValue("@StokKodu", thb.stokkodu);
                komut.Parameters.AddWithValue("@StokCinsi", thb.stokcinsi);
                komut.Parameters.AddWithValue("@Aciklama", thb.aciklama);
                komut.Parameters.AddWithValue("@Aciklama1", thb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", thb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", thb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod", thb.ozelkod);
                komut.Parameters.AddWithValue("@Birim", thb.birim);
                komut.Parameters.AddWithValue("@DepoKodu", thb.depokodu);
                komut.Parameters.AddWithValue("@DovizKodu", thb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", thb.dovizturu);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(thi_baglanti, thi_transaction, thi_komutCalismaSuresi);
                object result;

                if (thi_transaction == null)
                { result = asi.Komut_ExecuteScalar_Int(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketYaz"); hataMesaji = asi.hataMesaji; }
                else
                { result = asi.Komut_ExecuteScalar_Int_Transaction(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketYaz"); hataMesaji = asi.hataMesaji; }

                if (result != null && result != DBNull.Value)
                    return Convert.ToInt32(result);

                return -1; // başarısız
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return -1;
            }
        }
        public bool TeklifHareketDuzenle(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"UPDATE TeklifHareket SET id = @id, Fisid = @Fisid, Tarih = @Tarih, VadeTarihi = @VadeTarihi, TerminTarihi = @TerminTarihi, Miktar = @Miktar, Fiyat = @Fiyat, IndirimYuzde1 = @IndirimYuzde1, IndirimYuzde2 = @IndirimYuzde2, IndirimYuzde3 = @IndirimYuzde3, IndirimYuzde4 = @IndirimYuzde4, IndirimYuzde5 = @IndirimYuzde5, IndirimToplam = @IndirimToplam, KDVYuzde = @KDVYuzde, KDVTutar = @KDVTutar, Tutar = @Tutar, Kur = @Kur, TeklifNo = @TeklifNo, StokKodu = @StokKodu, StokCinsi = @StokCinsi, Aciklama = @Aciklama, Aciklama1 = @Aciklama1, Aciklama2 = @Aciklama2, Aciklama3 = @Aciklama3, OzelKod = @OzelKod, Birim = @Birim, DepoKodu = @DepoKodu, DovizKodu = @DovizKodu, DovizTuru = @DovizTuru";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@id", thb.id);
                komut.Parameters.AddWithValue("@Fisid", thb.fisid);
                komut.Parameters.AddWithValue("@Tarih", thb.tarih);
                komut.Parameters.AddWithValue("@VadeTarihi", thb.vadetarihi);
                komut.Parameters.AddWithValue("@TerminTarihi", thb.termintarihi);
                komut.Parameters.AddWithValue("@Miktar", thb.miktar);
                komut.Parameters.AddWithValue("@Fiyat", thb.fiyat);
                komut.Parameters.AddWithValue("@IndirimYuzde1", thb.indirimyuzde1);
                komut.Parameters.AddWithValue("@IndirimYuzde2", thb.indirimyuzde2);
                komut.Parameters.AddWithValue("@IndirimYuzde3", thb.indirimyuzde3);
                komut.Parameters.AddWithValue("@IndirimYuzde4", thb.indirimyuzde4);
                komut.Parameters.AddWithValue("@IndirimYuzde5", thb.indirimyuzde5);
                komut.Parameters.AddWithValue("@IndirimToplam", thb.indirimtoplam);
                komut.Parameters.AddWithValue("@KDVYuzde", thb.kdvyuzde);
                komut.Parameters.AddWithValue("@KDVTutar", thb.kdvtutar);
                komut.Parameters.AddWithValue("@Tutar", thb.tutar);
                komut.Parameters.AddWithValue("@Kur", thb.kur);
                komut.Parameters.AddWithValue("@TeklifNo", thb.teklifno);
                komut.Parameters.AddWithValue("@StokKodu", thb.stokkodu);
                komut.Parameters.AddWithValue("@StokCinsi", thb.stokcinsi);
                komut.Parameters.AddWithValue("@Aciklama", thb.aciklama);
                komut.Parameters.AddWithValue("@Aciklama1", thb.aciklama1);
                komut.Parameters.AddWithValue("@Aciklama2", thb.aciklama2);
                komut.Parameters.AddWithValue("@Aciklama3", thb.aciklama3);
                komut.Parameters.AddWithValue("@OzelKod", thb.ozelkod);
                komut.Parameters.AddWithValue("@Birim", thb.birim);
                komut.Parameters.AddWithValue("@DepoKodu", thb.depokodu);
                komut.Parameters.AddWithValue("@DovizKodu", thb.dovizkodu);
                komut.Parameters.AddWithValue("@DovizTuru", thb.dovizturu);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(thi_baglanti, thi_transaction, thi_komutCalismaSuresi);
                bool sonuc = false;
                if (thi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketDuzenle"); hataMesaji = asi.hataMesaji; }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketDuzenle"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public bool TeklifHareketSil(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"DELETE FROM TeklifHareket";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(thi_baglanti, thi_transaction, thi_komutCalismaSuresi);
                bool sonuc = false;
                if (thi_transaction == null)
                { sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketDuzenle"); hataMesaji = asi.hataMesaji;   }
                else
                { sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "TeklifHareketIslemleri : TeklifHareketDuzenle"); hataMesaji = asi.hataMesaji; }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
    }

    public class TeklifIslemleri
    {

        public Sonuc TeklifFisListesiniAl(SayfalamaBilgileri sb, string eksorgu2 = " AND FisTipi > 0")
        {

            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string eksorgu = TeklifKriterleriniAl(sb);
            eksorgu += eksorgu2;
            TeklifFisIslemleri tfi = new TeklifFisIslemleri(baglanti);
            List<TeklifFisBilgileri> tfbListe = new List<TeklifFisBilgileri>();
            DataTable tablo = tfi.TeklifFisOku(eksorgu);
            if (tablo == null)
            {
                sonuc.mesaj = "Teklif tablosu okunamadı.";
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.veriOkuBasari = false;
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.mesaj = "Kriterlere uygun teklif bulunamadı.";
                sonuc.sonuc = false;
                sonuc.data = tfbListe;
                sonuc.veriOkuBasari = true;
                return sonuc;
            }
            for (int i = 0; i < tablo.Rows.Count; ++i)
            {
                TeklifFisBilgileri tfb = new TeklifFisBilgileri();
                tfb.id = Convert.ToInt32(tablo.Rows[i]["id"]);
                tfb.aciklama1 = tablo.Rows[i]["Aciklama1"].ToString();
                tfb.aciklama2 = tablo.Rows[i]["Aciklama2"].ToString();
                tfb.aciklama3 = tablo.Rows[i]["Aciklama3"].ToString();
                tfb.adres1 = tablo.Rows[i]["Adres1"].ToString();
                tfb.adres2 = tablo.Rows[i]["Adres2"].ToString();
                tfb.adres3 = tablo.Rows[i]["Adres3"].ToString();
                tfb.aratoplam = Convert.ToDecimal(tablo.Rows[i]["AraToplam"]);
                tfb.carikodu = tablo.Rows[i]["CariKodu"].ToString();
                tfb.cariunvani = tablo.Rows[i]["CariUnvani"].ToString();
                tfb.dovizkodu = tablo.Rows[i]["DovizKodu"].ToString();
                tfb.dovizturu = tablo.Rows[i]["DovizTuru"].ToString();
                tfb.etakayitdurum = Convert.ToInt32(tablo.Rows[i]["ETAKayitDurum"]);
                tfb.etakayittarih = Convert.ToDateTime(tablo.Rows[i]["ETAKayitTarih"]);
                tfb.etasirketadi = tablo.Rows[i]["ETASirketAdi"].ToString();
                tfb.fistipi = Convert.ToInt32(tablo.Rows[i]["FisTipi"]);
                tfb.geneltoplam = Convert.ToDecimal(tablo.Rows[i]["GenelToplam"]);
                tfb.il = tablo.Rows[i]["Il"].ToString();
                tfb.ilce = tablo.Rows[i]["Ilce"].ToString();
                tfb.iskontotoplam = Convert.ToDecimal(tablo.Rows[i]["IskontoToplam"]);
                tfb.iskontoyuzde1 = Convert.ToDecimal(tablo.Rows[i]["IskontoYuzde1"]);
                tfb.iskontoyuzde2 = Convert.ToDecimal(tablo.Rows[i]["IskontoYuzde2"]);
                tfb.kdvflag = Convert.ToInt32(tablo.Rows[i]["KDVFlag"]);
                tfb.kdvtoplam = Convert.ToDecimal(tablo.Rows[i]["KDVToplam"]);
                tfb.kur = Convert.ToDecimal(tablo.Rows[i]["Kur"]);
                tfb.maltoplam = Convert.ToDecimal(tablo.Rows[i]["MalToplam"]);
                tfb.ozelkod1 = tablo.Rows[i]["OzelKod1"].ToString();
                tfb.ozelkod2 = tablo.Rows[i]["OzelKod2"].ToString();
                tfb.ozelkod3 = tablo.Rows[i]["OzelKod3"].ToString();
                tfb.tarih = Convert.ToDateTime(tablo.Rows[i]["Tarih"]);
                tfb.teklifno = tablo.Rows[i]["TeklifNo"].ToString();
                tfb.ulke = tablo.Rows[i]["Ulke"].ToString();
                tfb.vergidairesi = tablo.Rows[i]["VergiDairesi"].ToString();
                tfb.verginumarasi = tablo.Rows[i]["VergiNumarasi"].ToString();
                tfbListe.Add(tfb);
            }
            int sayac = TeklifSayisiniBul(sb.ekSorgu);
            sonuc.sonuc = true;
            sonuc.mesaj = "Başarılı";
            sonuc.data = tfbListe;
            sonuc.ekData = sayac;
            sonuc.veriOkuBasari = true;
            return sonuc;
        }
        public Sonuc SepetOku(string cariKodu)
        {
            Sonuc sonuc = new Sonuc();
            SayfalamaBilgileri sb = new SayfalamaBilgileri();
            sb.ekSorgu = $"WHERE CariKodu = '{cariKodu}' AND FisTipi = 0";
            sonuc = TeklifFisListesiniAl(sb, "");
            if (!sonuc.sonuc)
            {
                return sonuc;
            }
            TeklifEvrakBilgileri evrak = new TeklifEvrakBilgileri();
            evrak.tfb = (sonuc.data as List<TeklifFisBilgileri>)[0];
            sonuc = TeklifHareketListesiniAl(evrak.tfb.teklifno);
            if (!sonuc.sonuc)
            {
                return sonuc;
            }
            evrak.thbListe = sonuc.data as List<TeklifHareketBilgileri>;
            sonuc.data = evrak;
            return sonuc;
        }
        public Sonuc TeklifHareketListesiniAl(string teklifNo)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string eksorgu = $"WHERE TeklifNo = '{teklifNo}'";
            TeklifHareketIslemleri thi = new TeklifHareketIslemleri(baglanti);
            List<TeklifHareketBilgileri> thbListe = new List<TeklifHareketBilgileri>();
            DataTable tablo = thi.TeklifHareketOku(eksorgu);
            if (tablo == null)
            {
                sonuc.mesaj = "Teklif hareket tablosu okunamadı.";
                sonuc.sonuc = false;
                sonuc.data = null;
                sonuc.veriOkuBasari = false;
                return sonuc;
            }
            if (tablo.Rows.Count == 0)
            {
                sonuc.mesaj = "Kriterlere uygun teklif hareketi bulunamadı.";
                sonuc.sonuc = false;
                sonuc.data = thbListe;
                sonuc.veriOkuBasari = true;
                return sonuc;
            }
            for (int i = 0; i < tablo.Rows.Count; ++i)
            {
                TeklifHareketBilgileri thb = new TeklifHareketBilgileri();
                thb.id = Convert.ToInt32(tablo.Rows[i]["id"]);
                thb.aciklama = tablo.Rows[i]["Aciklama"].ToString();
                thb.aciklama1 = tablo.Rows[i]["Aciklama1"].ToString();
                thb.aciklama2 = tablo.Rows[i]["Aciklama2"].ToString();
                thb.aciklama3 = tablo.Rows[i]["Aciklama3"].ToString();
                thb.birim = tablo.Rows[i]["Birim"].ToString();
                thb.depokodu = tablo.Rows[i]["DepoKodu"].ToString();
                thb.dovizkodu = tablo.Rows[i]["DovizKodu"].ToString();
                thb.dovizturu = tablo.Rows[i]["DovizTuru"].ToString();
                thb.fisid = Convert.ToInt32(tablo.Rows[i]["Fisid"]);
                thb.fiyat = Convert.ToDecimal(tablo.Rows[i]["Fiyat"]);
                thb.indirimtoplam = Convert.ToDecimal(tablo.Rows[i]["IndirimToplam"]);
                thb.indirimyuzde1 = Convert.ToDecimal(tablo.Rows[i]["IndirimYuzde1"]);
                thb.indirimyuzde2 = Convert.ToDecimal(tablo.Rows[i]["IndirimYuzde2"]);
                thb.indirimyuzde3 = Convert.ToDecimal(tablo.Rows[i]["IndirimYuzde3"]);
                thb.indirimyuzde4 = Convert.ToDecimal(tablo.Rows[i]["IndirimYuzde4"]);
                thb.indirimyuzde5 = Convert.ToDecimal(tablo.Rows[i]["IndirimYuzde5"]);
                thb.kdvtutar = Convert.ToDecimal(tablo.Rows[i]["KDVTutar"]);
                thb.kdvyuzde = Convert.ToDecimal(tablo.Rows[i]["KDVYuzde"]);
                thb.kur = Convert.ToDecimal(tablo.Rows[i]["Kur"]);
                thb.miktar = Convert.ToDecimal(tablo.Rows[i]["Miktar"]);
                thb.ozelkod = tablo.Rows[i]["OzelKod"].ToString();
                thb.stokcinsi = tablo.Rows[i]["StokCinsi"].ToString();
                thb.stokkodu = tablo.Rows[i]["StokKodu"].ToString();
                thb.tarih = Convert.ToDateTime(tablo.Rows[i]["Tarih"]);
                thb.teklifno = tablo.Rows[i]["TeklifNo"].ToString();
                thb.termintarihi = Convert.ToDateTime(tablo.Rows[i]["TerminTarihi"]);
                thb.tutar = Convert.ToDecimal(tablo.Rows[i]["Tutar"]);
                thb.vadetarihi = Convert.ToDateTime(tablo.Rows[i]["VadeTarihi"]);
                thbListe.Add(thb);
            }
            sonuc.sonuc = true;
            sonuc.mesaj = "Başarılı";
            sonuc.data = thbListe;
            sonuc.veriOkuBasari = true;
            sonuc.ekData = null;
            return sonuc;
        }
        public Sonuc TeklifEkle(TeklifEvrakBilgileri evrak)
        {
            evrak.tfb.fistipi = 1;
            return TeklifKaydet(evrak);
        }
        public Sonuc SepetEkle(TeklifEvrakBilgileri evrak)
        {
            evrak.tfb.fistipi = 0;
            return TeklifKaydet(evrak);
        }
        private Sonuc TeklifKaydet(TeklifEvrakBilgileri evrak)
        {

            Sonuc sonuc = new Sonuc();
            if (evrak.tfb.etakayitdurum > 0)
            {
                sonuc.sonuc = false;
                sonuc.mesaj = "Teklif ETA'ya kaydedilmiş. Değişiklik yapılamaz.";
                sonuc.data = null;
                sonuc.veriOkuBasari = true;
                return sonuc;
            }
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);

            if (evrak.tfb.id == 0)
            {
                TeklifFisIslemleri tfi = new TeklifFisIslemleri(baglanti);
                tfi.tfb = TekliffisBilfgileriniDuzelt(evrak.tfb);
            }
            TeklifEvrakBilgisiniDuzelt(evrak.tfb, evrak.thbListe);
            baglanti.Open();
            SqlTransaction transaction = baglanti.BeginTransaction();
            if (evrak.tfb.id > 0)
            {

                TeklifFisIslemleri tfi = new TeklifFisIslemleri(baglanti, transaction);
                tfi.tfb = evrak.tfb;
                if (!tfi.TeklifFisDuzenle($"WHERE id = {evrak.tfb.id}"))
                {
                    transaction.Rollback();
                    baglanti.Close();
                    sonuc.sonuc = false;
                    sonuc.mesaj = "Teklif kaydedilemedi." + tfi.hataMesaji;
                    sonuc.veriOkuBasari = true;
                    sonuc.data = null;
                    return sonuc;
                }
                foreach (TeklifHareketBilgileri thb in evrak.thbListe)
                {
                    TeklifHareketIslemleri thi = new TeklifHareketIslemleri(baglanti, transaction);
                    
                    thb.fisid = evrak.tfb.id;
                    thb.teklifno = evrak.tfb.teklifno;
                    thb.tarih = evrak.tfb.tarih;
                    thi.thb = thb;
                    if (thb.id > 0)
                    {
                        if (!thi.TeklifHareketDuzenle($"WHERE id = {thb.id}"))
                        {
                            transaction.Rollback();
                            baglanti.Close();
                            sonuc.sonuc = false;
                            sonuc.mesaj = "Teklif kaydedilemedi." + thi.hataMesaji;
                            sonuc.veriOkuBasari = true;
                            sonuc.data = null;
                            return sonuc;
                        }
                    }
                    else
                    {   
                        int id = thi.TeklifHareketYazveIDAl();

                        if (id < 0)
                        {
                            transaction.Rollback();
                            baglanti.Close();
                            sonuc.sonuc = false;
                            sonuc.mesaj = "Teklif kaydedilemedi." + thi.hataMesaji;
                            sonuc.veriOkuBasari = true;
                            sonuc.data = null;
                            return sonuc;
                        }
                        thb.id = id;
                    }
                }
            }
            else
            {
                TeklifFisIslemleri tfi = new TeklifFisIslemleri(baglanti, transaction);
                evrak.tfb.teklifno = (Convert.ToInt32(tfi.TeklifMaxTeklifNoAl()) + 1).ToString("D13");
                tfi.tfb = evrak.tfb;
                int id = tfi.TeklifFisYazveIDAl();
                if (id < 0)
                {
                    transaction.Rollback();
                    baglanti.Close();
                    sonuc.sonuc = false;
                    sonuc.mesaj = "Teklif kaydedilemedi." + tfi.hataMesaji;
                    sonuc.veriOkuBasari = true;
                    sonuc.data = null;
                    return sonuc;
                }
                evrak.tfb.id = id;
                
                foreach (TeklifHareketBilgileri thb in evrak.thbListe)
                {
                    TeklifHareketIslemleri thi = new TeklifHareketIslemleri(baglanti, transaction);
                    thb.fisid = evrak.tfb.id;
                    thb.teklifno = evrak.tfb.teklifno;
                    thb.tarih = evrak.tfb.tarih;
                    thi.thb = thb;
                    int fisid = thi.TeklifHareketYazveIDAl();
                    if (id < 0)
                    {
                        transaction.Rollback();
                        baglanti.Close();
                        sonuc.sonuc = false;
                        sonuc.mesaj = "Teklif kaydedilemedi." + thi.hataMesaji;
                        sonuc.veriOkuBasari = true;
                        sonuc.data = null;
                        return sonuc;
                    }
                    thb.id = fisid;
                }
            }

            transaction.Commit();
            baglanti.Close();
            sonuc.sonuc = true;
            sonuc.data = evrak;
            sonuc.veriOkuBasari = true;
           sonuc.ekData = evrak.tfb.id;
            
            return sonuc;
        }
        
        private TeklifFisBilgileri TekliffisBilfgileriniDuzelt(TeklifFisBilgileri tfb)
        {
            CariIslemler ci = new CariIslemler();
            DataRow satir = ci.CariAdresBilgileriniAl(tfb.carikodu);
            if (satir != null)
            {
                tfb.adres1 = satir["ADRADRES1"].ToString();
                tfb.adres2 = satir["ADRADRES2"].ToString();
                tfb.adres3 = satir["ADRADRES3"].ToString();                
                tfb.il = satir["ADRIL"].ToString();
                tfb.ilce = satir["ADRILCE"].ToString();
                tfb.ulke = satir["ADRULKE"].ToString();
                tfb.vergidairesi = satir["CARVERDAIRE"].ToString();
                tfb.verginumarasi= satir["CARVERHESNO"].ToString();
            }
            tfb.tarih = DateTime.Today;
            return tfb;
        }
        private void TeklifEvrakBilgisiniDuzelt(TeklifFisBilgileri tfb, List<TeklifHareketBilgileri> thbListe)
        {
            tfb.kdvflag = Convert.ToInt32(ConfigurationManager.AppSettings["KDVFlag"]);
            tfb.aratoplam = 0;
            tfb.maltoplam = 0;
            tfb.kdvtoplam = 0;
            tfb.geneltoplam = 0;
            tfb.iskontotoplam = 0;
            foreach (TeklifHareketBilgileri thb in thbListe)
            {
                decimal netFiyat = thb.fiyat;

                netFiyat *= (1 - thb.indirimyuzde1 / 100);
                netFiyat *= (1 - thb.indirimyuzde2 / 100);
                netFiyat *= (1 - thb.indirimyuzde3 / 100);
                netFiyat *= (1 - thb.indirimyuzde4 / 100);
                netFiyat *= (1 - thb.indirimyuzde5 / 100);
                netFiyat *= (1 - tfb.iskontoyuzde1 / 100);
                netFiyat *= (1 - tfb.iskontoyuzde2 / 100);
                thb.netTutar = netFiyat;
                if (tfb.kdvflag == 0)
                {
                    thb.kdvtutar = thb.netTutar * thb.kdvyuzde / 100;
                }
                else
                {
                    thb.kdvtutar = thb.netTutar - (thb.netTutar / (1 + thb.kdvyuzde / 100));
                }
                thb.tutar = thb.fiyat * thb.miktar;
                thb.netTutar = thb.netTutar * thb.miktar;
                thb.kdvtutar = thb.kdvtutar * thb.miktar;
                thb.indirimtoplam = thb.tutar - thb.netTutar;
                tfb.aratoplam += thb.netTutar;
                tfb.maltoplam += thb.tutar;
                tfb.iskontotoplam += thb.indirimtoplam;
                tfb.kdvtoplam += thb.kdvtutar;
                
            }
            tfb.geneltoplam = tfb.aratoplam + tfb.kdvtoplam;
        }

        private string TeklifKriterleriniAl(SayfalamaBilgileri sb)
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
                    TeklifFiltreBilgileri tfb = Newtonsoft.Json.JsonConvert.DeserializeObject<TeklifFiltreBilgileri>(json);
                    switch (sb.aramaTipiFlag)
                    {
                        case 0:
                            if (!string.IsNullOrEmpty(tfb.cariKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariKodu = '{tfb.cariKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariKodu = '{tfb.cariKodu}'";
                            }
                            if (!string.IsNullOrEmpty(tfb.cariUnvani))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariUnvani = '{tfb.cariUnvani}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariUnvani = '{tfb.cariUnvani}'";
                            }
                            break;
                        case 1:
                            if (!string.IsNullOrEmpty(tfb.cariKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariKodu LIKE '{tfb.cariKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariKodu LIKE '{tfb.cariKodu}%'";
                            }
                            if (!string.IsNullOrEmpty(tfb.cariUnvani))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariUnvani LIKE '{tfb.cariUnvani}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariUnvani LIKE '{tfb.cariUnvani}%'";
                            }
                            break;
                        case 2:
                            if (!string.IsNullOrEmpty(tfb.cariKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariKodu LIKE '%{tfb.cariKodu}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariKodu LIKE '%{tfb.cariKodu}'";
                            }
                            if (!string.IsNullOrEmpty(tfb.cariUnvani))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariUnvani LIKE '%{tfb.cariUnvani}'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariUnvani LIKE '%{tfb.cariUnvani}'";
                            }
                            break;
                        case 3:
                            if (!string.IsNullOrEmpty(tfb.cariKodu))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariKodu LIKE '%{tfb.cariKodu}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariKodu LIKE '%{tfb.cariKodu}%'";
                            }
                            if (!string.IsNullOrEmpty(tfb.cariUnvani))
                            {
                                if (eksorgu1 == "")
                                { eksorgu2 += $" CariUnvani LIKE '%{tfb.cariUnvani}%'"; eksorgu1 = "var"; }
                                else
                                    eksorgu2 += $" OR CariUnvani LIKE '%{tfb.cariUnvani}%'";
                            }
                            break;
                    }
                    if (!string.IsNullOrEmpty(eksorgu2))
                    {
                        eksorgu += " AND ( " + eksorgu2 + " )";
                    }
                    if (!string.IsNullOrEmpty(tfb.teklifNo))
                    {
                        eksorgu += $" AND TeklifNo = '{tfb.teklifNo}'";
                    }
                    if (!string.IsNullOrEmpty(tfb.baslangicTarihi))
                    {
                        eksorgu += $" AND Tarih >= '{tfb.baslangicTarihi}'";
                    }
                    if (!string.IsNullOrEmpty(tfb.bitisTarihi))
                    {
                        eksorgu += $" AND Tarih <= '{tfb.bitisTarihi}'";
                    }
                }
            }
            if (sb.sayfaUzunlugu > 0)
            {
                switch (sb.siralamaTipiFlag)
                {
                    case 0:
                        eksorgu += $" ORDER BY CariKodu OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 1:
                        eksorgu += $" ORDER BY CariUnvani OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 2:
                        eksorgu += $" ORDER BY CariUnvani DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 3:
                        eksorgu += $" ORDER BY TeklifNo OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 4:
                        eksorgu += $" ORDER BY TeklifNo DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 5:
                        eksorgu += $" ORDER BY Tarih OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                    case 6:
                        eksorgu += $" ORDER BY Tarih DESC OFFSET {((sb.gecerliSayfaNo - 1) * sb.sayfaUzunlugu)} ROWS FETCH NEXT {sb.sayfaUzunlugu} ROWS ONLY";
                        break;
                }
            }

            return eksorgu;
        }
        private int TeklifSayisiniBul(String ekSorgu)
        {
            Sonuc sonuc = new Sonuc();
            string baglantistr = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
            SqlConnection baglanti = new SqlConnection(baglantistr);
            string komutstr = $"SELECT COUNT(TeklifNo) FROM TeklifFis";
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
       
    }
}