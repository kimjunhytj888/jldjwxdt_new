//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace jldjwxdt.Controllers
//{
//    public class TestController : Controller
//    {


//        static int qdseq = 0;
//        static int QAcnt = 0;


//        // GET: answer
//        public ActionResult Index()
//        {
//            return Content("请从正确入口访问程序！");
//        }

//        //[HttpPost]
//        //[ActionName("real")]
//        //开始答题前 
//        public ActionResult real()
//        {

//            HttpCookie UserCookie = new HttpCookie("jldjwxdt");

//            string strdt = DateTime.Now.ToShortDateString(); //系统时间
//            string usr_id = "admin";// 读取login后cook 用户信息 


//            //当前用户今天答了几次题
//            string sql_par = "  user_id = '" + usr_id + "' and q_dt = '" + strdt + "'";
//            qdseq = DbHelperSQL.GetMaxID("q_seq", "u_question_hst", sql_par);

//            //读取配置文件 1.总题目数量
//            string sql_totcnt = "select config from b_minor where major_cd = 'A3' and minor_cd = 'S'";
//            QAcnt = int.Parse(DbHelperSQL.GetSingle(sql_totcnt).ToString());


//            // ///
//            // ///设置cookie 
//            // ///userid 用户id
//            // ///ask_dt 答题时间 = 当前时间
//            // ///ask_tot 总答题数量
//            // ///ask_cnt 当前数值
//            // UserCookie["UserId"] = usr_id; //测试时写死
//            // UserCookie["Ask_dt"] = DateTime.Now.ToShortDateString(); //"";当前时间
//            // UserCookie["Ask_seq"] = Convert.ToString(q_seq); //当天答了几次题
//            //// UserCookie["Ask_tot"] = tot_cnt; //答题总数量 从config中获取
//            // UserCookie["Ask_cnt"] = "0";
//            // UserCookie["pid"] = Convert.ToString(pid);
//            // //  UserCookie.Expires = DateTime.Now.AddDays(1);//这里设置要保存多长时间.
//            // Response.Cookies.Add(UserCookie);




//            ViewData["qdseq"] = qdseq;


//            return View();
//        }

//        [HttpPost]
//        // [ActionName("real_ask")]
//        //开始答题 循环答题内容 读取答题选择结果
//        public ActionResult Real_ask(FormCollection collection)
//        {
//            int pid = Process.GetCurrentProcess().Id; //线程id
//            DataSet ask_temp_Data_hd = new DataSet();
//            StringBuilder sb = new StringBuilder();
//            sb.Append("EXEC usp_real_question_by_user ");
//            sb.Append("@user = ").Append(pid);
//            ask_temp_Data_hd = DbHelperSQL.Query(sb.ToString());
//            List<QAskTempByPidHdr> LQATBHdr = new List<QAskTempByPidHdr>();


//            for (int i = 0; i < ask_temp_Data_hd.Tables[0].Rows.Count; i++)
//            {
//                QAskTempByPidHdr QATBHdr = new QAskTempByPidHdr();

//                QATBHdr.Spid = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["pid"].ToString());
//                QATBHdr.Sqid = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["q_id"].ToString());
//                QATBHdr.Sqseq = int.Parse(ask_temp_Data_hd.Tables[0].Rows[i]["q_seq"].ToString());
//                QATBHdr.q_nm = ask_temp_Data_hd.Tables[0].Rows[i]["q_nm"].ToString();
//                QATBHdr.q_key = ask_temp_Data_hd.Tables[0].Rows[i]["q_nm"].ToString();
//                QATBHdr.q_type = ask_temp_Data_hd.Tables[0].Rows[i]["q_nm"].ToString();
//                QATBHdr.q_rmk = ask_temp_Data_hd.Tables[0].Rows[i]["q_nm"].ToString();
//                LQATBHdr.Add(QATBHdr);

//            }

//            //  QaskHdrExec(LQATBHdr);

//            Session["pidlist"] = LQATBHdr;

//            int ppid = int.Parse(Request.QueryString["id"]);
//            if (ppid < QAcnt)
//            {

//                List<QAskTempByPidHdr> sRarr = new List<QAskTempByPidHdr>();
//                sRarr = (List<QAskTempByPidHdr>)Session["pidlist"];
//                ViewData["pask_list"] = sRarr[ppid].q_id.ToString();

//                int dtl_id = int.Parse(sRarr[ppid].q_id.ToString());

//                string q_hdnm_sql = "select q_nm from b_question_hdr where q_id = " + dtl_id;
//                string q_hd_nm = DbHelperSQL.GetSingle(q_hdnm_sql).ToString();

//                ViewData["q_hd_nm"] = q_hd_nm;


//                List<question_dtl> l_dtl = new List<question_dtl>();
//                DataSet d_dtl = new DataSet();

//                StringBuilder sbdtl = new StringBuilder();
//                sbdtl.Append("EXEC usp_real_q_dtl ");
//                sbdtl.Append("@q_id = ").Append(dtl_id);


//                d_dtl = DbHelperSQL.Query(sbdtl.ToString());
//                l_dtl.Clear();

//                for (int i = 0; i < d_dtl.Tables[0].Rows.Count; i++)
//                {
//                    question_dtl _Dtl = new question_dtl();

//                    _Dtl.question_dt_id = dtl_id;
//                    _Dtl.k_id = d_dtl.Tables[0].Rows[i]["k_id"].ToString();
//                    _Dtl.k_nm = d_dtl.Tables[0].Rows[i]["k_nm"].ToString();

//                    l_dtl.Add(_Dtl);

//                }
//                if (ppid > 0)
//                {
//                    AskDtlChk askDtl = new AskDtlChk();
//                    string chk = Request["chk"].ToString();
//                    Session.Add("askDtl", askDtl);

//                }


//                ViewData["ask_dtl"] = l_dtl;

//                ppid = ppid + 1;

//                ViewData["test"] = ppid;
//                return View();
//            }
//            else
//            {

//                string rtn = "real_ask_end";
//                return View("real_ask_end");



//            }
//        }
//        public bool QaskHdrExec(List<QAskTempByPidHdr> QAskTempByPidHdr)
//        {
//            return true;
//        }
//    }
//}