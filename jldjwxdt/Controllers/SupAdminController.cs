using jldjwxdt.Helps;
using jldjwxdt.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace jldjwxdt.Controllers
{
    public class SupAdminController : Controller
    {


        static DataTable dt1; //导出表格用【明细】
        static DataTable dt2; //导出表格用【最高分】


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
                DataSet Qds = new DataSet();
                string fr_dt = "2018-01-01";
                string to_dt = DateTime.Now.ToShortDateString();
                string dept_cd = UserName.Substring(0, 3);
                string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','" + dept_cd + "','"+ UserName +"'";

                Qds = DbHelperSQL.Query(Q_str);

                List<_QaListPage> qalists = new List<_QaListPage>();


                for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
                {
                    _QaListPage listPage = new _QaListPage();
                    listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                    listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                    listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                    listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                    listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
                    qalists.Add(listPage);
                }
                return RedirectToAction("Login_user", "supadmin", new { qalists });
            }
            else if (user_type == "A")
            {
                DataSet Qds = new DataSet();
                string fr_dt = "2018-01-01";
                string to_dt = DateTime.Now.ToShortDateString();
                string dept_cd = UserName.Substring(0, 3);
                string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','" + dept_cd + "','%'";

                Qds = DbHelperSQL.Query(Q_str);

                List<_QaListPage> qalists = new List<_QaListPage>();
                

                for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
                {
                    _QaListPage listPage = new _QaListPage();
                    listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                    listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                    listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                    listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                    listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
                    qalists.Add(listPage);
                }
                return RedirectToAction("Login_Admin", "supadmin", new {  qalists });
            }
            else if (user_type == "S")
            {
                DataSet Qds = new DataSet();
                string fr_dt = "2018-01-01";
                string to_dt = DateTime.Now.ToShortDateString();
                string dept_cd = UserName.Substring(0, 3);
                string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','%','%'";

                Qds = DbHelperSQL.Query(Q_str);

                List<_QaListPage> qalists = new List<_QaListPage>();


                for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
                {
                    _QaListPage listPage = new _QaListPage();
                    listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                    listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                    listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                    listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                    listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
                    qalists.Add(listPage);
                }
                return RedirectToAction("Login_SupAdmin", "supadmin", new { qalists });
            }
            else
                return Content("非法用户");

        }


        public ActionResult Login_user(_QaListPageParams param)
        {
            string UserName = Server.HtmlEncode(Request.Cookies["userName"].Value);
            DataSet Qds = new DataSet();
            string fr_dt = "2018-01-01";
            string to_dt = DateTime.Now.ToShortDateString();
            string dept_cd = UserName.Substring(0, 3);
            string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','" + dept_cd + "','"+ UserName +"'";

            Qds = DbHelperSQL.Query(Q_str);
            dt1 = Qds.Tables[0];
            List<_QaListPage> qalists = new List<_QaListPage>();


            for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
            {
                _QaListPage listPage = new _QaListPage();
                listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
                qalists.Add(listPage);
            }
            //每页显示的条数默认10
            param.PageSize = 10;

            //保存搜索条件
            ViewBag.Searchuser = param.userid;
            ViewBag.Searchdept = param.dept_nm;


            //获取数据集合
            var list = qalists;// PageContent();

            //根据条件检索
            var query = param.userid != null ?
                list.Where(t => t.userid.Contains(param.userid)).ToList() :
                list.ToList();

            //根据条件检索
            var query1 = param.dept_nm != null ?
                list.Where(t => t.dept_nm.Contains(param.dept_nm)).ToList() :
                list.ToList();

            //分页数据
            var data = query.Skip(param.Skip).Take(param.PageSize);

            //总页数
            var count = query.Count;
            var res = new PagerResult<_QaListPage>
            {
                Code = 0,
                DataList = data,
                Total = count,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                RequestUrl = param.RequetUrl
            };
            return View(res);


        }

        public ActionResult Login_Admin(_QaListPageParams param)
        {

            string UserName = Server.HtmlEncode(Request.Cookies["userName"].Value);
            DataSet Qds = new DataSet();
            string fr_dt = "2018-01-01";
            string to_dt = DateTime.Now.ToShortDateString();
            string dept_cd = UserName.Substring(0, 3);
            string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','" + dept_cd + "','%'";

            Qds = DbHelperSQL.Query(Q_str);
            dt1 = Qds.Tables[0];
            List<_QaListPage> qalists = new List<_QaListPage>();
           

            for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
            {
                _QaListPage listPage = new _QaListPage();
                listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
                qalists.Add(listPage);
            }
            //每页显示的条数默认10
            param.PageSize = 10;

            //保存搜索条件
            ViewBag.Searchuser = param.userid;
            ViewBag.Searchdept = param.dept_nm;


            //获取数据集合
            var list = qalists;// PageContent();

            //根据条件检索
            var query = param.userid != null ?
                list.Where(t => t.userid.Contains(param.userid)).ToList() :
                list.ToList();

            //根据条件检索
            var query1 = param.dept_nm != null ?
                list.Where(t => t.dept_nm.Contains(param.dept_nm)).ToList() :
                list.ToList();


            //分页数据
            var data = query.Skip(param.Skip).Take(param.PageSize);

            //总页数
            var count = query.Count;
            var res = new PagerResult<_QaListPage>
            {
                Code = 0,
                DataList = data,
                Total = count,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                RequestUrl = param.RequetUrl
            };
            return View(res);

            
               
        }

       

        public ActionResult Login_SupAdmin(_QaListPageParams param)
        {

            string UserName = Server.HtmlEncode(Request.Cookies["userName"].Value);
            DataSet Qds = new DataSet();
            string fr_dt = "2018-01-01";
            string to_dt = DateTime.Now.ToShortDateString();
            string dept_cd = UserName.Substring(0, 3);
            string Q_str = "exec USP_USR_SUM_Q '" + fr_dt + "','" + to_dt + "','%','%'";

            Qds = DbHelperSQL.Query(Q_str);
            dt1 = Qds.Tables[0];
            dt2 = Qds.Tables[1];
            List<_QaListPage> qalists = new List<_QaListPage>();


            for (int i = 0; i < Qds.Tables[0].Rows.Count; i++)
            {
                _QaListPage listPage = new _QaListPage();
                listPage.userid = Qds.Tables[0].Rows[i]["用户id"].ToString();
                listPage.dept_nm = Qds.Tables[0].Rows[i]["部门名称"].ToString();
                listPage.q_dt = Convert.ToDateTime(Qds.Tables[0].Rows[i]["答题日期"].ToString()).ToString("yyyy-MM-dd");
                listPage.q_seq = int.Parse(Qds.Tables[0].Rows[i]["答题次数"].ToString());
                listPage.q_fen = int.Parse(Qds.Tables[0].Rows[i]["分数"].ToString());
               
                qalists.Add(listPage);
            }
            //每页显示的条数默认10
            param.PageSize = 10;

            //保存搜索条件
            ViewBag.Searchuser = param.userid;
            ViewBag.Searchdept = param.dept_nm;


            //获取数据集合
            var list = qalists;// PageContent();

            ////根据条件检索
            //var query = param.userid != null ?
            //    list.Where(t => t.userid.Contains(param.userid)).ToList() :
            //    list.ToList();

            //根据条件检索
            var query = param.dept_nm != null ?
                list.Where(t => t.dept_nm.Contains(param.dept_nm)).ToList() :
                list.ToList();

        



            //分页数据
            var data = query.Skip(param.Skip).Take(param.PageSize);

            dt1 = D2t.ToDataTable(query);

            //总页数
            var count = query.Count;
            var res = new PagerResult<_QaListPage>
            {
                Code = 0,
                DataList = data,
                Total = count,
                PageSize = param.PageSize,
                PageIndex = param.PageIndex,
                RequestUrl = param.RequetUrl
            };
            return View(res);
        }

    public void OutPutExcel()
        {
           

            ExcelDownload excel = new ExcelDownload();

            excel.ExportExcel(dt1);

           
        }

        public void OutPutExcel1()
        {


            ExcelDownload excel = new ExcelDownload();

            excel.ExportExcel(dt2);


        }

        //用户答题明细查询 
        public ActionResult QueryUser(string usrid, DateTime q_dt, int q_seq)
        {
            DataSet QueryUserDs = new DataSet();
            string Qdt = q_dt.ToShortDateString();
            StringBuilder QueryUserSql = new StringBuilder();
            QueryUserSql.Append("exec usp_ask_QueryUser ");
            QueryUserSql.Append("@user_id = '").Append(usrid).Append("',");
            QueryUserSql.Append("@q_dt = '").Append(Qdt).Append("',");
            QueryUserSql.Append("@q_seq = ").Append(q_seq);

            QueryUserDs = DbHelperSQL.Query(QueryUserSql.ToString());

            List<QAskEndByPidReal> Lqend = new List<QAskEndByPidReal>();

            for (int i = 0; i < QueryUserDs.Tables[0].Rows.Count; i++)
            {
                QAskEndByPidReal QueryUser = new QAskEndByPidReal();

               
                QueryUser.eq_sub_seq = int.Parse(QueryUserDs.Tables[0].Rows[i]["q_sub_seq"].ToString());
                QueryUser.eq_nm = QueryUserDs.Tables[0].Rows[i]["q_nm"].ToString();
                QueryUser.eq_key = QueryUserDs.Tables[0].Rows[i]["q_key"].ToString();
                QueryUser.eu_key = QueryUserDs.Tables[0].Rows[i]["k_val"].ToString();
                QueryUser.eq_rmk = QueryUserDs.Tables[0].Rows[i]["q_rmk"].ToString();
                Lqend.Add(QueryUser);

            }

            ViewData["fen"] = DbHelperSQL.GetSingle("exec usp_ask_QueryUser_fen '" + usrid + "','"+ Qdt + "',"+ q_seq);
            ViewData["dtck"] = "需要加油哦！";
            ViewData["QueryUser"] = Lqend;//==ViewData中





            return View();            
        }

        //同步数据 
        public ActionResult CommitData()
        {


            string commsql = "exec Usp_commitData";
             DbHelperSQL.GetSingle(commsql);


            return RedirectToAction("Login", "supadmin");



        }

    }
}