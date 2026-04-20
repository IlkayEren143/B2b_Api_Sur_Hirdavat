using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    public class MailEvrak
    {


        public string gondericiMail { get; set; }
        public string hostAdi { get; set; }
        public string mailSifre { get; set; }
        public string hedefMail { get; set; }
        public string[] hedefCCMail { get; set; }
        public string ekMail1 { get; set; }
        public string ekMail2 { get; set; }
        public string mailBody { get; set; }
        public Attachment attachment { get; set; }
        public bool ssl { get; set; }
        public int portNo { get; set; }
        public string attachmentUri { get; set; }
        public string baslik { get; set; }
        public bool isHTML { get; set; }
    }
}