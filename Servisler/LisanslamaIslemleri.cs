using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SQL_Genel_Islemleri;

namespace B2b_Api.Servisler
{
    public class LisanslamaBilgileri
    {
        public int lisansid { get; set; }
        public int projeno { get; set; }
        public DateTime baslangictarihi { get; set; }
        public DateTime bitistarihi { get; set; }
        public string macadresi { get; set; }
        public string etaserino { get; set; }
        public string etkinlestirmekodu { get; set; }
        public string parametre1 { get; set; }
        public string parametre2 { get; set; }
        public string parametre3 { get; set; }

        public LisanslamaBilgileri()
        {
            lisansid = 0;
            projeno = 0;
            baslangictarihi = Convert.ToDateTime("01.01.1900");
            bitistarihi = Convert.ToDateTime("01.01.1900");
            macadresi = "";
            etaserino = "";
            etkinlestirmekodu = "";
            parametre1 = "";
            parametre2 = "";
            parametre3 = "";
        }
    }

    public class LisanslamaIslemleri
    {
        public SqlConnection li_baglanti = null;
        public SqlTransaction li_transaction = null;
        public int li_komutCalismaSuresi = 30;
        public LisanslamaBilgileri lb = new LisanslamaBilgileri();
        string hataLogDosyasiAdi = "";
        public LisanslamaIslemleri(SqlConnection baglanti, SqlTransaction transaction = null, int calismaSuresi = 30, string hataDosyasi = "")
        {
            li_baglanti = baglanti;
            li_komutCalismaSuresi = calismaSuresi;
            li_transaction = transaction;
            hataLogDosyasiAdi = hataDosyasi;
        }
        public DataTable LisanslamaOku(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"SELECT LisansID, ProjeNo, BaslangicTarihi, BitisTarihi, MacAdresi, ETASeriNo, EtkinlestirmeKodu, Parametre1, Parametre2, Parametre3 From LisanslamaListesi";
                komutstr += " " + eksorgu;
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(li_baglanti, li_transaction, li_komutCalismaSuresi, hataLogDosyasiAdi);
                if (li_transaction == null)
                    return asi.Sorgu_Adaptor(komutstr, hatagoster, "LisanslamaIslemleri : LisanslamaOku");
                else
                    return asi.Sorgu_Adaptor_Transaction(komutstr, hatagoster, "LisanslamaIslemleri : LisanslamaOku");
            }
            catch (Exception ex)
            {
                //HataLogIslemleri.HataLoglariniKaydet hlk = new HataLogIslemleri.HataLoglariniKaydet(hataLogDosyasiAdi);
                //hlk.HataBilgileriniKaydet(ex.Message, "LisanslamaIslemleri: LisanslamaOku");
                return null;
            }
        }

        public bool LisanslamaYaz(bool hatagoster = false)
        {
            try
            {
                string komutstr = @"INSERT INTO LisanslamaListesi(ProjeNo, BaslangicTarihi, BitisTarihi, MacAdresi, ETASeriNo, EtkinlestirmeKodu, Parametre1, Parametre2, Parametre3)
                                        values (@ProjeNo, @BaslangicTarihi, @BitisTarihi, @MacAdresi, @ETASeriNo, @EtkinlestirmeKodu, @Parametre1, @Parametre2, @Parametre3)";
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@ProjeNo", lb.projeno);
                komut.Parameters.AddWithValue("@BaslangicTarihi", lb.baslangictarihi);
                komut.Parameters.AddWithValue("@BitisTarihi", lb.bitistarihi);
                komut.Parameters.AddWithValue("@MacAdresi", lb.macadresi);
                komut.Parameters.AddWithValue("@ETASeriNo", lb.etaserino);
                komut.Parameters.AddWithValue("@EtkinlestirmeKodu", lb.etkinlestirmekodu);
                komut.Parameters.AddWithValue("@Parametre1", lb.parametre1);
                komut.Parameters.AddWithValue("@Parametre2", lb.parametre2);
                komut.Parameters.AddWithValue("@Parametre3", lb.parametre3);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(li_baglanti, li_transaction, li_komutCalismaSuresi, hataLogDosyasiAdi);
                if (li_transaction == null)
                    return asi.Komut_ExecuteNonQuery(komut, hatagoster, "LisanslamaIslemleri : LisanslamaYaz");
                else
                    return asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "LisanslamaIslemleri : LisanslamaYaz");
            }
            catch (Exception ex)
            {
                //HataLogIslemleri.HataLoglariniKaydet hlk = new HataLogIslemleri.HataLoglariniKaydet(hataLogDosyasiAdi);
                //hlk.HataBilgileriniKaydet(ex.Message, "LisanslamaIslemleri : LisanslamaYaz");
                return false;
            }
        }
        public bool LisanslamaDuzenle(string eksorgu = "", bool hatagoster = false)
        {
            try
            {
                string komutstr = @"UPDATE LisanslamaListesi SET  ProjeNo = @ProjeNo, BaslangicTarihi = @BaslangicTarihi, BitisTarihi = @BitisTarihi,
                                    MacAdresi = @MacAdresi, ETASeriNo = @ETASeriNo, EtkinlestirmeKodu = @EtkinlestirmeKodu, Parametre1 = @Parametre1, Parametre2 = @Parametre2, Parametre3 = @Parametre3";
                komutstr += " " + eksorgu;
                SqlCommand komut = new SqlCommand(komutstr);
                komut.Parameters.AddWithValue("@ProjeNo", lb.projeno);
                komut.Parameters.AddWithValue("@BaslangicTarihi", lb.baslangictarihi);
                komut.Parameters.AddWithValue("@BitisTarihi", lb.bitistarihi);
                komut.Parameters.AddWithValue("@MacAdresi", lb.macadresi);
                komut.Parameters.AddWithValue("@ETASeriNo", lb.etaserino);
                komut.Parameters.AddWithValue("@EtkinlestirmeKodu", lb.etkinlestirmekodu);
                komut.Parameters.AddWithValue("@Parametre1", lb.parametre1);
                komut.Parameters.AddWithValue("@Parametre2", lb.parametre2);
                komut.Parameters.AddWithValue("@Parametre3", lb.parametre3);
                Ana_SQL_Islemleri asi = new Ana_SQL_Islemleri(li_baglanti, li_transaction, li_komutCalismaSuresi, hataLogDosyasiAdi);
                if (li_transaction == null)
                    return asi.Komut_ExecuteNonQuery(komut, hatagoster, "LisanslamaIslemleri : LisanslamaDuzenle");
                else
                    return asi.Komut_ExecuteNonQuery_Transaction(komut, hatagoster, "LisanslamaIslemleri : LisanslamaDuzenle");
            }
            catch (Exception ex)
            {
                //HataLogIslemleri.HataLoglariniKaydet hlk = new HataLogIslemleri.HataLoglariniKaydet(hataLogDosyasiAdi);
                //hlk.HataBilgileriniKaydet(ex.Message, "LisanslamaIslemleri : LisanslamaDuzenle");
                return false;
            }
        }
    }
}
