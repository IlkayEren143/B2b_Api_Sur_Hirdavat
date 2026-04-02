using B2b_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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
                ckb.bakiye = Convert.ToDecimal(tabloCari.Rows[i]["CARBAKIYE"]);
                ckb.cariKodu = tabloCari.Rows[i]["CARKOD"].ToString();
                ckb.cariUnvani = tabloCari.Rows[i]["CARUNVAN"].ToString();
                ckb.iskonto = Convert.ToDecimal(tabloCari.Rows[i]["CARISKYUZ"]);
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
    }
}