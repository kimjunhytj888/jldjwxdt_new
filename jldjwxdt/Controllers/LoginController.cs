using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using jldjwxdt.Helps;

namespace jldjwxdt.Controllers
{
    public class LoginController : Controller
    {

        [HttpGet]
        public ActionResult index(FormCollection collection)
        {

            string userName = collection["userName"];
            string password = collection["password"];
            return View();
        }

        private void GetOnline(string Name)
        {
            Hashtable SingleOnline = (Hashtable)System.Web.HttpContext.Current.Application["Online"];
            if (SingleOnline == null)
                SingleOnline = new Hashtable();

            Session["mySession"] = "jldjwxdt";
            //SessionID
            if (SingleOnline.ContainsKey(Name))
            {
                SingleOnline[Name] = Session.SessionID;
            }
            else
                SingleOnline.Add(Name, Session.SessionID);

            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application["Online"] = SingleOnline;
            System.Web.HttpContext.Current.Application.UnLock();
        }

        public class LoginActionFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                Hashtable singleOnline = (Hashtable)filterContext.HttpContext.Application["Online"];

                var test = filterContext.HttpContext.User.Identity.Name;
                // 判断当前SessionID是否存在
                if (singleOnline != null && singleOnline.ContainsKey(filterContext.HttpContext.User.Identity.Name))
                {
                    if (!singleOnline[filterContext.HttpContext.User.Identity.Name].Equals(filterContext.HttpContext.Session.SessionID))
                    {
                        filterContext.Result = new ContentResult() { Content = "<script>if(confirm('你的账号已在别处登陆，是否返回登陆页面重新登陆？')){window.location.href='/Authentication/Login';}else{window.close();}</script>" };
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }

        [LoginActionFilter]
        [HttpPost]
        public ActionResult index(string ReturnCtrl, string ReturnAction)
        {

            string login = Request.Form["login"];


            if (login == "Y")
            {
                string userName = Request.Form["userName"];
                string password = Request.Form["password"];
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password))
                {
                    var script = String.Format("<script>alert('用户名或者密码不能为空！');location.href='{0}'</script>", Url.Action("index", "login"));//Url.Action()用于指定跳转的路径
                    return Content(script, "text/html");
                }
                else
                {
                    Encrypt encrypt = new Encrypt();
                    string pwdsql = "select user_pwd from b_user where user_id = '" + userName + "'";
                    if( encrypt.JiaMi(password) == DbHelperSQL.GetSingle(pwdsql).ToString())
                    {
                   
                        GetOnline(userName);

                        //把登陆用户名存到cookies中
                        Response.Cookies["userName"].Value = userName;
                        Response.Cookies["password"].Value = password;
                        Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["password"].Expires = DateTime.Now.AddDays(1);




                     

                        if (!string.IsNullOrEmpty(ReturnCtrl))
                        {
                            return RedirectToAction(ReturnAction, ReturnCtrl);
                        }

                        else
                        {
                            return RedirectToAction("login", "supadmin");//成功时的跳转路径
                        }
                    }
                    else
                    {
                        var script = String.Format("<script>alert('用户名或密码错误！请重新确认！');location.href='{0}'</script>", Url.Action("index", "login"));//Url.Action()用于指定跳转的路径
                        return Content(script, "text/html");
                    }
                }
            }
            // return RedirectToAction("Index", "Home");//成功时的跳转路径
            else
            {
                string userName = Request.Form["userName"];
                if (string.IsNullOrWhiteSpace(userName))
                {
                    var script = String.Format("<script>alert('请输入需要重置密码的ID！');location.href='{0}'</script>", Url.Action("index", "login"));//Url.Action()用于指定跳转的路径


                    return Content(script, "text/html");
                }
                else
                {
                    SendMessage sendMessage = new SendMessage();
                    var rtn = sendMessage.ResetPwd(userName);
                    var script = String.Format("<script>alert('已发送重置密码申请 请联系部门管理员！');location.href='{0}'</script>", Url.Action("index", "login"));//Url.Action()用于指定跳转的路径


                    return Content(script, "text/html");
                }
            }
        }


    }
}