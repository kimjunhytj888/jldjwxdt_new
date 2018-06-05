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


          //  static
           

    // GET: SupAdmin


    public ActionResult Index()
        {
            return Content("supadmin");
        }

        // [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            string UserName = string.Empty;

            if (Request.Cookies["userName"] != null)
            {

                UserName = Server.HtmlEncode(Request.Cookies["userName"].Value);
            }


            if (string.IsNullOrEmpty(UserName))
            {
                return RedirectToAction("Index", "login", new { ReturnCtrl = "supadmin", ReturnAction = "login" });
            }


            string sing_sql = "select user_type from b_user where user_id = '"+UserName+"'" ;

            string user_type =  DbHelperSQL.GetSingle(sing_sql).ToString();

            if (user_type == "U")
            {
                return RedirectToAction("Login_user", "supadmin", new {  UserName });
            }
            else if (user_type == "A")
            {
                return RedirectToAction("Login_Admin", "supadmin", new {  UserName });
            }
            else if (user_type == "S")
            {
                return RedirectToAction("Login_SupAdmin", "supadmin", new {  UserName });
            }
            else
                return Content("非法用户");

        }


        public ActionResult Login_user(string UserName)
        {


            //string usr_id = collection["username"];
            //string pwd = collection["password"];
            // User_info user_Info = new User_info();
            ViewData["username"] = UserName;


            DataSet ds = new DataSet();


            StringBuilder str_sql = new StringBuilder();
            str_sql.Append("EXEC USP_USER_SUM ");
            str_sql.Append("@fr_dt = N'2018-01-01',");//.Append(usr_id).Append("',");
            str_sql.Append("@to_dt = N'2018-05-31',");//.Append(q_seq).Append("'");
            str_sql.Append("@dept_cd = N'',");
            str_sql.Append("@usr_id = N'").Append(UserName).Append("'");



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
            return Content(UserName);
        }

        public ActionResult Login_Admin(string UserName)
        {

            return Content(UserName);
        }


        public ActionResult Login_SupAdmin(string UserName)
        {
            return Content(UserName);
        }


    }
}