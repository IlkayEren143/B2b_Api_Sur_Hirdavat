using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class Evrak
    {
        public class TeklifEvrakBilgileri
        {
            public TeklifFisBilgileri tfb = new TeklifFisBilgileri();
            public List<TeklifHareketBilgileri> thbListe = new List<TeklifHareketBilgileri>();
        }
    }
}