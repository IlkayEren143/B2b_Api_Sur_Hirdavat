using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class Sonuc
    {
        public bool sonuc { get; set; }
        public bool servisUlasmaBasari { get; set; }
        public bool veriOkuBasari { get; set; }
        public object data { get; set; }
        public object ekData { get; set; }
        public string mesaj { get; set; }
    }
}