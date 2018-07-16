using MVC_ACE.Areas.RollCall.Common;
using MVC_ACE.Areas.RollCall.Models;
using RollCall.BLL;
using RollCall.IBLL;
using RollCall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_ACE.Areas.RollCall.Controllers
{
    public class PhoneController : BasePhoneController
    {
        // GET: RollCall/Phone
       
            IStaffService staff = new StaffService();
            AttendanceService attendanceService = new AttendanceService();
            ISpecialCaseService specialService = new SpecialCaseService();
            WorkOvertimeService workOvertimeService = new WorkOvertimeService();

            PhoneCommon Common = new PhoneCommon();
            //考慮特殊情況  白晚班 都是他點名的情況
            public ActionResult CallNameSelectClass()
            {
                //判斷有沒有 一個線長 白晚班 都是他點名的情況
                UserID = Session["UserID"].ToString();
                var temp = specialService.GetEntity(u => u.ID == UserID && u.CODE == "A");
                if (temp.Count() == 0) return RedirectToAction("../Phone/Index");

                return View();
            }
            // GET: Phone
            public ActionResult Index(string id)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    UserCLASS = id == "AM" ? "白班" : "晚班";
                }

                return View();
            }

            #region 點名
            public ActionResult CallName(string id)
            {

                List<CallName> CallName_model = new List<CallName>();
                //線長信息
                string ID = UserID;
                if (string.IsNullOrEmpty(ID)) { return RedirectToAction("../Home/Login"); }
                var temp1 = staff.GetEntity(U => U.ID == ID);
                List<Staff> user = new List<Staff>();
                foreach (Staff a in temp1)
                {
                    user.Add(a);
                }
                ViewData["Users"] = user;
                //更改班別
                //if (!string.IsNullOrEmpty(id)) { UserCLASS = id=="AM"?"白班":"晚班"; } // id有值是特殊情況

                //判斷有沒有點過名          
                CallName_model = Common.JudgeCallName(UserBU, UserLINENAME, UserCLASS);
                if (CallName_model.Count() == 0)  //沒點名就抓staff 表中的數據 默認全到
                {
                    //線員信息
                    var temp = staff.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS);
                    foreach (Staff s in temp)
                    {
                        CallName cn = new CallName();
                        cn.ID = s.ID;
                        cn.NAME = s.NAME;
                        cn.STATE = "到";//默認
                        CallName_model.Add(cn);

                    }
                }
                int sd = 0;
                foreach (var a in CallName_model)
                {
                    if (a.STATE == "到")
                    {
                        sd++;
                    };
                }
                ViewData["names"] = CallName_model;
                ViewData["CLASS"] = UserCLASS;
                ViewData["YD"] = CallName_model.Count;
                ViewData["SD"] = sd;
                return View();
            }


            public ActionResult CallNameSubmit(string aa)
            {
                //獲取線人員名字
                List<Staff> names = new List<Staff>();
                aa = aa.TrimEnd(',');
                string[] uid = aa.Split(',');
                List<string> ID = new List<string>();
                List<string> State = new List<string>();

                foreach (string b in uid)
                {
                    string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                    if (bb.Count() > 1)
                    {
                        ID.Add(bb[0]); State.Add(bb[2]);
                    }
                }
                //UserCLASS = id;//實際班別
                //判斷有沒有點過名
                List<Attendance> List_Attendance = new List<Attendance>();
                List_Attendance = Common.Get_Attendance(ID, State);
                int number = Common.JudgeCallName(UserBU, UserLINENAME, UserCLASS).Count();
                if (number > 0)  //沒點名就抓staff 表中的數據 默認全到
                {
                    attendanceService.update_Attendance(ID, State, "state1");
                }
                else
                {
                    //attendanceService.add_Attendance(ID, State);
                    attendanceService.add_Attendance(List_Attendance);
                }


                return Content("OK");
            }

            #endregion

            #region  缺勤處理
            public ActionResult CallNameReason()
            {
                List<CallName> CallName_Models = new List<Models.CallName>();
                DateTime now = DateTime.Now;
                string today = now.Date.ToShortDateString();
                DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                var temp = attendanceService.CurrentDal.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 > start && u.date1 < end && u.state1 == "不到");
                foreach (Attendance ad in temp)
                {
                    CallName cn = new CallName();
                    cn.ID = ad.ID;
                    cn.NAME = ad.NAME;
                    cn.STATE = ad.reason1;
                    CallName_Models.Add(cn);
                }
                ViewData["CallName_Models"] = CallName_Models;
                return View();
            }

            public ActionResult CallNameReasonSubmit(string users)
            {
                users = users.TrimEnd(',');
                string[] uid = users.Split(',');
                List<string> ID = new List<string>();
                List<string> State = new List<string>();
                foreach (string b in uid)
                {
                    string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                    if (bb.Count() > 1)
                    {
                        ID.Add(bb[0]); State.Add(bb[1]);
                    }
                }
                attendanceService.update_Attendance(ID, State, "reason1");
                return Content("OK");
            }
            #endregion


            #region  中途人員變動
            public ActionResult ZTBD()
            {
                List<CallName> CallName_Models = new List<Models.CallName>();
                DateTime now = DateTime.Now;
                string today = now.Date.ToShortDateString();
                DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                var temp = attendanceService.CurrentDal.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 > start && u.date1 < end);
                foreach (Attendance ad in temp)
                {
                    CallName cn = new CallName();
                    cn.ID = ad.ID;
                    cn.NAME = ad.NAME;
                    if (ad.state2 == null)
                    {
                        cn.STATE = ad.state1 == "到" ? "出勤" : "缺勤";
                    }
                    else
                    {
                        cn.STATE = ad.state2;
                    }
                    CallName_Models.Add(cn);
                }
                ViewData["CallName_Models"] = CallName_Models;
                return View();
            }

            public ActionResult ZTBDSubmit(string aa, string id)
            {
                aa = aa.TrimEnd(',');
                string[] uid = aa.Split(',');
                List<string> ID = new List<string>();
                List<string> State = new List<string>();
                foreach (string b in uid)
                {
                    string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                    if (bb.Count() > 1)
                    {
                        ID.Add(bb[0]); State.Add(bb[1]);
                    }
                }
                if (id == "state2")
                    attendanceService.update_Attendance(ID, State, "state2");
                if (id == "reason2")
                    attendanceService.update_Attendance(ID, State, "reason2");
                return Content("OK");
            }
            public ActionResult ZTBDYY()
            {
                List<CallName> CallName_Models = new List<Models.CallName>();
                DateTime now = DateTime.Now;
                string today = now.Date.ToShortDateString();
                DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                var temp = attendanceService.CurrentDal.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 > start && u.date1 < end && u.state2 == "中途離開");
                foreach (Attendance ad in temp)
                {
                    CallName cn = new CallName();
                    cn.ID = ad.ID;
                    cn.NAME = ad.NAME;
                    if (ad.state2 == null)
                    {
                        cn.STATE = ad.state1 == "到" ? "出勤" : "缺勤";
                    }
                    else
                    {
                        cn.STATE = ad.reason2;
                    }
                    CallName_Models.Add(cn);
                }
                ViewData["CallName_Models"] = CallName_Models;

                return View();
            }

            #endregion

            #region 預報加班
            public ActionResult AddWork()
            {
                //獲取今天上班人員名字工號
                List<CallName> CallName_Models = new List<Models.CallName>();
                DateTime now = DateTime.Now;
                string today = now.Date.ToShortDateString();
                DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
                DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
                var temp = attendanceService.CurrentDal.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS &&
                                                                       u.date1 > start && u.date1 < end && (u.state1 == "到" || u.state2 == "中途上班")).OrderBy(u => u.ID);
                foreach (Attendance ad in temp)
                {
                    CallName cn = new CallName();
                    cn.ID = ad.ID;
                    cn.NAME = ad.NAME;
                    cn.STATE = ad.time1.ToString();
                    if (ad.time1 == null)
                    {
                        cn.STATE = "0";
                    }
                    CallName_Models.Add(cn);
                }
                ViewData["CallName"] = CallName_Models;
                return View();
            }

            public ActionResult AddWeekWork()
            {
                List<CallName> CallName_Models = new List<Models.CallName>();
                if (!Common.JudgeAddwork(UserBU, UserLINENAME, UserCLASS, "週末加班"))
                {
                    //獲取今天上班人員名字工號
                    var temp = staff.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS).OrderBy(u => u.ID);


                    foreach (Staff ad in temp)
                    {
                        CallName cn = new CallName();
                        cn.ID = ad.ID;
                        cn.NAME = ad.NAME;
                        cn.STATE = "0";
                        CallName_Models.Add(cn);
                    }
                    ViewData["CallName"] = CallName_Models;
                }
                else
                {
                    DateTime now6 = DateTime.Now.AddDays(1).Date;
                    DateTime now7 = DateTime.Now.AddDays(2).Date;
                    var temp6 = workOvertimeService.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 == now6).OrderBy(u => u.ID);
                    var temp7 = workOvertimeService.GetEntity(u => u.BU == UserBU && u.LINENAME == UserLINENAME && u.CLASS == UserCLASS && u.date1 == now7)
                                                                .OrderBy(u => u.ID).Select(u => u.time1).ToList();
                    int number = 0;
                    foreach (WorkOvertime wk in temp6)
                    {
                        CallName cn = new CallName();
                        cn.ID = wk.ID;
                        cn.NAME = wk.NAME;
                        cn.STATE = wk.time1.ToString();
                        cn.STATE1 = temp7[number].ToString();
                        number++;
                        CallName_Models.Add(cn);
                    }
                    ViewData["CallName"] = CallName_Models;
                }
                return View();
            }

            public ActionResult AddWorkSubmit(string aa, string id, string aa6, string aa7)
            {
                //週末加班
                if (id == "Week")
                {
                    aa6 = aa6.TrimEnd(',');
                    aa7 = aa7.TrimEnd(',');
                    string[] uid6 = aa6.Split(',');
                    string[] uid7 = aa7.Split(',');
                    List<string> ID = new List<string>();
                    List<string> State6 = new List<string>();
                    List<string> State7 = new List<string>();
                    List<WorkOvertime> List_WorkOvertimes = new List<WorkOvertime>();
                    foreach (string b in uid6)
                    {
                        string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                        if (bb.Count() > 1)
                        {
                            ID.Add(bb[0]); State6.Add(bb[1]);
                        }
                    }

                    foreach (string b in uid7)
                    {
                        string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                        if (bb.Count() > 1)
                        {
                            ID.Add(bb[0]); State7.Add(bb[1]);
                        }
                    }
                    //attendanceService.update_Attendance(ID, State, "time1");//更改出勤表的加班時間

                    List_WorkOvertimes = Common.GetWorkOvertimes(ID, State6, State7);
                    if (!Common.JudgeAddwork(UserBU, UserLINENAME, UserCLASS, "週末加班"))
                    {

                        workOvertimeService.addBatch(List_WorkOvertimes);
                    }
                    else
                    {
                        workOvertimeService.updateBatch(List_WorkOvertimes);
                    }



                }
                else//日常加班
                {
                    aa = aa.TrimEnd(',');
                    string[] uid = aa.Split(',');
                    List<string> ID = new List<string>();
                    List<string> State = new List<string>();
                    foreach (string b in uid)
                    {
                        string[] bb = System.Text.RegularExpressions.Regex.Split(b, @"\s{1,}");
                        if (bb.Count() > 1)
                        {
                            ID.Add(bb[0]); State.Add(bb[1]);
                        }
                    }
                    attendanceService.update_Attendance(ID, State, "time1");//更改出勤表的加班時間
                    List<WorkOvertime> List_WorkOvertimes = new List<WorkOvertime>();
                    List_WorkOvertimes = Common.GetWorkOvertimes(ID, State);

                    //條件 之前有無填預報加班 有的話更，沒有就添加
                    if (!Common.JudgeAddwork(UserBU, UserLINENAME, UserCLASS))
                    {

                        workOvertimeService.addBatch(List_WorkOvertimes);
                    }
                    else
                    {
                        workOvertimeService.updateBatch(List_WorkOvertimes);
                    }
                }
                return Content("OK");
            }

            #endregion


            public ActionResult Details()
            {
                return View();
            }


        }
    }
