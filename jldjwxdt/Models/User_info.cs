using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jldjwxdt.Models
{
    public class User_info
    {
        public string usr_id { get; set; }
        public string usr_type { get; set; }
        public int login_cnt { get; set; }
        public int dt_cnt { get; set; }
        public int pingjunfen { get; set; }
        public int all_fen { get; set; }
        public int paihang { get; set; }
        public int dtfen { get; set; }
    }
}