using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollCall.UI.Commo
{
    public class DetailsCommon
    {
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public  bool IsDate(string date)
        {
            #region
            ////如果为空，认为验证合格
            //if (string.IsNullOrEmpty(date))
            //{
            //    return true;
            //}
            ////清除要验证字符串中的空格
            //date = date.Trim();
            ////替换\
            //date = date.Replace(@"\", "-");
            ////替换/
            //date = date.Replace(@"/", "-");
            ////如果查找到汉字"今",则认为是当前日期
            //if (date.IndexOf("今") != -1)
            #endregion

            #region 对纯数字进行解析
            //对8位纯数字进行解析
            if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }
                //拼接日期
                try
                {
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                }
                catch { return false; }
                    return true;
                }
                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }
                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }
                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }
                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);
                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion
                return false;
            
        }
    }
}