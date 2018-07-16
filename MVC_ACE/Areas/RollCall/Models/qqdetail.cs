using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.RollCall.Models
{
    public class qqdetail
    {
        public string ID { get; set; } //工號

        public string Name { get; set; } //姓名
        public string Linename { get; set; } //線名
        public string Class { get; set; } //班別
        public string reason { get; set; }//缺勤原因

        public string ZTbd { get; set; }//中途變動

        public string ZTreason { get; set; }//中途變動原因
        
    }
}