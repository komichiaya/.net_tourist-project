using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPro.Web.Models;

namespace TravelPro.Web.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            var db = new TraverBDEntities();
            List<Admin> list = db.Admins.ToList();

            return View(list);
        }
        
        public ActionResult QueryAdmin(string Admin) {
            var db = new TraverBDEntities();
            //where Mname like '%测试%'
            List<Admin> list = db.Admins.Where(m => m.Admin1.Contains(Admin)).ToList();

            return View("Index",list);

        }


        #region 新增
        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        public ActionResult SaveAddAdmin(Admin adminl)
        {
            var db = new TraverBDEntities();
            db.Admins.Add(adminl);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region 删除

        public ActionResult DeleAdmin(int id)
        {

            var db = new TraverBDEntities();
            Admin m = db.Admins.Where(s => s.Id == id).FirstOrDefault();

            db.Admins.Remove(m);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region 修改


        public ActionResult ModifyAdmin(int id)
        {
            var db = new TraverBDEntities();
            Admin m = db.Admins.Where(s => s.Id == id).FirstOrDefault();
            return View(m);

        }

        public ActionResult SaveModifyAdmin(Admin m)
        {
            var db = new TraverBDEntities(); 
            //附加对象
            db.Set<Admin>().Attach(m);
            //修改 对象状态
            db.Entry(m).State = EntityState.Modified;
            //保存
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion
    

    }
}
