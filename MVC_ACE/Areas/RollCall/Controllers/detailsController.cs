using MVC_ACE.Areas.RollCall.Common;
using MVC_ACE.Areas.RollCall.Models;
using RollCall.BLL;
using RollCall.IBLL;
using RollCall.Model;
using RollCall.UI.Commo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.RollCall.Controllers
{
    public class detailsController : BasePhoneController
    {
        IAttendanceService attendanceService = new AttendanceService();
        DetailsCommon common = new DetailsCommon();
        DataModelContainer DB1 = new DataModelContainer();
        // GET: details
        public ActionResult Index()
        {
            return View();
        }
        //全廠明細
        public ActionResult QCdetail(string id)
        {
            //文员维护跳转用
            if (id == "010010")
            {
                Session["UserId"] = id;
            }
            #region  查詢時間 設置
            if (id == null) id = "0";
            DateTime now = DateTime.Now.AddDays(int.Parse(id));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                    id = (now - DateTime.Now).Days.ToString();
                };

            }
            #endregion

            DataModelContainer DB1 = new DataModelContainer();
            List<QCdetail> qcdetails = new List<QCdetail>();
            QCdetail qcdetail = new QCdetail();
            //權限判斷
            string UserId = Session["UserId"].ToString();
            var power_ = DB1.Power_.FirstOrDefault(u => u.id == UserId);
            //獲取今天出去明細
            var tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end);
            //編制人力
            GigabyteFrameEntities db = new GigabyteFrameEntities();

            if (power_ != null && (power_.QC == "Y" || power_.BUx != ""))
            {
                //有多少線
                var Lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => true).OrderBy(u => u.OrderbBy).Select(u => u.type1).ToList();
                Lists.Add("PM11"); Lists.Add("PM12"); Lists.Add("PM14"); Lists.Add("PM31");
                //特殊部門PE ，與PQ一樣
                Lists.Add("PE");

                Lists = Lists.Distinct().ToList();
                //下面的四個部門統稱為 PM 要做特殊添加
                //Lists.Add("PM11"); Lists.Add("PM12"); Lists.Add("PM14"); Lists.Add("PM31");
                if (power_.QC == "Y")
                {

                    var bz = db.DepartmentCompilation.Where(u => Lists.Contains(u.department)).Sum(u => u.nums);
                    var BZ = int.Parse(bz.ToString());
                    #region  獲取全廠的出勤 EF
                    /**
                    var yd = attendanceService.DbSession.StaffDal.GetEntity(u => true).Count();
                    //獲取今天出去明細
                   //  var tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end);             
                    var sd = tatol.Where(u => u.state1 == "到").Count();
                    //缺勤狀況
                    var cd = tatol.Where(u => u.reason1 == "遲到").Count();
                    var qj = tatol.Where(u => u.reason1 == "請假").Count();
                    var kg = tatol.Where(u => u.reason1 == "曠工").Count();
                    var lz = tatol.Where(u => u.reason1 == "離職").Count();
                    var qt = tatol.Where(u => u.state1 != "到" && u.reason1 != "遲到" && u.reason1 != null && u.reason1 != ""
                            && u.reason1 != "請假" && u.reason1 != "曠工" && u.reason1 != "離職").Count();
                    //中途表動
                    var zsb = tatol.Where(u => u.state2 == "中途上班").Count();
                    var zzt = tatol.Where(u => u.reason2 == "早退").Count();
                    var zqj = tatol.Where(u => u.reason2 == "請假").Count();
                    var ztx = tatol.Where(u => u.reason2 == "調休").Count();
                    //預報，實際 加班
                    var time1 = tatol.Where(u => u.time1 != null).
                        Sum(u => u.time1);
                    var timeOne1 = tatol.Where(u => u.time1 > 0).Count();
                    //實際加班
                    var time2 = DB1.FactWorkTimeR.Where(u =>  u.date_ == start).Sum(u => u.WorkTime).ToString();                   
                    var timeOne2 = DB1.FactWorkTimeR.Where(u =>  u.date_ == start && u.WorkTime>0).Count();
                    
                    //預報/實際比
                    var timeOne = 0;

                    qcdetail.BUID = "全廠";
                    qcdetail.bz = BZ; qcdetail.yd = yd;
                    qcdetail.sd = sd;
                    qcdetail.cd = cd; qcdetail.qj = qj; qcdetail.kg = kg; qcdetail.lz = lz; qcdetail.qt = qt;
                    qcdetail.zsb = zsb; qcdetail.zzt = zzt; qcdetail.zqj = zqj; qcdetail.ztx = ztx;

                    if (time1 != null) qcdetail.time1 = double.Parse(time1.ToString());
                    qcdetail.timeOne1 = timeOne1;
                    if (time2 != null && time2 != "") qcdetail.time2 = double.Parse(time2.ToString());
                    qcdetail.timeOne2 = timeOne2;

                    qcdetail.timeOne = timeOne;
                    qcdetails.Add(qcdetail);
                     **/
                    #endregion

                    #region 獲取全廠明細 SQL
                    string start1 = start.ToString("yyyy/MM/dd");
                    string sql = @"select ayd,bsd,bcd,bqj,bkg,blz,bqt,zsb,zzt,zqj,ztx,ybs,ybr,sbs,sbr
                    from 
                    (select COUNT(*) as ayd from  staff )a,
                    (select count(*) bsd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state1='到')b,
                    (select count(*) bcd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='遲到')c,
                    (select count(*) bqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='請假')d,
                    (select count(*) bkg from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='曠工')e,
                    (select count(*) blz from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='離職')f,
                    (select count(*) bqt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1!='遲到' and reason1!='請假' and reason1!='曠工' and reason1!='離職' )g,
                    (select count(*) zsb from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state2='中途來了')h,
                    (select count(*) zzt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='早退')i,
                    (select count(*) zqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='請假')j,
                    (select count(*) ztx from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='調休')k,
                    (select sum(time1)  ybs  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' )l,
                    (select count(time1)  ybr  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and time1>0 )m,
                    (select sum(WorkTime)  sbs  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' )n,
                    (select count(WorkTime)  sbr  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and WorkTime>0 )o";
                    DataTable dt = SQLHelper.GetTable(sql, CommandType.Text);
                    qcdetail.BUID = "全廠";
                    qcdetail.bz = BZ;
                    qcdetail.yd = int.Parse(dt.Rows[0][0].ToString());
                    qcdetail.sd = int.Parse(dt.Rows[0][1].ToString());
                    qcdetail.cd = int.Parse(dt.Rows[0][2].ToString());
                    qcdetail.qj = int.Parse(dt.Rows[0][3].ToString());
                    qcdetail.kg = int.Parse(dt.Rows[0][4].ToString());
                    qcdetail.lz = int.Parse(dt.Rows[0][5].ToString());
                    qcdetail.qt = int.Parse(dt.Rows[0][6].ToString());
                    qcdetail.zsb = int.Parse(dt.Rows[0][7].ToString());
                    qcdetail.zzt = int.Parse(dt.Rows[0][8].ToString());
                    qcdetail.zqj = int.Parse(dt.Rows[0][9].ToString());
                    qcdetail.ztx = int.Parse(dt.Rows[0][10].ToString());
                    if (dt.Rows[0][11] != null && dt.Rows[0][11].ToString() != "")
                        qcdetail.time1 = double.Parse(dt.Rows[0][11].ToString());
                    qcdetail.timeOne1 = int.Parse(dt.Rows[0][12].ToString());
                    if (dt.Rows[0][13] != null && dt.Rows[0][13].ToString() != "")
                        qcdetail.time2 = double.Parse(dt.Rows[0][13].ToString());
                    qcdetail.timeOne2 = int.Parse(dt.Rows[0][14].ToString());
                    //qcdetail.timeOne = 0;
                    //預報加班比=（預報-實報）/預報
                    if (qcdetail.time1 != 0)
                        qcdetail.timeOne = Math.Round(((qcdetail.time1 - qcdetail.time2) / qcdetail.time1 * 100),2).ToString() + "%";
                    else
                        qcdetail.timeOne = "0%";
                    qcdetails.Add(qcdetail);
                    #endregion                 

                }
                if (power_.QC != "Y" && power_.BUx.Length <= 6)
                {
                    return RedirectToAction("../../RollCall/details/BUdetail/" + power_.BUx + "/,0");
                }
               

                #region   個部門的出勤情況 EF
                /**
                //個部門的出勤情況
                foreach (string bu in Lists)
                {
                    QCdetail qcdetail1 = new QCdetail();
                    //更具PM，特殊情況處理
                    int BZ1 = 0;
                
                    if (bu == "PM")
                    {
                        BZ1 = db.DepartmentCompilation.Where(u => u.department == "PM11" || u.department == "PM12" || u.department == "PM14" || u.department == "PM31").Sum(u => u.nums);
                       
                    }

                    if (bu == "PQ")
                    {
                        BZ1 = db.DepartmentCompilation.Where(u => u.department == "PQ12" || u.department == "PQ13" || u.department == "PQ15").Sum(u => u.nums);
                       
                    }
                    if (bu == "PE")
                    {
                        BZ1 = db.DepartmentCompilation.Where(u => u.department == "PE11" || u.department == "PE16" || u.department == "PE17").Sum(u => u.nums);
                     
                    }
                    if (bu != "PQ" && bu != "PM" && bu != "PE")
                    {
                        BZ1 = db.DepartmentCompilation.Where(u => u.department == bu).Sum(u => u.nums);
                      
                    }

                    var yd1 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU == bu).Count();
                    var sd1 = tatol.Where(u => u.state1 == "到" && u.BU == bu).Count();
                    //缺勤狀況
                    var cd1 = tatol.Where(u => u.reason1 == "遲到" && u.BU == bu).Count();
                    var qj1 = tatol.Where(u => u.reason1 == "請假" && u.BU == bu).Count();
                    var kg1 = tatol.Where(u => u.reason1 == "曠工" && u.BU == bu).Count();
                    var lz1 = tatol.Where(u => u.reason1 == "離職" && u.BU == bu).Count();
                    var qt1 = tatol.Where(u => u.BU == bu && u.state1 != "到" && u.reason1 != "遲到" && u.reason1 != null && u.reason1 != ""
                      && u.reason1 != "請假" && u.reason1 != "曠工").Count();
                    //中途表動
                    var zsb1 = tatol.Where(u => u.state2 == "中途上班" && u.BU == bu).Count();
                    var zzt1 = tatol.Where(u => u.reason2 == "早退" && u.BU == bu).Count();
                    var zqj1 = tatol.Where(u => u.reason2 == "請假" && u.BU == bu).Count();
                    var ztx1 = tatol.Where(u => u.reason2 == "調休" && u.BU == bu).Count();
                    //預報，實際 加班
                    var time11 = tatol.Where(u => u.time1 != null && u.BU == bu).Sum(u => u.time1);
                    var timeOne11 = tatol.Where(u => u.time1 > 0 && u.BU == bu).Count();
                    //實際加班
                    var time12 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.bu == bu).Sum(u => u.WorkTime).ToString();
                    var timeOne12 = DB1.FactWorkTimeR.Where(u => u.date_ == start&& u.bu == bu && u.WorkTime > 0).Count();
                    //預報/實際比
                    var timeOne22 = 0;
                    // var time12 = DB1.FactWorkTime.Where(u => Lists.Contains(u.bu) && u.date_ == start).Sum(u => u.workTmenumber).ToString();
                    //var timeOne12 = DB1.FactWorkTime.Where(u => Lists.Contains(u.bu) && u.date_ == start).Sum(u => u.workEpynumber).ToString();

                    // var time12 = tatol.Where(u => u.time1 != null && u.BU == bu).Sum(u => u.time1);
                    // var timeOne12 = tatol.Where(u => u.time1 > 0 && u.BU == bu).Count();
                  //  var timeOne22 = tatol.Where(u => u.time1 > 0 && u.BU == bu).Count();
                    #region 特殊部門處理
                    if (bu == "PQ")
                    {
                        yd1 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15").Count();
                        sd1 = tatol.Where(u => u.state1 == "到" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        //缺勤狀況
                        cd1 = tatol.Where(u => u.reason1 == "遲到" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        qj1 = tatol.Where(u => u.reason1 == "請假" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        kg1 = tatol.Where(u => u.reason1 == "曠工" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        lz1 = tatol.Where(u => u.reason1 == "曠工" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        qt1 = tatol.Where(u => (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15") && u.state1 != "到" && u.reason1 != "遲到" && u.reason1 != null && u.reason1 != ""
                        && u.reason1 != "請假" && u.reason1 != "曠工").Count();
                        //中途表動
                        zsb1 = tatol.Where(u => u.state2 == "中途上班" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        zzt1 = tatol.Where(u => u.reason2 == "早退" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        zqj1 = tatol.Where(u => u.reason2 == "請假" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        ztx1 = tatol.Where(u => u.reason2 == "調休" && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        //預報，實際 加班
                        time11 = tatol.Where(u => u.time1 != null && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Sum(u => u.time1);
                        timeOne11 = tatol.Where(u => u.time1 > 0 && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15")).Count();
                        //實際加班
                         time12 = DB1.FactWorkTimeR.Where(u => u.date_ == start && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15")).Sum(u => u.WorkTime).ToString();
                         timeOne12 = DB1.FactWorkTimeR.Where(u => u.date_ == start && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15") && u.WorkTime > 0).Count();
                        //預報/實際比
                         timeOne22 = 0;
                    }
                    if (bu == "PE")
                    {
                        yd1 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17").Count();
                        sd1 = tatol.Where(u => u.state1 == "到" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        //缺勤狀況
                        cd1 = tatol.Where(u => u.reason1 == "遲到" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        qj1 = tatol.Where(u => u.reason1 == "請假" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        kg1 = tatol.Where(u => u.reason1 == "曠工" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        lz1 = tatol.Where(u => u.reason1 == "曠工" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        qt1 = tatol.Where(u => (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17") && u.state1 != "到" && u.reason1 != "遲到" && u.reason1 != null && u.reason1 != ""
                        && u.reason1 != "請假" && u.reason1 != "曠工").Count();
                        //中途表動
                        zsb1 = tatol.Where(u => u.state2 == "中途上班" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        zzt1 = tatol.Where(u => u.reason2 == "早退" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        zqj1 = tatol.Where(u => u.reason2 == "請假" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        ztx1 = tatol.Where(u => u.reason2 == "調休" && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        //預報，實際 加班
                        time11 = tatol.Where(u => u.time1 != null && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Sum(u => u.time1);
                        timeOne11 = tatol.Where(u => u.time1 > 0 && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17")).Count();
                        //實際加班
                        time12 = DB1.FactWorkTimeR.Where(u => u.date_ == start && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17")).Sum(u => u.WorkTime).ToString();
                        timeOne12 = DB1.FactWorkTimeR.Where(u => u.date_ == start && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17") && u.WorkTime > 0).Count();
                        //預報/實際比
                        timeOne22 = 0;
                    }
                    #endregion


                    qcdetail1.BUID = bu;
                    qcdetail1.bz = BZ1; qcdetail1.yd = yd1;
                    qcdetail1.sd = sd1;
                    qcdetail1.cd = cd1; qcdetail1.qj = qj1; qcdetail1.kg = kg1; qcdetail1.lz = lz1; qcdetail1.qt = qt1;
                    qcdetail1.zsb = zsb1; qcdetail1.zzt = zzt1; qcdetail1.zqj = zqj1; qcdetail1.ztx = ztx1;

                    if (time11 != null) qcdetail1.time1 = double.Parse(time11.ToString());
                    qcdetail1.timeOne1 = timeOne11;
                    if (time12 != null && time12 != "") qcdetail1.time2 = double.Parse(time12.ToString());
                     qcdetail1.timeOne2 = timeOne12;
                    qcdetail1.timeOne = timeOne22;

                    qcdetails.Add(qcdetail1);
                }**/
                #endregion

                Lists.Remove("PM11"); Lists.Remove("PM12"); Lists.Remove("PM14"); Lists.Remove("PM31");               
                Lists.Remove("PQ12"); Lists.Remove("PQ13"); Lists.Remove("PQ15");                
                Lists.Remove("PE11"); Lists.Remove("PE16"); Lists.Remove("PE17");
                if (power_.QC != "Y" && power_.BUx.Length > 6)
                {
                    Lists.Clear();
                    string[] bu = power_.BUx.Split('/');
                    foreach (string a in bu)
                    {
                        Lists.Add(a);
                    }
                }

                #region 個部門的出勤情況 SQL
                foreach (string bu1 in Lists)
                {
                    string bu = bu1;
                    var BZ = 0;
                    #region 編制人力
                    if (bu == "PM")
                    {
                        BZ = db.DepartmentCompilation.Where(u => u.department == "PM11" || u.department == "PM12" || u.department == "PM14" || u.department == "PM31").Sum(u => u.nums);

                    }

                    if (bu == "PQ")
                    {
                        BZ = db.DepartmentCompilation.Where(u => u.department == "PQ12" || u.department == "PQ13" || u.department == "PQ15").Sum(u => u.nums);

                    }
                    if (bu == "PE")
                    {
                        BZ = db.DepartmentCompilation.Where(u => u.department == "PE11" || u.department == "PE16" || u.department == "PE17").Sum(u => u.nums);

                    }
                    if (bu != "PQ" && bu != "PM" && bu != "PE")
                    {
                        BZ = db.DepartmentCompilation.Where(u => u.department == bu).Sum(u => u.nums);

                    }
                    #endregion
                    if (bu1 == "PQ")
                    {
                        bu = "PQ12' or   BU='PQ13' or BU='PQ15";
                    }
                    if (bu1 == "PE")
                    {
                        bu = "PE11' or   BU='PE16' or BU='PE17";
                    }
                    string start1 = start.ToString("yyyy/MM/dd");
                    string sql = @"select ayd,bsd,bcd,bqj,bkg,blz,bqt,zsb,zzt,zqj,ztx,ybs,ybr,sbs,sbr
                    from 
                    (select COUNT(*) as ayd from  staff where (BU='" + bu + @"'))a,
                    (select count(*) bsd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state1='到' and (BU='" + bu + @"'))b,
                    (select count(*) bcd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='遲到'and (BU='" + bu + @"'))c,
                    (select count(*) bqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='請假' and (BU='" + bu + @"'))d,
                    (select count(*) bkg from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='曠工' and (BU='" + bu + @"'))e,
                    (select count(*) blz from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='離職' and (BU='" + bu + @"'))f,
                    (select count(*) bqt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1!='遲到' and reason1!='請假' and reason1!='曠工' and reason1!='離職' and (BU='" + bu + @"'))g,
                    (select count(*) zsb from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state2='中途來了' and (BU='" + bu + @"'))h,
                    (select count(*) zzt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='早退' and (BU='" + bu + @"'))i,
                    (select count(*) zqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='請假' and (BU='" + bu + @"'))j,
                    (select count(*) ztx from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='調休' and (BU='" + bu + @"'))k,
                    (select sum(time1)  ybs  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and (BU='" + bu + @"'))l,
                    (select count(time1)  ybr  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and time1>0 and (BU='" + bu + @" '))m,
                    (select sum(WorkTime)  sbs  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and (BU='" + bu + @"'))n,
                    (select count(WorkTime)  sbr  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and WorkTime>0 and (BU='" + bu + @"'))o";
                    DataTable dt = SQLHelper.GetTable(sql, CommandType.Text);
                    QCdetail qcdetail_ = new QCdetail();
                    qcdetail_.BUID = bu1;
                    qcdetail_.bz = BZ;
                    qcdetail_.yd = int.Parse(dt.Rows[0][0].ToString());
                    qcdetail_.sd = int.Parse(dt.Rows[0][1].ToString());
                    qcdetail_.cd = int.Parse(dt.Rows[0][2].ToString());
                    qcdetail_.qj = int.Parse(dt.Rows[0][3].ToString());
                    qcdetail_.kg = int.Parse(dt.Rows[0][4].ToString());
                    qcdetail_.lz = int.Parse(dt.Rows[0][5].ToString());
                    qcdetail_.qt = int.Parse(dt.Rows[0][6].ToString());
                    qcdetail_.zsb = int.Parse(dt.Rows[0][7].ToString());
                    qcdetail_.zzt = int.Parse(dt.Rows[0][8].ToString());
                    qcdetail_.zqj = int.Parse(dt.Rows[0][9].ToString());
                    qcdetail_.ztx = int.Parse(dt.Rows[0][10].ToString());
                    if (dt.Rows[0][11] != null && dt.Rows[0][11].ToString() != "")
                        qcdetail_.time1 = double.Parse(dt.Rows[0][11].ToString());
                    qcdetail_.timeOne1 = int.Parse(dt.Rows[0][12].ToString());
                    if (dt.Rows[0][13] != null && dt.Rows[0][13].ToString() != "")
                        qcdetail_.time2 = double.Parse(dt.Rows[0][13].ToString());
                    qcdetail_.timeOne2 = int.Parse(dt.Rows[0][14].ToString());
                  
                    if (qcdetail_.time1 != 0)
                        qcdetail_.timeOne = Math.Round(((qcdetail_.time1 - qcdetail_.time2) / qcdetail_.time1 * 100),2).ToString() + "%"; 
                    else
                        qcdetail_.timeOne = "0%";
                    qcdetails.Add(qcdetail_);

                }
                #endregion

            }
            ViewData["qcdetail"] = qcdetails;
            ViewData["date_int"] = id;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");


            return View();
        }
        //全廠白晚班明細
        public ActionResult QCdetailBW(string id)
        {
            #region  查詢時間 設置
            if (id == null) id = "0";
            DateTime now = DateTime.Now.AddDays(int.Parse(id));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                    id = (now - DateTime.Now).Days.ToString();
                };

            }
            #endregion

            #region  獲取全廠的出勤
            List<QCdetail> qcdetails = new List<QCdetail>();

            //有多少線
            var Lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => true).OrderBy(u => u.OrderbBy).Select(u => u.type1).ToList();
            //下面的四個部門統稱為 PM 要做特殊添加
            Lists.Add("PM11"); Lists.Add("PM12"); Lists.Add("PM14"); Lists.Add("PM31");
            Lists = Lists.Distinct().ToList();
            //編制人力
            GigabyteFrameEntities db = new GigabyteFrameEntities();
            var bz = db.DepartmentCompilation.Where(u => Lists.Contains(u.department)).Sum(u => u.nums);

            //獲取全廠的出勤
            var BZ = int.Parse(bz.ToString());
            //分白晚班
            string class_ = "";
            for (int x = 0; x < 2; x++)
            {
                if (x == 0)
                {
                    class_ = "白班";
                }
                if (x == 1)
                {
                    class_ = "晚班";
                }

                #region 數據查詢
                /**
                var yd = attendanceService.DbSession.StaffDal.GetEntity(u => u.CLASS==class_).Count();
                //獲取今天出去明細
                var tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end &&u.CLASS==class_);
                var sd = tatol.Where(u => u.state1 == "到").Count();
                //缺勤狀況
                var cd = tatol.Where(u => u.reason1 == "遲到").Count();
                var qj = tatol.Where(u => u.reason1 == "請假").Count();
                var kg = tatol.Where(u => u.reason1 == "曠工").Count();
                var lz = tatol.Where(u => u.reason1 == "離職").Count();
                var qt = tatol.Where(u => u.state1 != "到" && u.reason1 != "遲到" && u.reason1 != null && u.reason1 != ""
                        && u.reason1 != "請假" && u.reason1 != "曠工" && u.reason1 != "離職").Count();
                //中途表動
                var zsb = tatol.Where(u => u.state2 == "中途上班").Count();
                var zzt = tatol.Where(u => u.reason2 == "早退").Count();
                var zqj = tatol.Where(u => u.reason2 == "請假").Count();
                var ztx = tatol.Where(u => u.reason2 == "調休").Count();
                //預報，實際 加班
                var time1 = tatol.Where(u => u.time1 != null).
                    Sum(u => u.time1);
                var timeOne1 = tatol.Where(u => u.time1 > 0).Count();
                //實際加班
                var time2 = DB1.FactWorkTimeR.Where(u => u.date_ == start &&u.@class==class_).Sum(u => u.WorkTime).ToString();
                var timeOne2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == class_ && u.WorkTime > 0).Count();
                //預報/實際比
                var timeOne = 0;
           
                QCdetail qcdetail = new QCdetail();
                qcdetail.BUID = "全廠"+class_;
                qcdetail.bz = BZ; qcdetail.yd = yd;
                qcdetail.sd = sd;
                qcdetail.cd = cd; qcdetail.qj = qj; qcdetail.kg = kg; qcdetail.lz = lz; qcdetail.qt = qt;
                qcdetail.zsb = zsb; qcdetail.zzt = zzt; qcdetail.zqj = zqj; qcdetail.ztx = ztx;

                if (time1 != null) qcdetail.time1 = double.Parse(time1.ToString());
                qcdetail.timeOne1 = timeOne1;
                if (time2 != null && time2 != "") qcdetail.time2 = double.Parse(time2.ToString());
               qcdetail.timeOne2 = timeOne2;

                qcdetail.timeOne = timeOne;
                qcdetails.Add(qcdetail);
    **/
                #endregion
                #region 獲取全廠明細 SQL
                string start1 = start.ToString("yyyy/MM/dd");
                string sql = @"select ayd,bsd,bcd,bqj,bkg,blz,bqt,zsb,zzt,zqj,ztx,ybs,ybr,sbs,sbr
                    from 
                    (select COUNT(*) as ayd from  staff where CLASS='" + class_ + @"')a,
                    (select count(*) bsd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state1='到' and  CLASS='" + class_ + @"')b,
                    (select count(*) bcd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='遲到' and  CLASS='" + class_ + @"')c,
                    (select count(*) bqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='請假' and  CLASS='" + class_ + @"')d,
                    (select count(*) bkg from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='曠工' and  CLASS='" + class_ + @"')e,
                    (select count(*) blz from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='離職' and  CLASS='" + class_ + @"')f,
                    (select count(*) bqt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1!='遲到' and reason1!='請假' and reason1!='曠工' and reason1!='離職' and  CLASS='" + class_ + @"' )g,
                    (select count(*) zsb from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state2='中途來了' and  CLASS='" + class_ + @"')h,
                    (select count(*) zzt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='早退' and  CLASS='" + class_ + @"')i,
                    (select count(*) zqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='請假' and  CLASS='" + class_ + @"')j,
                    (select count(*) ztx from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='調休' and  CLASS='" + class_ + @"')k,
                    (select sum(time1)  ybs  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and  CLASS='" + class_ + @"')l,
                    (select count(time1)  ybr  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and time1>0  and  CLASS='" + class_ + @"')m,
                    (select sum(WorkTime)  sbs  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"'  and  CLASS='" + class_ + @"')n,
                    (select count(WorkTime)  sbr  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and WorkTime>0  and  CLASS='" + class_ + @"')o";
                DataTable dt = SQLHelper.GetTable(sql, CommandType.Text);
                QCdetail qcdetail = new QCdetail();
                qcdetail.BUID = "全廠" + class_;
                qcdetail.bz = BZ;
                qcdetail.yd = int.Parse(dt.Rows[0][0].ToString());
                qcdetail.sd = int.Parse(dt.Rows[0][1].ToString());
                qcdetail.cd = int.Parse(dt.Rows[0][2].ToString());
                qcdetail.qj = int.Parse(dt.Rows[0][3].ToString());
                qcdetail.kg = int.Parse(dt.Rows[0][4].ToString());
                qcdetail.lz = int.Parse(dt.Rows[0][5].ToString());
                qcdetail.qt = int.Parse(dt.Rows[0][6].ToString());
                qcdetail.zsb = int.Parse(dt.Rows[0][7].ToString());
                qcdetail.zzt = int.Parse(dt.Rows[0][8].ToString());
                qcdetail.zqj = int.Parse(dt.Rows[0][9].ToString());
                qcdetail.ztx = int.Parse(dt.Rows[0][10].ToString());
                if (dt.Rows[0][11] != null && dt.Rows[0][11].ToString() != "")
                    qcdetail.time1 = double.Parse(dt.Rows[0][11].ToString());
                qcdetail.timeOne1 = int.Parse(dt.Rows[0][12].ToString());
                if (dt.Rows[0][13] != null && dt.Rows[0][13].ToString() != "")
                    qcdetail.time2 = double.Parse(dt.Rows[0][13].ToString());
                qcdetail.timeOne2 = int.Parse(dt.Rows[0][14].ToString());
             
                if (qcdetail.time1 != 0)
                    qcdetail.timeOne = Math.Round(((qcdetail.time1 - qcdetail.time2) / qcdetail.time1 * 100),2).ToString() + "%";
                else
                    qcdetail.timeOne = "0%";
                qcdetails.Add(qcdetail);
                #endregion
            }


            #endregion

            ViewData["qcdetail"] = qcdetails;
            ViewData["date_int"] = id;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");
            return View();
        }

        //個部門明細
        public ActionResult BUdetail(string id)
        {
            List<BUdetail> list_budetails = new List<BUdetail>();
            List<BUdetail> list_budetail_total = new List<BUdetail>();
            QCdetail qcdetail = new QCdetail();
            GigabyteFrameEntities db = new GigabyteFrameEntities();

            string bu = id;//部門
            string CLASS = "";//班別
            string id1 = "0";//上下天加的數
            if (id1 == null) id1 = "0";
            string[] uid = id.ToString().Split(',');
            if (uid.Count() == 2)
            {
                bu = uid[0];
                id1 = uid[1];
            }

            #region  上一天，下一天 搜索 查詢時間 設置


            DateTime now = DateTime.Now.AddDays(int.Parse(id1));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索時間設置
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                    id1 = (now - DateTime.Now).Days.ToString();
                };

            }
            #endregion

            #region 部門狀況 EF
            /**
            //查詢出今天出勤
            var tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end && u.BU == bu);
            if (bu == "PM")
            {
                 tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end && u.BU.Contains(bu));
            }
            if (bu == "PQ")
            {
                tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end && (u.BU == "PQ12" || u.BU == "PQ13" || u.BU =="PQ15"));
            }
            if (bu == "PE")
            {
                tatol = attendanceService.GetEntity(u => u.date1 > start && u.date1 < end && (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17"));
            }
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) { CLASS = "白班"; } else { CLASS = "晚班"; }
                #region  總出勤狀況             
                var YD1 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU == bu && u.CLASS == CLASS).Count();
                if (bu == "PM")
                {
                    YD1 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU.Contains(bu) && u.CLASS == CLASS).Count();
                }
                if (bu == "PQ")
                {
                    YD1 = attendanceService.DbSession.StaffDal.GetEntity(u => (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15") && u.CLASS == CLASS).Count();
                }
                if (bu == "PE")
                {
                    YD1 = attendanceService.DbSession.StaffDal.GetEntity(u => (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17") && u.CLASS == CLASS).Count();
                }
                if (YD1 == 0) { continue; }
                var SD1 = tatol.Where(u => u.state1 == "到" && u.CLASS == CLASS).Count();
                var cd1 = tatol.Where(u => u.reason1 == "遲到" && u.CLASS == CLASS).Count();
                var qj1 = tatol.Where(u => u.reason1 == "請假" && u.CLASS == CLASS).Count();
                var kg1 = tatol.Where(u => u.reason1 == "曠工" && u.CLASS == CLASS).Count();
                var lz1 = tatol.Where(u => u.reason1 == "離職" && u.CLASS == CLASS).Count();
                var zsb1 = tatol.Where(u => u.state2 == "中途上班" && u.CLASS == CLASS).Count();
                var zzt1 = tatol.Where(u => u.reason2 == "早退" && u.CLASS == CLASS).Count();
                var zqj1 = tatol.Where(u => u.reason2 == "請假" && u.CLASS == CLASS).Count();
                var zkg1 = tatol.Where(u => u.reason2 == "調休" && u.CLASS == CLASS).Count();
                var time1 = tatol.Where(u => u.time1 != null && u.CLASS == CLASS).Sum(u => u.time1);
                var qt1 = tatol.Where(u => u.reason1 != "曠工" && u.reason1 != "遲到" && u.reason1 != "請假" && u.reason1 != "離職"  && u.reason1 != "" && u.reason1 != null && u.CLASS == CLASS ).Count();            
                var timeOne = tatol.Where(u => u.time1 > 0 && u.CLASS == CLASS).Count();
                //實際加班
                var time2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && u.bu == bu).Sum(u => u.WorkTime).ToString();
                var timeOne2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && u.bu == bu && u.WorkTime > 0).Count();
                //預報/實際比
                var timeOne12 = 0;
                if (bu == "PQ")
                {
                     time2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15")).Sum(u => u.WorkTime).ToString();
                     timeOne2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15") && u.WorkTime > 0).Count();
                    //預報/實際比
                     timeOne12 = 0;
                }
                if (bu == "PE")
                {
                     time2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17")).Sum(u => u.WorkTime).ToString();
                     timeOne2 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17") && u.WorkTime > 0).Count();
                    //預報/實際比
                     timeOne12 = 0;
                }

                BUdetail BUdetail = new BUdetail();

                BUdetail.BUID = bu; BUdetail.CLASS = CLASS; BUdetail.YD = YD1;
                BUdetail.SD = SD1; BUdetail.cd = cd1; BUdetail.qj = qj1; BUdetail.kg = kg1; BUdetail.lz = lz1;
                BUdetail.zsb = zsb1; BUdetail.zzt = zzt1; BUdetail.zqj = zqj1; BUdetail.zkg = zkg1;
                if (time1 != null) BUdetail.time1 = double.Parse(time1.ToString());
                if (bu != "PE")
                    BUdetail.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => u.type1 == bu).id;
                else
                    BUdetail.BUIDID = 2;
                BUdetail.qt = qt1; BUdetail.timeOne = timeOne;
                if (time2 != null && time2 != "")
                    BUdetail.time2 = double.Parse(time2);
                else
                    BUdetail.time2 = 0;
                BUdetail.timeOne2 = timeOne2; BUdetail.timeOne12 = timeOne12;
                list_budetail_total.Add(BUdetail);
                #endregion
            }
            // 查詢線別

            var lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == bu)
                                       .OrderBy(u => u.linename).Select(u => u.linename);
            if (bu == "PQ")
            {
                lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == "PQ12" || u.type1 == "PQ13" || u.type1 == "PQ15")
                                       .OrderBy(u => u.linename).Select(u => u.linename);
            }
            if (bu == "PE")
            {
                lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == "PE11" || u.type1 == "PE16" || u.type1 == "PE17")
                                       .OrderBy(u => u.linename).Select(u => u.linename);
            }
            bool isfirst = true;
                if (lists.Count() == 1) { isfirst=false; }
            if (isfirst)
            {
                #region  個線出勤狀況
                foreach (string linename in lists)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (i == 0) { CLASS = "白班"; } else { CLASS = "晚班"; }
                        var YD2 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU == bu && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        if (bu == "PQ")
                        {
                            YD2 = attendanceService.DbSession.StaffDal.GetEntity(u => (u.BU == "PQ12" || u.BU == "PQ13"|| u.BU == "PQ15") && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        }
                        if (bu == "PE")
                        {
                            YD2 = attendanceService.DbSession.StaffDal.GetEntity(u => (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17") && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        }
                        if (bu == "PM")
                        {
                            YD2 = attendanceService.DbSession.StaffDal.GetEntity(u => u.BU.Contains(bu) && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        }
                      
                        var SD2 = tatol.Where(u => u.state1 == "到" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var cd2 = tatol.Where(u => u.reason1 == "遲到" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var qj2 = tatol.Where(u => u.reason1 == "請假" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var kg2 = tatol.Where(u => u.reason1 == "曠工" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var lz2 = tatol.Where(u => u.reason1 == "離職" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var zsb2 = tatol.Where(u => u.state2 == "中途上班" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var zzt2 = tatol.Where(u => u.reason2 == "早退" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var zqj2 = tatol.Where(u => u.reason2 == "請假" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var zkg2 = tatol.Where(u => u.reason2 == "調休" && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var time2 = tatol.Where(u => u.time1 != null && u.CLASS == CLASS && u.LINENAME == linename).Sum(u => u.time1);
                        var qt2 = tatol.Where(u => u.reason1 != "曠工" && u.reason1 != "遲到" && u.reason1 != "請假" && u.reason1 != "離職" && u.reason1 != "" && u.reason1 != null && u.CLASS == CLASS && u.LINENAME == linename).Count();
                        var timeOne2 = tatol.Where(u => u.time1 > 0 &&  u.CLASS == CLASS && u.LINENAME == linename).Count();
                        //實際加班
                        var time22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && u.bu == bu &&u.linename== linename).Sum(u => u.WorkTime).ToString();
                        var timeOne22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && u.bu == bu && u.linename == linename  && u.WorkTime > 0).Count();
                        //預報/實際比
                        var timeOne112 = 0;
                        if (bu == "PQ")
                        {
                            time22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15") && u.linename == linename).Sum(u => u.WorkTime).ToString();
                            timeOne22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PQ12" || u.bu == "PQ13" || u.bu == "PQ15") && u.WorkTime > 0 && u.linename == linename).Count();
                            //預報/實際比
                            timeOne112 = 0;
                        }
                        if (bu == "PE")
                        {
                            time22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17") && u.linename == linename).Sum(u => u.WorkTime).ToString();
                            timeOne22 = DB1.FactWorkTimeR.Where(u => u.date_ == start && u.@class == CLASS && (u.bu == "PE11" || u.bu == "PE16" || u.bu == "PE17") && u.WorkTime > 0 && u.linename == linename).Count();
                            //預報/實際比
                            timeOne112 = 0;
                        }

                        BUdetail BUdetail2 = new BUdetail();

                        BUdetail2.BUID = linename; BUdetail2.CLASS = CLASS; BUdetail2.YD = YD2;
                        BUdetail2.SD = SD2; BUdetail2.cd = cd2; BUdetail2.qj = qj2; BUdetail2.kg = kg2; BUdetail2.lz = lz2;
                        BUdetail2.zsb = zsb2; BUdetail2.zzt = zzt2; BUdetail2.zqj = zqj2; BUdetail2.zkg = zkg2;
                        BUdetail2.qt = qt2; BUdetail2.timeOne = timeOne2;
                        if (time2 != null) BUdetail2.time1 = double.Parse(time2.ToString());
                        if (time22 != null && time22 != "")
                            BUdetail2.time2 = double.Parse(time22);
                        else
                            BUdetail2.time2 = 0;
                        BUdetail2.timeOne2 = timeOne22; BUdetail2.timeOne12 = timeOne112;
                        //if (YD2 != 0)
                        //{
                        //    list_budetails.Add(BUdetail2);
                        //}

                        //BUdetail2.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => u.type1 == bu && u.linename == linename).id;
                        if (bu == "PQ")
                        {
                            BUdetail2.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => (u.type1 == "PQ12" || u.type1 == "PQ13" || u.type1 == "PQ15") && u.linename == linename).id;
                        }
                        if (bu== "PE")
                        {
                            BUdetail2.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => (u.type1 == "PE11" || u.type1 == "PE16" || u.type1 == "PE17") && u.linename == linename).id;
                        }
                        if(bu!="PQ" &&bu!="PE" )
                        {
                            BUdetail2.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => u.type1 == bu && u.linename == linename).id;
                        }
                        if (YD2 != 0)
                        {
                            list_budetails.Add(BUdetail2);
                        }
                    }

                }
                #endregion
            }
    **/
            #endregion

            #region 部門出勤狀況 SQL

            #region 部門總
            for (int i = 0; i < 2; i++)
            {
                if (i == 0) { CLASS = "白班"; } else { CLASS = "晚班"; }
                string start1 = start.ToString("yyyy/MM/dd");
                if (bu == "PQ")
                {
                    bu = "PQ12' or   BU='PQ13' or BU='PQ15";
                }
                if (bu == "PE")
                {
                    bu = "PE11' or   BU='PE16' or BU='PE17";
                }
                string sql = @"select ayd,bsd,bcd,bqj,bkg,blz,bqt,zsb,zzt,zqj,ztx,ybs,ybr,sbs,sbr
                    from 
                    (select COUNT(*) as ayd from  staff where (BU='" + bu + @"') and CLASS='" + CLASS + @"')a,
                    (select count(*) bsd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state1='到' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')b,
                    (select count(*) bcd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='遲到'and (BU='" + bu + @"') and CLASS='" + CLASS + @"')c,
                    (select count(*) bqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='請假' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')d,
                    (select count(*) bkg from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='曠工' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')e,
                    (select count(*) blz from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='離職' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')f,
                    (select count(*) bqt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1!='遲到' and reason1!='請假' and reason1!='曠工' and reason1!='離職' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')g,
                    (select count(*) zsb from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state2='中途來了' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')h,
                    (select count(*) zzt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='早退' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')i,
                    (select count(*) zqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='請假' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')j,
                    (select count(*) ztx from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='調休' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')k,
                    (select sum(time1)  ybs  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and (BU='" + bu + @"'))l,
                    (select count(time1)  ybr  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and time1>0 and (BU='" + bu + @" ') and CLASS='" + CLASS + @"')m,
                    (select sum(WorkTime)  sbs  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and (BU='" + bu + @"') and CLASS='" + CLASS + @"')n,
                    (select count(WorkTime)  sbr  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and WorkTime>0 and (BU='" + bu + @"') and CLASS='" + CLASS + @"')o";

                DataTable dt = SQLHelper.GetTable(sql, CommandType.Text);
                BUdetail BUdetail = new BUdetail();
                if (bu == "PQ12' or   BU='PQ13' or BU='PQ15") bu = "PQ";
                if (bu == "PE11' or   BU='PE16' or BU='PE17") bu = "PE";
                BUdetail.BUID = bu;
                BUdetail.CLASS = CLASS;
                BUdetail.yd = int.Parse(dt.Rows[0][0].ToString());if (BUdetail.yd == 0) { continue; }
                BUdetail.sd = int.Parse(dt.Rows[0][1].ToString());
                BUdetail.cd = int.Parse(dt.Rows[0][2].ToString());
                BUdetail.qj = int.Parse(dt.Rows[0][3].ToString());
                BUdetail.kg = int.Parse(dt.Rows[0][4].ToString());
                BUdetail.lz = int.Parse(dt.Rows[0][5].ToString());
                BUdetail.qt = int.Parse(dt.Rows[0][6].ToString());
                BUdetail.zsb = int.Parse(dt.Rows[0][7].ToString());
                BUdetail.zzt = int.Parse(dt.Rows[0][8].ToString());
                BUdetail.zqj = int.Parse(dt.Rows[0][9].ToString());
                BUdetail.ztx = int.Parse(dt.Rows[0][10].ToString());
                if (dt.Rows[0][11] != null && dt.Rows[0][11].ToString() != "")
                    BUdetail.time1 = double.Parse(dt.Rows[0][11].ToString());
                BUdetail.timeOne1 = int.Parse(dt.Rows[0][12].ToString());
                if (dt.Rows[0][13] != null && dt.Rows[0][13].ToString() != "")
                    BUdetail.time2 = double.Parse(dt.Rows[0][13].ToString());
                BUdetail.timeOne2 = int.Parse(dt.Rows[0][14].ToString());
              
                if (BUdetail.time1 != 0)
                    BUdetail.timeOne12 = Math.Round(((BUdetail.time1 - BUdetail.time2) / BUdetail.time1 * 100),2).ToString() + "%";
                else
                    BUdetail.timeOne12 = "0%";
                list_budetail_total.Add(BUdetail);
            }
            #endregion

            #region 個線別
            var lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == bu)
                                      .OrderBy(u => u.linename).Select(u => u.linename);
            if (bu == "PQ")
            {
                lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == "PQ12" || u.type1 == "PQ13" || u.type1 == "PQ15")
                                       .OrderBy(u => u.linename).Select(u => u.linename);
            }
            if (bu == "PE")
            {
                lists = attendanceService.DbSession.LienNumberDal.GetEntity(u => u.type1 == "PE11" || u.type1 == "PE16" || u.type1 == "PE17")
                                       .OrderBy(u => u.linename).Select(u => u.linename);
            }
            foreach (string linename in lists)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0) { CLASS = "白班"; } else { CLASS = "晚班"; }
                    string start1 = start.ToString("yyyy/MM/dd");
                    string type1 = bu;
                    string bu1 = bu;
                    if (bu == "PQ")
                    {
                        bu1 = "PQ12' or   BU='PQ13' or BU='PQ15";
                        type1 = "PQ12' or   type1='PQ13' or type1='PQ15";
                    }
                    if (bu == "PE")
                    {
                        bu1 = "PE11' or   BU='PE16' or BU='PE17";
                        type1 = "PE11' or   type1='PE16' or type1='PE17";
                    }
                    string sql = @"select ayd,bsd,bcd,bqj,bkg,blz,bqt,zsb,zzt,zqj,ztx,ybs,ybr,sbs,sbr,buidid
                    from 
                    (select COUNT(*) as ayd from  staff where (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')a,
                    (select count(*) bsd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state1='到' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')b,
                    (select count(*) bcd from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='遲到'and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')c,
                    (select count(*) bqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='請假' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')d,
                    (select count(*) bkg from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='曠工' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')e,
                    (select count(*) blz from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1='離職' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')f,
                    (select count(*) bqt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason1!='遲到' and reason1!='請假' and reason1!='曠工' and reason1!='離職' and (BU='" + bu + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')g,
                    (select count(*) zsb from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and state2='中途來了' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')h,
                    (select count(*) zzt from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='早退' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')i,
                    (select count(*) zqj from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='請假' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')j,
                    (select count(*) ztx from Attendance   where convert(char(10),date1,111) ='" + start1 + @"'  and reason2='調休' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')k,
                    (select sum(time1)  ybs  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and (BU='" + bu1 + @"') and linename='" + linename + @"')l,
                    (select count(time1)  ybr  from WorkOvertime    where convert(char(10),date1,111) ='" + start1 + @"' and time1>0 and (BU='" + bu1 + @" ') and CLASS='" + CLASS + @"' and linename='" + linename + @"')m,
                    (select sum(WorkTime)  sbs  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"')n,
                    (select count(WorkTime)  sbr  from FactWorkTimeR    where convert(char(10),date_,111) ='" + start1 + @"' and WorkTime>0 and (BU='" + bu1 + @"') and CLASS='" + CLASS + @"' and linename='" + linename + @"' )o,
                    (select id as buidid  from LienNumber where (type1 = '" + type1 + @"') and  linename='" + linename + @"')p";
                    //BUdetail2.BUIDID = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => u.type1 == bu && u.linename == linename).id;
                    DataTable dt = SQLHelper.GetTable(sql, CommandType.Text);
                    BUdetail BUdetail = new BUdetail();
                    BUdetail.BUID = linename;
                    BUdetail.CLASS = CLASS;
                    BUdetail.BUIDID = int.Parse(dt.Rows[0][15].ToString());
                    BUdetail.yd = int.Parse(dt.Rows[0][0].ToString());
                    BUdetail.sd = int.Parse(dt.Rows[0][1].ToString());
                    BUdetail.cd = int.Parse(dt.Rows[0][2].ToString());
                    BUdetail.qj = int.Parse(dt.Rows[0][3].ToString());
                    BUdetail.kg = int.Parse(dt.Rows[0][4].ToString());
                    BUdetail.lz = int.Parse(dt.Rows[0][5].ToString());
                    BUdetail.qt = int.Parse(dt.Rows[0][6].ToString());
                    BUdetail.zsb = int.Parse(dt.Rows[0][7].ToString());
                    BUdetail.zzt = int.Parse(dt.Rows[0][8].ToString());
                    BUdetail.zqj = int.Parse(dt.Rows[0][9].ToString());
                    BUdetail.ztx = int.Parse(dt.Rows[0][10].ToString());
                    if (dt.Rows[0][11] != null && dt.Rows[0][11].ToString() != "")
                        BUdetail.time1 = double.Parse(dt.Rows[0][11].ToString());
                    BUdetail.timeOne1 = int.Parse(dt.Rows[0][12].ToString());
                    if (dt.Rows[0][13] != null && dt.Rows[0][13].ToString() != "")
                        BUdetail.time2 = double.Parse(dt.Rows[0][13].ToString());
                    BUdetail.timeOne2 = int.Parse(dt.Rows[0][14].ToString());
                  
                    if (BUdetail.time1 != 0)
                        BUdetail.timeOne12 =  Math.Round(((BUdetail.time1 - BUdetail.time2) / BUdetail.time1 * 100),2).ToString() + "%";
                    else
                        BUdetail.timeOne12 = "0%";
                    if (BUdetail.yd == 0) continue;
                    list_budetails.Add(BUdetail);
                }
            }
            #endregion

            #endregion


            ViewData["date_int"] = id1;
            ViewData["BU"] = bu;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");
            ViewData["list_budetails"] = list_budetails;
            ViewData["list_budetail_total"] = list_budetail_total;

            return View();
        }

        //線長明細
        public ActionResult xzdetail2(string id)
        {
            List<xzdetail> list_xzdetail = new List<xzdetail>();
            if (id == null) id = "0";
            string[] uid = id.Split(',');
            string UserBU1 = "";
            string UserLINENAME1 = "";
            string UserCLASS1 = "";

            if (id == null) id = "0";
            if (uid.Count() == 4)
            {
                UserBU1 = uid[0];
                UserLINENAME1 = uid[1];
                UserCLASS1 = uid[2] == "b" ? "白班" : "晚班";

                id = uid[3];

            }
            int lineID = int.Parse(UserLINENAME1);
            ViewData["UserLINENAME1"] = UserLINENAME1;
            UserLINENAME1 = attendanceService.DbSession.LienNumberDal.FirstOrDefault(u => u.id == lineID).linename;
            //查詢今日出勤明細 
            DateTime now = DateTime.Now.AddDays(int.Parse(id));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                    id = (now - DateTime.Now).Days.ToString();
                };
            }

            var temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => u.BU == UserBU1 && u.LINENAME == UserLINENAME1 && u.CLASS == UserCLASS1 && u.date1 > start && u.date1 < end);
            if (UserBU1 == "PQ")
            {
                temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15") && u.LINENAME == UserLINENAME1 && u.CLASS == UserCLASS1 && u.date1 > start && u.date1 < end);
            }
            if (UserBU1 == "PE")
            {
                temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => (u.BU == "PE11" || u.BU == "PE16" || u.BU == "PE17") && u.LINENAME == UserLINENAME1 && u.CLASS == UserCLASS1 && u.date1 > start && u.date1 < end);
            }
            foreach (Attendance ad in temp)
            {
                xzdetail dt = new xzdetail();
                dt.ID = ad.ID; dt.NAME = ad.NAME;
                if (ad.state1 == "到") { dt.cq = true; } else { dt.cq = false; }
                if (ad.state1 == "到") { dt.cq = true; }
                if (ad.reason1 == "遲到") { dt.cd = "遲到"; }
                if (ad.reason1 == "請假") { dt.qj = "請假"; }
                if (ad.reason1 == "曠工") { dt.kg = "曠工"; }
                if (ad.reason1 == "離職") { dt.lz = "離職"; }
                if (ad.reason1 != "曠工" && ad.reason1 != "請假" && ad.reason1 != "遲到" && ad.reason1 != "離職") { dt.qt = ad.reason1; }
                if (ad.state2 == "中途上班") { dt.zsb = "中途上班"; }
                if (ad.reason2 == "早退") { dt.zzt = "早退"; }
                if (ad.reason2 == "請假") { dt.zqj = "請假"; }
                if (ad.reason2 == "調休") { dt.ztx = "調休"; }
                if (ad.time1 != null) { dt.time1 = double.Parse(ad.time1.ToString()); }
                var time2 = DB1.FactWorkTimeR.FirstOrDefault(u => u.date_ == start && u.id == ad.ID);
                if (time2 != null)
                    dt.time2 = double.Parse(time2.WorkTime.ToString());
                list_xzdetail.Add(dt);
            }
            ViewData["list_xzdetail"] = list_xzdetail;
            ViewData["date_int"] = id;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");
            ViewData["UserBU1"] = UserBU1;

            ViewData["UserCLASS1"] = UserCLASS1;
            return View();
        }



        //線長點名可看的明細
        public ActionResult xzdetail(string id)
        {
            List<xzdetail> list_xzdetail = new List<xzdetail>();

            //查詢今日出勤明細 
            if (id == null) id = "0";
            DateTime now = DateTime.Now.AddDays(int.Parse(id));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                };


            }

            var temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 > start && u.date1 < end);
            foreach (Attendance ad in temp)
            {
                xzdetail dt = new xzdetail();
                dt.ID = ad.ID; dt.NAME = ad.NAME;
                if (ad.state1 == "到") { dt.cq = true; }
                if (ad.reason1 == "遲到") { dt.cd = "遲到"; }
                if (ad.reason1 == "請假") { dt.qj = "請假"; }
                if (ad.reason1 == "曠工") { dt.kg = "曠工"; }
                if (ad.reason1 == "離職") { dt.lz = "離職"; }
                if (ad.reason1 != "曠工" && ad.reason1 != "請假" && ad.reason1 != "遲到" && ad.reason1 != "離職") { dt.qt = ad.reason1; }
                if (ad.state2 == "中途上班") { dt.zsb = "中途上班"; }
                if (ad.reason2 == "早退") { dt.zzt = "早退"; }
                if (ad.reason2 == "請假") { dt.zqj = "請假"; }
                if (ad.reason2 == "調休") { dt.ztx = "調休"; }
                if (ad.time1 != null) { dt.time1 = double.Parse(ad.time1.ToString()); }
                list_xzdetail.Add(dt);
            }
            ViewData["list_xzdetail"] = list_xzdetail;
            ViewData["date_int"] = id;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");
            return View();
        }

        //缺勤明細
        public ActionResult qqdetail(string id)
        {

            List<qqdetail> list_qqdetail = new List<qqdetail>();
            if (id == null) id = "0";
            string[] uid = id.Split(',');
            string UserBU1 = "";

            if (id == null) id = "0";
            if (uid.Count() == 2)
            {
                UserBU1 = uid[0];
                id = uid[1];
            }
            //查詢今日出勤明細 
            DateTime now = DateTime.Now.AddDays(int.Parse(id));
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");

            //搜索
            if (Request.Form["RData"] != null && Request.Form["RData"].ToString().Length == 8)
            {
                if (common.IsDate(Request.Form["RData"].ToString()))
                {
                    string RData = Request.Form["RData"];
                    now = DateTime.ParseExact(RData, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    today = now.Date.ToShortDateString();
                    start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                    end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                    id = (now - DateTime.Now).Days.ToString();
                };
            }

            var temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => u.BU == UserBU1 && u.date1 > start && u.date1 < end && (u.state1 == "不到" || u.state2 == "中途上班" || u.state2 == "中途離開")).OrderBy(u => u.CLASS);
            if (UserBU1 == "PQ")
                temp = attendanceService.DbSession.AttendanceDal.GetEntity(u => (u.BU == "PQ12" || u.BU == "PQ13" || u.BU == "PQ15") && u.date1 > start && u.date1 < end && (u.state1 == "不到" || u.state2 == "中途上班" || u.state2 == "中途離開")).OrderBy(u => u.CLASS);
            foreach (Attendance ad in temp)
            {
                qqdetail dt = new qqdetail();
                dt.ID = ad.ID;
                dt.Name = ad.NAME;
                dt.Linename = ad.LINENAME;
                dt.Class = ad.CLASS;
                dt.reason = ad.reason1;
                if (ad.state2 != "出勤" && ad.state2 != "缺勤")
                { dt.ZTbd = ad.state2; }
                dt.ZTreason = ad.reason2;
                list_qqdetail.Add(dt);
            }
            ViewData["list_qqdetail"] = list_qqdetail;
            ViewData["date_int"] = id;
            ViewData["Date"] = now.ToString("yyyy/MM/dd");
            ViewData["bu"] = UserBU1;
            ViewData["UserLINENAME1"] = "";
            ViewData["UserCLASS1"] = "";
            return View();
        }


    }
}