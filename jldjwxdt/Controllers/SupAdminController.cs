using jldjwxdt.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace jldjwxdt.Controllers
{
    public class SupAdminController : Controller
    {
        // GET: SupAdmin
        public ActionResult Index()
        {
            return View();
        }

       // [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string username = string.Empty;
            string password = string.Empty;

            if (Request.Cookies["username"] != null)
            {

                username = Server.HtmlEncode(Request.Cookies["userName"].Value);
                password = Server.HtmlEncode(Request.Cookies["password"].Value);
            }
            else
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "supadmin", ReturnAction = "login" });
            }

            string usrtypesql = "select user_type from b_user where user_id = '"+username+"'";

            string user_type = DbHelperSQL.GetSingle(usrtypesql).ToString();

            if(user_type == "U")
            {
                return RedirectToAction("LoginUser");
            }
            else if  (user_type == "A")
            {
                return RedirectToAction("LoginAdmin");
            }
            else if (user_type == "S")
            {
                return RedirectToAction("LoginSupadmin");
            }
            else
                return Content("请从正确入口访问程序！");


        }


        
        public ActionResult LoginUser()
        {

            string username = string.Empty;
            if (Request.Cookies["username"] != null)
            {

                username = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }
            else
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "supadmin", ReturnAction = "LoginUser" });
            }

            DataSet ds = new DataSet();


            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("EXEC USP_USER_SUM ");
            str_sql.Append("@fr_dt = N'2018-01-01',");//.Append(usr_id).Append("',");
            str_sql.Append("@to_dt = N'2018-05-31',");//.Append(q_seq).Append("'");
            str_sql.Append("@dept_cd = N'',");
            str_sql.Append("@usr_id = N'").Append(username).Append("'");



            ds = DbHelperSQL.Query(str_sql.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                List<User_info> user_s = new List<User_info>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    User_info user_Info = new User_info();
                    user_Info.usr_id = ds.Tables[0].Rows[i]["usr_id"].ToString();
                    user_Info.dt_cnt = Convert.ToInt32(ds.Tables[0].Rows[i]["dt_cnt"].ToString());
                    user_Info.all_fen = Convert.ToInt32(ds.Tables[0].Rows[i]["all_fen"].ToString());
                    user_Info.pingjunfen = Convert.ToInt32(ds.Tables[0].Rows[i]["pingjun"].ToString());
                    user_Info.paihang = Convert.ToInt32(ds.Tables[0].Rows[i]["paihang"].ToString());
                    user_s.Add(user_Info);
                    ViewData["user_s"] = user_s;
                }


            }
            return View("Login");
        }

        public ActionResult LoginAdmin()
        {
            string username = string.Empty;
            if (Request.Cookies["username"] != null)
            {

                username = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }
            else
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "supadmin", ReturnAction = "LoginAdmin" });
            }
            return View("Login");
        }

        public ActionResult LoginSupadmin()
        {
            string username = string.Empty;
            if (Request.Cookies["username"] != null)
            {

                username = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }
            else
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "supadmin", ReturnAction = "LoginSupadmin" });
            }
            return View();
        }
    }
}