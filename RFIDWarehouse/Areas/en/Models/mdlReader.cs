using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RFIDWarehouse.Areas.en.Models
{
    public class mdlReader
    {
        public string ID { get; set; }
        public string GID { get; set; }
        public string MODEL { get; set; }
        public string READERNAME { get; set; }
        public string GATE { get; set; }
        public string PORT { get; set; }
        public string IPADDRESS { get; set; }
        public string TIME { get; set; }
        public string ACTIVE { get; set; }
        public string TAG { get; set; }
    }
}