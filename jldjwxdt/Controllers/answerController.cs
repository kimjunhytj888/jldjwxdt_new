using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using System.Web.WebPages.Html;
using System.Web.Mvc.Html;
using jldjwxdt.Models;



namespace jldjwxdt.Controllers
{
    //[CheckLogin(true)]
    public class answerController : Controller
    {
        static int qdseq = 0;
        static int QAcnt = 0;
        static int pid = 0;
        static string strdt = DateTime.Now.ToShortDateString(); //系统时间
        HttpCookie UserCookie = new HttpCookie("jldjwxdt");
       
        //static string usr_id = UserCookie["username"].ToString();
            //"admin";// 读取login后cook 用户信息 

        // GET: answer
        //[CheckLogin(false)]
        public ActionResult Index()
        {
            return Content("请从正确入口访问程序！");
        }

        //[HttpPost]
        //[ActionName("real")]
        //开始答题前 
        /// <summary>
        /// [CheckLogin] 检查是否登陆 没登陆跳转到登陆页面
        /// </summary>
        /// <returns></returns>
         
        public ActionResult Real()
        {
            string UserName = string.Empty;
            if (Request.Cookies["userName"] != null)
            {
                UserName = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }
            
            if (string.IsNullOrEmpty(UserName))
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "answer" , ReturnAction ="Real"});
            }

                pid = Process.GetCurrentProcess().Id; //线程id

            //当前用户今天答了几次题
            string sql_par = "  user_id = '" + UserName + "' and q_dt = '" + strdt + "'";
            qdseq = DbHelperSQL.GetMaxID("q_seq", "u_question_hst_log", sql_par); //需要修改成历史表 测试时是log表

            //读取配置文件 1.总题目数量
            string sql_totcnt = "select config from b_minor where major_cd = 'A3' and minor_cd = 'S'";
            QAcnt = int.Parse(DbHelperSQL.GetSingle(sql_totcnt).ToString()); //题目总数

            ViewData["username"] = UserName;
            ViewData["qdseq"] = qdseq; //今天第几次答题

            //封装随机抽取的N道题
            DataSet ask_temp_Data_hd = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.Append("EXEC usp_real_question_by_user ");
            sb.Append("@user = ").Append(pid);
            ask_temp_Data_hd = DbHelperSQL.Query(sb.ToString());
            List<QAskTempByPidHdr> LQATBHdr = new List<QAskTempByPidHdr>();

            for (int i = 0; i < ask_temp_Data_hd.Tables[0].Rows.Count; i++)
            {
                QAskTempByPidHdr QATBHdr = new QAskTempByPidHdr();

                QATBHdr.Spid = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["pid"].ToString());
                QATBHdr.Sqid = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["q_id"].ToString());
                QATBHdr.Sqseq = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["q_seq"].ToString());
                QATBHdr.q_nm = ask_temp_Data_hd.Tables[0].Rows[i]["q_nm"].ToString();
                QATBHdr.q_key = ask_temp_Data_hd.Tables[0].Rows[i]["q_key"].ToString();
                QATBHdr.q_type = ask_temp_Data_hd.Tables[0].Rows[i]["q_type"].ToString();
                QATBHdr.q_rmk = ask_temp_Data_hd.Tables[0].Rows[i]["q_rmk"].ToString();
                LQATBHdr.Add(QATBHdr);

            }
            Session["pidlist"] = LQATBHdr;//封装到session中


            return View(); //view中 开始答题时 默认从第一题 seq = 0开始

        }

        [HttpPost]
        [ActionName("real_ask")]
        //开始答题 循环答题内容 读取答题选择结果
        public ActionResult Real_ask(FormCollection collection)
        {
            HttpCookie UserCookie = new HttpCookie("jldjwxdt");

            string UserName = HttpUtility.UrlDecode(UserCookie["UserName"]);
            if (string.IsNullOrEmpty(UserName))
            {
                return View("~/login/index", "~/answer/real");
            }

            int AskSeq = int.Parse(Request.QueryString["id"]) ; //题目index
            int AskSeq1 = 0;
            if (AskSeq > 0)
            {

                 AskSeq1 = AskSeq - 1;
            }

            List<QAskTempByPidHdr> LQATBHdr = new List<QAskTempByPidHdr>();
            LQATBHdr = (List<QAskTempByPidHdr>)Session["pidlist"];
            int HQseq = 0;
            if (AskSeq < QAcnt)
            {
                ViewData["VQATBHdr1"] = LQATBHdr[AskSeq].Sqid; //题目的列表 题目id
                ViewData["VQATBHdr2"] = LQATBHdr[AskSeq].q_nm; //题目的列表 题目描述
                ViewData["VQATBHdr3"] = LQATBHdr[AskSeq].q_type; //题目的列表 题目类型
                ViewData["VQATBHdr4"] = LQATBHdr[AskSeq].q_key; //题目的列表 答案 需隐藏？
                ViewData["VQATBHdr5"] = LQATBHdr[AskSeq].q_rmk; //题目的列表 正解
                HQseq = LQATBHdr[AskSeq].Sqid; //题号
           

            


            DataSet QAskDtl = new DataSet();
            StringBuilder sbdtl = new StringBuilder();
            sbdtl.Append("EXEC usp_real_q_dtl ");
            sbdtl.Append("@q_id = ").Append(HQseq);
            List<QAskTempByPidDtl> LQATBDtl = new List<QAskTempByPidDtl>();

            QAskDtl = DbHelperSQL.Query(sbdtl.ToString());
            Helps.dts2l dts2L = new Helps.dts2l();

            LQATBDtl = dts2L.DataSetToList<QAskTempByPidDtl>(QAskDtl, 0);
            ViewData["VQATBDtl"] = LQATBDtl;
            if (AskSeq > 0)
            {
                string chk = Request["chk"].ToString();
                chk = chk.Replace("false", "");
                chk = chk.Replace(",", "");


                // Adtlexec(LQATBDtl);
                int askseqck = AskSeq - 1;

                Ahdrexec(LQATBHdr, AskSeq, chk,UserName); //insert 到历史表中 
            }
            
            }
            if (AskSeq < QAcnt)
            {
                AskSeq = AskSeq + 1;
                ViewData["AskSeq"] = AskSeq;
                return View();
            }
            else
            {

                string chk = Request["chk"].ToString();
                chk = chk.Replace("false", "");
                chk = chk.Replace(",", "");


             

                Ahdrexec(LQATBHdr, AskSeq, chk,UserName); //insert 到历史表中 

                return RedirectToAction("ask_end");

                //return View("ask_end");
            }

        }

       

        public ActionResult ask_end()
        {

            HttpCookie UserCookie = new HttpCookie("jldjwxdt");

            string UserName = HttpUtility.UrlDecode(UserCookie["UserName"]);
            if (string.IsNullOrEmpty(UserName))
            {
                return View("~/login/index", "~/answer/real");
            }

            DataSet AskEnd = new DataSet();
            StringBuilder AskEndSql = new StringBuilder();
            AskEndSql.Append("exec usp_ask_end_by_pid ");
            AskEndSql.Append("@pid= ").Append(pid).Append(",");
            AskEndSql.Append("@user_id = '").Append(UserName).Append("',");
            AskEndSql.Append("@q_dt = '").Append(strdt).Append("',");
            AskEndSql.Append("@q_seq = ").Append(qdseq);

            AskEnd = DbHelperSQL.Query(AskEndSql.ToString());

            List<QAskEndByPidReal> Lqend = new List<QAskEndByPidReal>();

            for (int i = 0; i < AskEnd.Tables[0].Rows.Count; i++)
            {
                QAskEndByPidReal QAend = new QAskEndByPidReal();


                QAend.eq_sub_seq = int.Parse(AskEnd.Tables[0].Rows[i]["q_sub_seq"].ToString());
                QAend.eq_nm = AskEnd.Tables[0].Rows[i]["q_nm"].ToString();
                QAend.eq_key = AskEnd.Tables[0].Rows[i]["q_key"].ToString();
                QAend.eu_key = AskEnd.Tables[0].Rows[i]["k_val"].ToString();
                QAend.eq_rmk = AskEnd.Tables[0].Rows[i]["q_rmk"].ToString();
                Lqend.Add(QAend);

            }
            ViewData["QAend"] = Lqend;//==ViewData中

            return View();
        }

            private void Ahdrexec(List<QAskTempByPidHdr> lQATBHdr, int AskSeq, string chk,string UserName)
        {

            //HttpCookie UserCookie = new HttpCookie("jldjwxdt");

            //string UserName = HttpUtility.UrlDecode(UserCookie["UserName"]);
            //if (string.IsNullOrEmpty(UserName))
            //{
            //    return View("login/index", "answer/real");
            //}

            QAskTempByPidHdr qAskTempByPidHdr = new QAskTempByPidHdr();
            foreach (QAskTempByPidHdr s in lQATBHdr)
            {
                if (s.Sqseq == AskSeq)
                {
                    StringBuilder Ahdrexec = new StringBuilder();
                    Ahdrexec.Append("insert into  u_question_hst_log " +
                        "(    pid,    user_id,    q_dt,    q_seq,    q_sub_seq,    q_id,    k_val,    q_rmk,    isrt_dt) VALUES ( ");
                    Ahdrexec.Append("'").Append(s.Spid).Append("',");
                    Ahdrexec.Append("'").Append(UserName).Append("',");
                    Ahdrexec.Append("'").Append(strdt).Append("',");
                    Ahdrexec.Append("'").Append(qdseq).Append("',");
                    Ahdrexec.Append("'").Append(AskSeq).Append("',");
                    Ahdrexec.Append("'").Append(s.Sqid).Append("',");
                    Ahdrexec.Append("'").Append(chk).Append("',");
                    Ahdrexec.Append("'").Append(s.q_rmk).Append("',");
                    Ahdrexec.Append("'").Append(strdt).Append("')");

                    int trn = DbHelperSQL.ExecuteSql(Ahdrexec.ToString());
                    if (trn == 0)
                    {
                        Content("Error Ahdrexec");
                    }
                }




            }

        }

        private void Adtlexec(List<QAskTempByPidDtl> LQATBDtl)
        {
            throw new NotImplementedException();
        }
    }


}