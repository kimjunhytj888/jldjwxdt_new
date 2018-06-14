using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jldjwxdt.Helps;

namespace jldjwxdt.Models
{
    public class _QaListPageParams:PagerInBase
    {
       
        public string userid { get; set; }
       
        public string dept_nm { get; set; }
       
        public DateTime q_dt { get; set; }
        
        public int q_seq { get; set; }
        
        public int q_fen { get; set; }
    }
}