using jldjwxdt.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jldjwxdt.Controllers
{
    public class QController : Controller
    {
        // GET: Q
        public ActionResult Index()
        {
            return Content("q");
        }

        [HttpGet]
        public ActionResult Add()
        {
            string userid = string.Empty;
            if (Request.Cookies["userName"] != null)
            {
                userid = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }

            if (string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "q", ReturnAction = "add" });
            }

            int m_qid = DbHelperSQL.GetMaxID("q_id",  "b_question_hdr", "1=1");
            DataSet Qdtoption = new DataSet();
            DataSet QHdoption = new DataSet();
            List <NewsType> minordt = new List<NewsType>();
            List<NewsType> minorhd= new List<NewsType>();
            Qdtoption = DbHelperSQL.Query("SELECT minor_cd ,minor_nm  FROM dbo.b_minor WHERE major_cd = 'A2' ");
            QHdoption = DbHelperSQL.Query("SELECT minor_cd ,minor_nm  FROM dbo.b_minor WHERE major_cd = 'A4' ");

            for (int i = 0; i < Qdtoption.Tables[0].Rows.Count; i++)
            {
                NewsType newstp = new NewsType();



                newstp.TypeCd = Qdtoption.Tables[0].Rows[i]["minor_cd"].ToString();
                newstp.Typenm = Qdtoption.Tables[0].Rows[i]["minor_nm"].ToString();

                minordt.Add(newstp);

            }
            for (int i = 0; i < QHdoption.Tables[0].Rows.Count; i++)
            {
                NewsType newstp = new NewsType();



                newstp.TypeCd = QHdoption.Tables[0].Rows[i]["minor_cd"].ToString();
                newstp.Typenm = QHdoption.Tables[0].Rows[i]["minor_nm"].ToString();

                minorhd.Add(newstp);

            }

            ViewData["minorhd"] = minorhd;
            ViewData["minordt"] = minordt;
            ViewData["qid"] = m_qid;

            return View();
        }

        [ValidateInput(false)]
        // GET: News
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            int AskSeq = int.Parse(Request.QueryString["id"]); //题目id
            if (string.IsNullOrWhiteSpace(Request["chk"].ToString()))
            {
                var script = String.Format("<script>alert('题目不能为空！');location.href='{0}'</script>", Url.Action("add", "q"));//Url.Action()用于指定跳转的路径
                return Content(script, "text/html");
            }
            else
            {
                string Qhd = collection["Qhd"]; //题目
            }
            //QhdSelect
            string QhdType = collection["QhdSelect"]; //题库类型
            string chk = Request["chk"].ToString(); //正确答案
            chk = chk.Replace("false", "");
            chk = chk.Replace(",", "");

            if (string.IsNullOrWhiteSpace(chk))
            {
                var script = String.Format("<script>alert('正确答案不能为空！');location.href='{0}'</script>", Url.Action("add", "q"));//Url.Action()用于指定跳转的路径
                return Content(script, "text/html");
            }
            string Qrmk = "";

            if (Request["Qrmk"].ToString() != null && Request["Qrmk"].ToString() !="")
            {
                Qrmk= Request["Qrmk"].ToString(); //正确答案
            }

            DataSet Qdtoption = new DataSet();
            List<NewsType> minordt = new List<NewsType>();
            Qdtoption = DbHelperSQL.Query("SELECT minor_cd ,minor_nm  FROM dbo.b_minor WHERE major_cd = 'A2' ");
            for (int i = 0; i < Qdtoption.Tables[0].Rows.Count; i++)
            {
                

               string kid =  Qdtoption.Tables[0].Rows[i]["minor_cd"].ToString();
                if(Request[kid].ToString() != null && Request[kid].ToString() != "")
                {
                    string knm = Request[kid].ToString();
                }
              

            }




            return RedirectToAction("add", "q");
        }

        public ActionResult Edit()
        {
            return Content("q");
        }

        public ActionResult Del()
        {
            return Content("q");
        }
    }
}