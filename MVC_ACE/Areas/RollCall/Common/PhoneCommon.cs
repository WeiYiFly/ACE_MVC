using MVC_ACE.Areas.RollCall.Models;
using RollCall.BLL;
using RollCall.DALFactory;
using RollCall.IBLL;
using RollCall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.RollCall.Common
{
    public class PhoneCommon
    {
        public AttendanceService attendanceService = new AttendanceService();
        public WorkOvertimeService workOvertimeService = new WorkOvertimeService();
        public IStaffService staffService = new StaffService();
        DbSession dbSession = new DbSession();


        /// <summary>
        /// 判斷有沒有點過名
        /// </summary>
        /// <param name="BU">部門</param>
        /// <param name="LINENAME">線別</param>
        /// <param name="CLASS">班別</param>
        /// <returns></returns>
        public List<CallName> JudgeCallName(string BU, string LINENAME, string CLASS)
        {
            List<CallName> CallName_Models = new List<CallName>();
            DateTime now = DateTime.Now;
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
            //string now1 = DateTime.Now.Date.ToString("yyyyMMdd");
            var temp2 = attendanceService.CurrentDal.GetEntity(u => u.date1 > start && u.date1 < end && u.BU == BU && u.LINENAME == LINENAME && u.CLASS == CLASS);
            foreach (Attendance ad in temp2)
            {
                CallName cn = new CallName();
                cn.ID = ad.ID;
                cn.NAME = ad.NAME;
                cn.STATE = ad.state1;
                CallName_Models.Add(cn);
            }
            return CallName_Models;
        }

        /// <summary>
        /// 判斷有沒有預報加班
        /// </summary>
        /// <param name="BU"></param>
        /// <param name="LINENAME"></param>
        /// <param name="CLASS"></param>
        /// <returns></returns>
        public bool JudgeAddwork(string BU, string LINENAME, string CLASS)
        {
            bool flag = false;

            DateTime now = DateTime.Now.Date;
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
            //string now1 = DateTime.Now.Date.ToString("yyyyMMdd");
            var temp2 = workOvertimeService.CurrentDal.GetEntity(u => u.date1 == now && u.BU == BU && u.LINENAME == LINENAME && u.CLASS == CLASS);
            if (temp2.Count() > 0)
            { flag = true; }
            return flag;
        }

        /// <summary>
        /// 判斷有沒有預報加班
        /// </summary>
        /// <param name="BU"></param>
        /// <param name="LINENAME"></param>
        /// <param name="CLASS"></param>
        /// <returns></returns>
        public bool JudgeAddwork(string BU, string LINENAME, string CLASS, string t)
        {
            bool flag = false;

            DateTime now = DateTime.Now.AddDays(1).Date;
            string today = now.Date.ToShortDateString();
            DateTime start = Convert.ToDateTime(today.Trim() + " " + "00:00:00");
            DateTime end = Convert.ToDateTime(today.Trim() + " " + "23:59:59");
            //string now1 = DateTime.Now.Date.ToString("yyyyMMdd");
            var temp2 = workOvertimeService.CurrentDal.GetEntity(u => u.date1 == now && u.BU == BU && u.LINENAME == LINENAME && u.CLASS == CLASS);
            if (temp2.Count() > 0)
            { flag = true; }
            return flag;
        }


        /// <summary>
        /// 更具工號，加班時間數組 返回ListWorkOvertime
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time1"></param>
        /// <returns></returns>
        public List<WorkOvertime> GetWorkOvertimes(List<string> id, List<string> time1)
        {
            List<WorkOvertime> list_WorkOvertime = new List<WorkOvertime>();
            int number = 0;
            var temp = staffService.GetEntity(u => id.Contains(u.ID)).OrderBy(u => u.ID);
            foreach (Staff w in temp)
            {
                WorkOvertime work = new WorkOvertime();
                work.ID = w.ID;
                work.NAME = w.NAME;
                work.BU = w.BU;
                work.LINENAME = w.LINENAME;
                work.CLASS = w.CLASS;
                work.time1 = double.Parse(time1[number]);
                work.date1 = DateTime.Now.Date;
                work.date2 = DateTime.Now;
                number++;
                list_WorkOvertime.Add(work);
            }
            return list_WorkOvertime;
        }

        public List<WorkOvertime> GetWorkOvertimes(List<string> id, List<string> time6, List<string> time7)
        {
            List<WorkOvertime> list_WorkOvertime = new List<WorkOvertime>();
            int number = 0;
            var temp = staffService.GetEntity(u => id.Contains(u.ID)).OrderBy(u => u.ID);
            foreach (Staff w in temp)
            {
                WorkOvertime work6 = new WorkOvertime();
                WorkOvertime work7 = new WorkOvertime();
                work6.ID = w.ID;
                work6.NAME = w.NAME;
                work6.BU = w.BU;
                work6.LINENAME = w.LINENAME;
                work6.CLASS = w.CLASS;
                work6.time1 = double.Parse(time6[number]);
                work6.date1 = DateTime.Now.AddDays(1).Date;
                work6.date2 = DateTime.Now;
                work7.ID = w.ID;
                work7.NAME = w.NAME;
                work7.BU = w.BU;
                work7.LINENAME = w.LINENAME;
                work7.CLASS = w.CLASS;
                work7.time1 = double.Parse(time7[number]);
                work7.date1 = DateTime.Now.AddDays(2).Date;
                work7.date2 = DateTime.Now;
                number++;
                list_WorkOvertime.Add(work6);
                list_WorkOvertime.Add(work7);
            }
            return list_WorkOvertime;
        }

        public List<Attendance> Get_Attendance(List<string> ID, List<string> state)
        {
            List<Attendance> list_Attendance = new List<Attendance>();
            int count = 0;
            foreach (string id in ID)
            {
                var temp = dbSession.StaffDal.FirstOrDefault(u => u.ID == id);
                {
                    Attendance a = new Attendance();
                    a.ID = temp.ID;
                    a.NAME = temp.NAME;
                    a.BU = temp.BU;
                    a.LINENAME = temp.LINENAME;
                    a.CLASS = temp.CLASS;
                    a.state1 = state[count];
                    a.date1 = DateTime.Now;
                    //   a.date2 = null;a.note1 = null; a.note2 = null; a.reason2 = null;
                    list_Attendance.Add(a);
                    //dbContext.SaveChanges();
                }
                count++;
            }
            return list_Attendance;


        }
    }
}