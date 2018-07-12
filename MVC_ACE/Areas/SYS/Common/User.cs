using MVC_DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_ACE.Areas.SYS.Common
{
    public class User
    {
        ACE_MVCEntities Database = new ACE_MVCEntities();
        public int add(string ID, string Name, string Bu, string Email,string Tel,string Passw)
        {

            SYS_User User_table = new SYS_User();
            User_table.Id = ID;
            User_table.Name = Name;
            User_table.Bu = Bu;
            User_table.Email = Email;
            User_table.Tel = Tel;
            User_table.Passw = Passw;
            Database.SYS_User.Add(User_table);
            return Database.SaveChanges();
        }


        public int Update(string ID, string Name, string Bu, string Email, string Tel, string Passw)
        {
            var temp = Database.SYS_User.FirstOrDefault(u => u.Id == ID);
            temp.Id = ID;
            temp.Name = Name;
            temp.Bu = Bu;
            temp.Email = Email;
            temp.Tel = Tel;
            temp.Passw = Passw;
            return Database.SaveChanges();
        }
        public int del(string[] ID)
        {
            foreach (string a in ID)
            {
               // int id = int.Parse(a);
                var temp = Database.SYS_User.FirstOrDefault(u => u.Id == a);
                Database.SYS_User.Remove(temp);
            }
            return Database.SaveChanges();
        }
    }
}