using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace jldjwxdt.Models
{

 
    public class _QaListPage
    {
        [Display(Name = "用户ID")]
        public string userid { get; set; }
        [Display(Name = "部门名称")]
        [MaxLength(500)]
        public string dept_nm { get; set; }
        [Display(Name = "答题日期")]
        public string q_dt { get; set; }
        [Display(Name = "答题次数")]
        public int q_seq { get; set; }
        [Display(Name = "分数")]
        public int q_fen { get; set; }
      
    }
}