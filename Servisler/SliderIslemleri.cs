using B2b_Api.Models;
using SQL_Genel_Islemleri;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace B2b_Api.Servisler
{
    public class SliderIslemleri
    {
        public SqlConnection si_baglanti = null;
        public SqlTransaction si_transaction = null;
        public int si_komutCalismaSuresi = 30;
        public SliderBilgileri sb = new SliderBilgileri();
        string hataLogDosyasiAdi = "";
        public string hataMesaji = "";
        public SliderIslemleri(SqlConnection baglanti, SqlTransaction transaction = null, int calismaSuresi = 30, string hataDosyasi = "")
        {
            si_baglanti = baglanti;
            si_komutCalismaSuresi = calismaSuresi;
            si_transaction = transaction;
            hataLogDosyasiAdi = hataDosyasi;
        }
        public DataTable SliderOku(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT Id, Aktif, SiraNo, LinkAktif, Link From Slider";
                komutstr += " " + eksorgu;
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(si_baglanti, si_transaction, si_komutCalismaSuresi, hataLogDosyasiAdi);
                DataTable tablo = new DataTable();
                if (si_transaction == null)
                {
                    tablo = asi.Sorgu_Adaptor(komutstr, hatagoster, "SliderIslemleri : SliderOku"); 
                    hataMesaji = asi.hataMesaji; 
                }
                else
                { 
                    tablo = asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "SliderIslemleri : SliderOku");
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

        public bool SliderYaz(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"INSERT INTO Slider(Aktif, SiraNo, LinkAktif, Link) values (@Aktif, @SiraNo, @LinkAktif, @Link)";
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@Aktif", sb.aktif);
                komut.Parameters.AddWithValue("@SiraNo", sb.sirano);
                komut.Parameters.AddWithValue("@LinkAktif", sb.linkaktif);
                komut.Parameters.AddWithValue("@Link", sb.link);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(si_baglanti, si_transaction, si_komutCalismaSuresi, hataLogDosyasiAdi);
                bool sonuc = false;
                if (si_transaction == null)
                {
                    sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "SliderIslemleri : SliderYaz");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "SliderIslemleri : SliderYaz");
                    hataMesaji = asi.hataMesaji;
                }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public bool SliderDuzenle(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"UPDATE Slider SET Aktif = @Aktif, SiraNo = @SiraNo, LinkAktif = @LinkAktif, Link = @Link";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@Aktif", sb.aktif);
                komut.Parameters.AddWithValue("@SiraNo", sb.sirano);
                komut.Parameters.AddWithValue("@LinkAktif", sb.linkaktif);
                komut.Parameters.AddWithValue("@Link", sb.link);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(si_baglanti, si_transaction, si_komutCalismaSuresi, hataLogDosyasiAdi);
                bool sonuc = false;
                if (si_transaction == null)
                {
                    sonuc =  asi.Komut_ExecuteNonQuery(komut, hatagoster, "SliderIslemleri : SliderDuzenle");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    sonuc =  asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "SliderIslemleri : SliderDuzenle");
                    hataMesaji = asi.hataMesaji;
                }
                return sonuc;               
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
        public bool SliderSil(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"DELETE FROM Slider";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(si_baglanti, si_transaction, si_komutCalismaSuresi, hataLogDosyasiAdi);
                bool sonuc = false;
                if (si_transaction == null)
                {
                    sonuc = asi.Komut_ExecuteNonQuery(komut, hatagoster, "SliderIslemleri : SliderSil");
                    hataMesaji = asi.hataMesaji;
                }
                else
                {
                    sonuc = asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "SliderIslemleri : SliderSil");
                    hataMesaji = asi.hataMesaji;
                }
                return sonuc;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
        }
    }

}