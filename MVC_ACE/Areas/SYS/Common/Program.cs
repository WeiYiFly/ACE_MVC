using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.SYS.Common
{
    public class Program
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        public int add(string Cname,string Ename,string ID)
        {  
            SYS_Program Program_table = new SYS_Program();
            Program_table.ChinaName = Cname;
            Program_table.EnglishName = Ename;
            Program_table.CreatorID = ID;
            Program_table.CreatedTime = DateTime.Now;
            Database.SYS_Program.Add(Program_table);
           return Database.SaveChanges();
        }

        public int Update(int ID, string Cname, string Ename, string CID)
        {      
            var temp = Database.SYS_Program.FirstOrDefault(u => u.Id == ID);
               temp.ChinaName = Cname;
               temp.EnglishName = Ename;
               temp.CreatorID = CID;
               temp.CreatedTime = DateTime.Now;
            return Database.SaveChanges();
        }
        public int del(string [] ID)
        {
            foreach (string  a in ID)
            {
                int id = int.Parse(a);
                var temp= Database.SYS_Program.FirstOrDefault(u => u.Id == id);
                 Database.SYS_Program.Remove(temp);
            }
            return Database.SaveChanges();
        }


    }
}