using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.SYS.Common
{
    public class Module
    {
        ACE_MVCEntities DB = new ACE_MVCEntities();
        public int add(string Name, string Lv, string Conetroller, string View_, string Url, string Icon, string UpId, string ProgramId)
        {
            SYS_Module Module_table = new SYS_Module();
            Module_table.Name = Name;
            Module_table.Lv = Lv;
            Module_table.Controller = Conetroller;
            Module_table.View_ = View_;
            Module_table.Url = Url;
            Module_table.Icon = Icon;
            Module_table.UpId = int.Parse(UpId);
            Module_table.ProgramId = int.Parse(ProgramId);            
            DB.SYS_Module.Add(Module_table);
            return DB.SaveChanges();
        }

        public int Update(int ID,string Name, string Lv, string Conetroller, string View_, string Url, string Icon, string UpId, string ProgramId)
        {
            var temp = DB.SYS_Module.FirstOrDefault(u => u.Id == ID);
            temp.Name = Name;
            temp.Lv = Lv;
            temp.Controller = Conetroller;
            temp.View_ = View_;
            temp.Url = Url;
            temp.Icon = Icon;
            temp.UpId = int.Parse(UpId);
            temp.ProgramId = int.Parse(ProgramId);
            return DB.SaveChanges();
        }
        public int del(string[] ID)
        {
            foreach (string a in ID)
            {
                int id = int.Parse(a);
                var temp = DB.SYS_Module.FirstOrDefault(u => u.Id == id);
                DB.SYS_Module.Remove(temp);
            }
            return DB.SaveChanges();
        }

    }
}