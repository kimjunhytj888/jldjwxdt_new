using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jldjwxdt.Models
{
    public class NewsType
    {
        [Display(Name = "新闻类型Code")]
        public string TypeCd { get; set; } //类型CD
        [Display(Name = "新闻类型")]
        public string Typenm { get; set; } //类型名称

    }
}