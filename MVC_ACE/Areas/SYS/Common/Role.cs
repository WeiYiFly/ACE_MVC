using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.SYS.Common
{
    public class Role
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        public int add(int PrID,string Name, string Remark, string CID)
        {
            
            SYS_Role Role_table = new SYS_Role();
            Role_table.Name = Name;
            Role_table.ProgramId = PrID;
            Role_table.Remark = Remark;
            Role_table.CreatorID = CID;
            Role_table.CreatedTime = DateTime.Now;
            Database.SYS_Role.Add(Role_table);
            return Database.SaveChanges();
        }
       

        public int Update(int ID, int PrID, string Name, string Remark, string CID)
        {
            var temp = Database.SYS_Role.FirstOrDefault(u => u.Id == ID);
            temp.Name = Name;
            temp.ProgramId = PrID;
            temp.Remark = Remark;
            temp.CreatorID = CID;
            temp.CreatedTime = DateTime.Now;
            
            return Database.SaveChanges();
        }
        public int del(string[] ID)
        {
            foreach (string a in ID)
            {
                int id = int.Parse(a);
                var temp = Database.SYS_Role.FirstOrDefault(u => u.Id == id);
                Database.SYS_Role.Remove(temp);
            }
            return Database.SaveChanges();
        }
    }
}