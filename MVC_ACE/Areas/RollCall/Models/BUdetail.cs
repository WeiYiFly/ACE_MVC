using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.RollCall.Models
{
    public class BUdetail
    {
        //部門 應到人數 實到人數  遲到 請假 曠工 中途上班 早退 請假 曠工 預報加班
        public string BUID { get; set; } //部門

        public int BUIDID { get; set; } //部門對應的主鍵
        public string CLASS { get; set; } //白晚班
        public int yd { get; set; } //應到人數
        public int sd { get; set; }//實到人數

        public int cd { get; set; }//遲到

        public int qj { get; set; }//請假
        public int kg { get; set; }//曠工
        public int lz { get; set; }//離職
        public int qt { get; set; }//其他


        public int zsb { get; set; }//中途上班
        public int zzt { get; set; }//中途早退
        public int zqj { get; set; }//中途請假
        public int ztx { get; set; }//中途調休

        public double time1 { get; set; }//預報加班
        public int timeOne1 { get; set; }//預報加班人數
        public double time2 { get; set; }//實際加班
        public int timeOne2 { get; set; }//實際加班人數
        public string timeOne12 { get; set; }//比例
    }
}