using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPro.Web.Models;

namespace TravelPro.Web.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMenuPageData(int page = 1, int limit = 10)
        {
            List<Menu> mlist = new List<Menu>();
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();
            }
            var list = mlist.Skip((page - 1) * limit).Take(limit).Select(s => new
            {
                ID = s.ID,
                MenuName = s.MenuName,
                MenuUrl = s.MenuUrl,
                IsDele = s.IsDele,
                ImgUrl = s.ImgUrl,
                OrderBy = s.OrderBy,
                CreateUserId = s.CreateUserId,
                CreateTime = s.CreateTime

            }).ToList();

            var obj = new { code = 0, msg = "", count = list.Count, data = list };
            return Json(obj);
        }

        [HttpPost]
        public ActionResult DelMenu(string id)
        {
            int n = Convert.ToInt32(id);
            int i = 0;
            using (var db = new TraverBDEntities())
            {
                var temp = db.Menus.Where(s => s.ID == n).FirstOrDefault();
                db.Menus.Remove(temp);
                i = db.SaveChanges();
            }
            var obj = new { code = i };
            return Json(obj);
        }

        public ActionResult GetNavMenu()
        {

            List<Menu> list = null;
            using (var db = new TraverBDEntities())
            {
                list = db.Menus.Where(s => s.IsDele == 0).ToList();

            }

            return PartialView(list);
        }

        public ActionResult AddMenu()
        {
            List<Menu> mlist = new List<Menu>();
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();

            }


            return View(mlist);

        }
       

        [HttpPost]
        public ActionResult SaveMenu(HttpPostedFileBase file, Menu tb)
        {
            
            if (file == null)
            {
                return Json(new { result = "false", code = 400, msg = "文件不存在" }, "text/html");
            }

            //用于同名图片处理，设置物理路径
            string fileName = "~/CreaveFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
            var physicsFileName = Server.MapPath(fileName);
            try
            {
                //保存图片到对应目录
                file.SaveAs(physicsFileName);
                using (var db = new TraverBDEntities())
                {
                    tb.ImgUrl = fileName.Replace("~", "");
                    tb.CreateTime = DateTime.Now;
                    tb.CreateUserId = 1;
                    db.Menus.Add(tb);
                    db.SaveChanges();
                }
                return Json(new { result = "true", msg = "上传成功", imgUrl = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
            }
        }

        public ActionResult ModifyMenu(string id)
        {
            int n = Convert.ToInt32(id);
            List<Menu> mlist = null;
            Menu tb = null;
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();
                tb = db.Menus.Where(t => t.ID == n).FirstOrDefault();


            }
            ViewBag.mlist = mlist;

            return PartialView(tb);
        }


        public ActionResult SaveModifyMenu(HttpPostedFileBase file, Menu tb)
        {
            //获取数据库中原始数据
            var db = new TraverBDEntities();
            var temp = db.Menus.Where(s => s.ID == tb.ID).FirstOrDefault();

            //有图片才进行如下处理
            if (file != null)
            {
                //用于同名图片处理，设置物理路径
                string fileName = "~/CreaveFiles/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try
                {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    temp.ImgUrl = fileName.Replace("~", "");
                }
                catch (Exception ex)
                {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                }
            }

            temp.ID = tb.ID;
            temp.MenuUrl = tb.MenuUrl;
            temp.MenuName = tb.MenuName;
            temp.OrderBy = tb.OrderBy;
            temp.ModifyUserId = 1;//后台管理员 的 Id【从登陆Session 中获取】
            temp.ModifyTime = DateTime.Now;

            //修改 原始数据的状态
            db.Entry<Menu>(temp).State = EntityState.Modified;
            int i = db.SaveChanges();
            if (i > 0)
            {
                return Json(new { result = "true", msg = "修改成功" });
            }
            else
            {
                return Json(new { result = "false", msg = "修改失败" });
            }

        }


        public ActionResult GetMenu()
        {
            List<Menu> list = null;
            using (var db = new TraverBDEntities())
            {
                list = db.Menus.Where(s => s.IsDele == 0).ToList();
            }
            return PartialView(list);
        }
        public ActionResult GetMenu1()
        {
            List<Menu> list = null;
            using (var db = new TraverBDEntities())
            {
                list = db.Menus.Where(s => s.ID != 1).ToList();
            }
            return PartialView(list);
        }


    }
}