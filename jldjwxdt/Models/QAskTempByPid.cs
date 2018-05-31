using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jldjwxdt.Models
{
    public class QAskTempByPid
    {
        public List<QAskTempByPidHdr> QAskTempByPidHdr { get; set; }
        public List<QAskTempByPidDtl> QAskTempByPidDtl { get; set; }
        public List<QAskTempByPidReal> QAskTempByPidReal { get; set; }
        public List<QAskEndByPidReal> QAskEndByPidReal { get; set; }
    }

    public class QAskEndByPidReal
    {

        //public int pid { get; set; } //session id
        //public string user_id { get; set; }//用户id
        //public string q_dt { get; set; }//答题日期
        //public int q_seq { get; set; } //答题次数
        public int eq_sub_seq { get; set; } //答题序号
        public int eqid { get; set; } //题目id
        public string eq_nm { get; set; }//题目内容
        public string eq_key { get; set; } //正确答案
        public string eu_key { get; set; } //用户答案
        public string eq_rmk { get; set; } //正解
    }

    public   class QAskTempByPidHdr
    {
        public int Spid { get; set; } //session id
        public int Sqid { get; set; } //题目id
        public int Sqseq { get; set; }//题目序号
        public string q_nm { get; set; }//题目内容
        public string q_type { get; set; }//单选多选
        public string q_key { get; set; } //答案
        public string q_rmk { get; set; } //正解
     

    }

    public  class QAskTempByPidDtl
    {
        public int Spid { get; set; } //session id
        public int Sqid { get; set; } //题目id
        public int Sqseq { get; set; }//题目序号
        public string k_id { get; set; }//选项id
        public string k_nm { get; set; }//选项名称      


    }

    public  class QAskTempByPidReal
    {
        public int Spid { get; set; } //session id
        public int Sqid { get; set; } //题目id
        public int Sqseq { get; set; }//题目序号
        public string a_id { get; set; }//回答选项
             


    }
    public static class QAskTempOption
    {
        static int QAllCnt { get; set; } //题目总数
        
    }

   
}