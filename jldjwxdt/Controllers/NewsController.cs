using jldjwxdt.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jldjwxdt.Helps;

namespace jldjwxdt.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Home()
        {
            return View();
        }

        [ValidateInput(false)]
        // GET: News
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
                return RedirectToAction("Index", "login", new { ReturnCtrl = "news", ReturnAction = "add" });
            }

            DataSet ntypeds = DbHelperSQL.Query("SELECT minor_cd ,minor_nm  FROM dbo.b_minor WHERE major_cd = 'N1' ");
            List<NewsType> ntypelist = new List<NewsType>();
            for (int i = 0; i < ntypeds.Tables[0].Rows.Count; i++)
            {
                NewsType newstp = new NewsType();



                newstp.TypeCd = ntypeds.Tables[0].Rows[i]["minor_cd"].ToString();
                newstp.Typenm = ntypeds.Tables[0].Rows[i]["minor_nm"].ToString();

                ntypelist.Add(newstp);

            }
            
            ViewData["ntype"] = ntypelist;
            return View();
        }


        [ValidateInput(false)]
        // GET: News
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {

            string userid = string.Empty;
            if (Request.Cookies["userName"] != null)
            {
                userid = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }

            if (string.IsNullOrEmpty(userid))
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "news", ReturnAction = "add" });
            }
            //string userid = "admin";
            string ntypevl = collection["ntype"];
            string newstitle = collection["content1"];
            string newshd = collection["content2"];
            string newsbd = collection["content3"];
            if (string.IsNullOrWhiteSpace(newstitle) || string.IsNullOrEmpty(newshd) || string.IsNullOrEmpty(newsbd))
            {
                var script = String.Format("<script>alert('内容不能为空！');location.href='{0}'</script>", Url.Action("add", "news"));//Url.Action()用于指定跳转的路径
                return Content(script, "text/html");
            }
            else
            {
               
                  newshd =  newshd.Replace("alt=\"\"", "height=\"150\" width=\"150\" class=\"img-responsive img-rounded pull-right\" ");
               
                string newisrtsql = "exec usp_isrt_news '" +newstitle+"','"+newshd+"','"+newsbd+"','"+userid+"','"+ ntypevl+"'";


                DbHelperSQL.GetSingle(newisrtsql);
                DataSet ntypeds = DbHelperSQL.Query("SELECT minor_cd ,minor_nm  FROM dbo.b_minor WHERE major_cd = 'N1' ");
                List<NewsType> ntypelist = new List<NewsType>();
                for (int i = 0; i < ntypeds.Tables[0].Rows.Count; i++)
                {
                    NewsType newstp = new NewsType();



                    newstp.TypeCd = ntypeds.Tables[0].Rows[i]["minor_cd"].ToString();
                    newstp.Typenm = ntypeds.Tables[0].Rows[i]["minor_nm"].ToString();

                    ntypelist.Add(newstp);

                }

                ViewData["ntype"] = ntypelist;

                return View();
            }
        }

        [ValidateInput(false)]
        // GET: News
        public ActionResult NewsView()
        {
            List<NewsModel> newslist = new List<NewsModel>();
            DataSet news = new DataSet();

            news = DbHelperSQL.Query("select NewsId,NewsTitle,NewsHd,NewsBady,Isrtid,Isrtdt from U_NewsInfo");
            for (int i = 0; i < news.Tables[0].Rows.Count; i++)
            {
                NewsModel newsm = new NewsModel();

                newsm.NewsId = int.Parse(news.Tables[0].Rows[i]["Newsid"].ToString());

                newsm.NewsTitle = news.Tables[0].Rows[i]["NewsTitle"].ToString();
                newsm.NewsHd = news.Tables[0].Rows[i]["NewsHd"].ToString();
                newsm.NewsBady = news.Tables[0].Rows[i]["NewsBady"].ToString();
                newsm.Isrtdt = Convert.ToDateTime(news.Tables[0].Rows[i]["Isrtdt"].ToString());
                newslist.Add(newsm);

            }
            ViewData["News"] = newslist;

            return View();

        }


        // GET: News
        public ActionResult Edit()
        {
            return View();
        }
    }
}