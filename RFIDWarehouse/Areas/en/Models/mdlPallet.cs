using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RFIDWarehouse.Areas.en.Models
{
    public class mdlPallet
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string BARCODE { get; set; }

        public string NAME { get; set; }
        public string RFID { get; set; }
        public string TYPE { get; set; }
        public string STYLE { get; set; }
        public string SIZE { get; set; }
        public string AREA { get; set; }
        public string LOCATION { get; set; }
        public string SUPPLIER { get; set; }
        public string ACTIVE { get; set; }
        public string READTIME { get; set; }
        public string READERNAME { get; set; }
        public string DAYS { get; set; }

        public string EMPLOYEE { get; set; }
        public string CNT { get; set; }
        
    }
}

