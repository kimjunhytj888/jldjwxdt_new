using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jldjwxdt.Controllers
{
    public class ParentController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            bool result = false;

            //controller上是否有特性CheckLogin，以及特性的IsNeedLogin值
            var controllerAttrs = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(CheckLogin), false);
            if (controllerAttrs.Count() > 0)
            {
                var conAttr = controllerAttrs[0] as CheckLogin;
                if (conAttr != null)
                {
                    if (conAttr.IsNeedLogin)
                        result = true;
                    else
                        result = false;
                }
            }

            //action上是否有特性CheckLogin，以及特性的IsNeedLogin值
            var actionAttrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckLogin), false);
            if (actionAttrs.Count() > 0)
            {
                var attr = actionAttrs[0] as CheckLogin;
                if (attr != null)
                {
                    if (attr.IsNeedLogin)
                        result = true;
                    else
                        result = false;
                }
            }

            if (!IsLogin() && result)
            {
                //如果没有登录，则跳至登陆页
                filterContext.Result = Redirect("/Login/index");
            }
        }

        protected bool IsLogin()
        {
            if (Session["UserInfo"] != null)
                return true;

            return false;
        }
    }
}