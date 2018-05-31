using System;
using System.Collections.Generic;
using System.Linq;

namespace jldjwxdt.Models
{
    public  class answer
    {
     

        public List<question_hd> question_hd { get; set; }

        public List<question_dtl> question_dtl { get; set; }

        public List<q_user_chk> q_user_chk { get; set; }


    }

    public class question_hd
    {
        public int Spid { get; set; } //session id
        public int Sqid { get; set; } //题目id
        public int Sqseq { get; set; }//题目序号
        public string q_nm { get; set; }//题目内容
        public string q_type { get; set; }//单选多选
        public string q_key { get; set; } //答案
        public string q_rmk { get; set; } //正解


    }

    public class question_dtl
    {
        public int question_dt_id { get; set; }
        public string k_id { get; set; }
        public string k_nm { get; set; }

    }

    public class q_user_chk
    {
        public int session_id { get; set; }
        public string ask_dt { get; set; }
        public int ask_seq { get; set; }
        public string q_id { get; set; }
    }


}