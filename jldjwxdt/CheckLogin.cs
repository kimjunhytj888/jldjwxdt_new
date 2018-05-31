using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jldjwxdt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public sealed class CheckLogin : Attribute
    {
        public bool IsNeedLogin = false;

        public CheckLogin(bool isNeed)
        {
            this.IsNeedLogin = isNeed;
        }
    }
}