using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace B2b_Api.Models
{
    public class SliderBilgileri
    {
        public int id { get; set; }
        public int aktif { get; set; }
        public int sirano { get; set; }
        public int linkaktif { get; set; }
        public string link { get; set; }


        public SliderBilgileri()
        {
            id = 0;
            aktif = 0;
            sirano = 0;
            linkaktif = 0;
            link = "";
        }
    }

}