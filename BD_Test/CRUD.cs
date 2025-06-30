using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace BD_Test
{
    public static class CRUD
    {
        public static void Add(Model model)
        {
            using(ApplicationContext db=new ApplicationContext())
            {
                db.Add(model);
                db.SaveChanges();
            }
        }

        public static void Delete(Model model)
        {
            using(ApplicationContext db=new ApplicationContext())
            {
                Model? models = db.DbSet.FirstOrDefault(b => b.id == model.id);
                if (models != null)
                {
                    db.DbSet.Remove(model);
                    db.SaveChanges();
                }
            }
        }

        public static void Update(Model model)
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                Model ? models=db.DbSet.FirstOrDefault(b=>b.id==model.id);
                if(models != null)
                {
                    models.author=model.author;
                    models.name=model.name;
                    db.SaveChanges();
                }
            }
        }

        public static void Read()
        {
            using(ApplicationContext db=new ApplicationContext())
            {
                var model = db.DbSet.ToList();
                Console.WriteLine("Данные после добввления");
                foreach(var item in model)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }
}
