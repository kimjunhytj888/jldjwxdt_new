﻿using System;
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
        public ActionResult index(string i)
        {
            

            return View();

        }

      


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
                    if (userName == "admin")
                    {




                        //把登陆用户名存到cookies中
                        Response.Cookies["userName"].Value = userName;
                        Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);




                        //HttpCookie UserCookie = new HttpCookie("jldjwxdt");

                        /////设置cookie 
                        /////userid 用户id


                        //UserCookie["username"] = userName.ToString(); //
                        //UserCookie.Expires = DateTime.Now.AddDays(10);//这里设置要保存多长时间.
                        //Response.Cookies.Add(UserCookie);

                        if (!string.IsNullOrEmpty(ReturnCtrl))
                        {
                            return RedirectToAction(ReturnAction, ReturnCtrl);
                        }

                        else
                        {                            
                            return RedirectToAction("Index", "user");//成功时的跳转路径
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