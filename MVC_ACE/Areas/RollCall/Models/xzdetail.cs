using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.RollCall.Models
{
    public class xzdetail
    {
        public string ID { get; set; }
        public string NAME { get; set; }

        public bool cq { get; set; }//出勤

        public string cd { get; set; }//遲到

        public string qj { get; set; }//請假
        public string kg { get; set; }//曠工
        public string lz { get; set; }//曠工
        public string qt { get; set; }//其他

        public string zsb { get; set; }//中途上班
        public string zzt { get; set; }//中途早退
        public string zqj { get; set; }//中途請假
        public string ztx { get; set; }//中途調休

        public double time1 { get; set; }//預報加班
        public double time2 { get; set; }//實報加班

    }
}