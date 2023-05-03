using SQLite;
using System;

namespace eNota
{
    public class tbl_settings
    {
        [PrimaryKey]
        public long intID { get; set; }
        public string strName { get; set; }
        public string strAddress { get; set; }
        public string strCity { get; set; }
        public string strTelephone { get; set; }
        public string strDevice { get; set; }
    }

    public class tbl_nota
    {
        [PrimaryKey, AutoIncrement]
        public long intID { get; set; }
        public DateTime dtOrder {get; set; }
        public string strPayment { get; set; }
        public string strMode { get; set; }
        public string strStatus { get; set; }
        public string strBarang { get; set; }
        public string strIMEI { get; set; }
        public string strHarga { get; set; }
        public string strModal { get; set; }
        public string strTelephone { get; set; }
        public string strGaransiToko { get; set; }
        public string strGaransiResmi { get; set; }
    }
}
