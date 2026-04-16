using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class SayfalamaBilgileri
    {
        public int gecerliSayfaNo { get; set; }
        public int sayfaUzunlugu { get; set; }
        public int siralamaTipiFlag { get; set; }// 0-Stok koduna göre 1-Cinse göre a->z 2-Cinse göre z->a 3-Fiyata göre artan 4-Fiyata göre azalan (Stok için)
                                                 // 0-Cari koduna göre 1-Ünvana göre a->z 2-Ünvana göre z->a 3-Bakiyeye göre artan 4-Bakiyeye göre azalan (Cari için)
                                                 //0-Cari koduna göre 1-Ünvana göre a->z 2-Ünvana göre z->a 3-TeklifNo'ya göre a->z 4-TeklifNo'ya göre z->a 5-Tarihe göre artan 6-Tarihe göre azalan (Teklif için)
        public int aramaTipiFlag { get; set; } //0: eşit 1: başlayan 2: biten 3 içinde
        public string ekSorgu { get; set; } //Buradaki veri dolu ise diğer veri sorgulamadaki veriler gözardı edilecek. 
        public object veriSorgulama { get; set; }


    }


}