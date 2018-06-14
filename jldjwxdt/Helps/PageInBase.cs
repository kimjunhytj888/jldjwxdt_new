using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jldjwxdt.Helps
{
    /// <summary>
    /// 分页基础类
    /// </summary>
    public class PagerInBase
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int PageSize { get; set; }

        //跳过序列中指定数量的元素
        public int Skip => (PageIndex - 1) * PageSize;

        /// <summary>
        /// 请求URL
        /// </summary>
        public string RequetUrl => System.Web.HttpContext.Current.Request.Url.OriginalString;

        /// <summary>
        /// 构造函数给当前页和页数初始化
        /// </summary>
        public PagerInBase()
        {
            if (PageIndex == 0) PageIndex = 1;
            if (PageSize == 0) PageSize = 10;
        }
    }
}