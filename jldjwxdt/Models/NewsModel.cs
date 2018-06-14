using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace jldjwxdt.Models
{
    public class NewsModel
    {
        [Display(Name = "新闻ID")]
        public int NewsId { get; set; } //新闻id
        [Display(Name = "新闻标题")]
        public string NewsTitle { get; set; } //新闻标题 列表显示用
        [Display(Name = "标题图片")]
        public string NewsHd { get; set; } //新闻缩略图
        [Display(Name = "新闻主体")]
        public string NewsBady { get; set; } //新闻本体 点击title显示

        //public string NewsPic { get; set; } //新闻图片
        [Display(Name = "输入用户")]
        public string Isrtid { get; set; } //输入用户id
        [Display(Name = "输入时间")]
        public DateTime Isrtdt { get; set; } //输入时间
        
    }
}