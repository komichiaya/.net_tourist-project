using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPro.Web.Models;
using System.IO;
using System.Data;

namespace TravelPro.Web.Controllers
{
    public class MationController : Controller
    {
        //
        // GET: /Mation/
        // GET: /Scenery/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetPageData(int page = 1, int limit = 10)
        {
            var db = new TraverBDEntities();
            //统计数据库中 总的记录
            int count = db.Texts.Count();

            var list = db.Texts.OrderBy(s => s.Id)//根据什么排序
                              .Skip((page - 1) * limit)// 跳过多少条数据 page:当前第几页
                              .Take(limit)//提取多少条数据， limit:每一页显示的条数
                              .Select(s => new
                              { //投影一个新的对象
                                  Id = s.Id,
                                  TextTile = s.TextTile,
                                  MenuName = s.Menu.MenuName,
                                  Textdetails = s.Textdetails,
                                  TextURL = s.TextURL,
                                  MenuId = s.MenuId,
                                  OrderBy = s.OrderBy,
                                  CreateUserId = s.CreateUserId,
                                  CreateTime = s.CreateTime
                              }).ToList();//转换成列表
            //根据 layui table 模块的数据格式来创建的一个 匿名对象。
            var obj = new { code = 0, msg = "", count = count, data = list };
            return Json(obj);
        }

        public ActionResult GetPageMation(int menuid)
        {

            List<Text> tbList = null;
            using (var db = new TraverBDEntities())
            {
                tbList = db.Texts.Where(t => t.MenuId == menuid).ToList();
            }


            return PartialView(tbList);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMation()
        {
            List<Menu> mlist = new List<Menu>();
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();

            }


            return View(mlist);
        }

        [HttpPost]
        public ActionResult SaveMation(HttpPostedFileBase file, Text tb)
        {
            if (file == null)
            {
                return Json(new { result = "false", code = 400, msg = "文件不存在" }, "text/html");
            }

            //用于同名图片处理，设置物理路径
            string fileName = "~/UploadFiles/Mation/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
            var physicsFileName = Server.MapPath(fileName);
            try
            {
                //保存图片到对应目录
                file.SaveAs(physicsFileName);
                using (var db = new TraverBDEntities())
                {
                    tb.TextURL = fileName.Replace("~", "");
                    tb.CreateTime = DateTime.Now;
                    tb.CreateUserId = 1;
                    db.Texts.Add(tb);
                    db.SaveChanges();
                }
                return Json(new { result = "true", msg = "上传成功", imgUrl = fileName });
            }
            catch (Exception ex)
            {
                return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
            }
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult DeleMation(int id)
        {
            int i = 0;
            using (var db = new TraverBDEntities())
            {
                var temp = db.Texts.Where(s => s.Id == id).FirstOrDefault();
                db.Texts.Remove(temp);
                i = db.SaveChanges();
            }
            var res = new { code = i };
            return Json(res);
        }



        public ActionResult ModifyMation(int id)
        {
            List<Menu> mlist = null;
            Text tb = null;
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();
                tb = db.Texts.Where(t => t.Id == id).FirstOrDefault();


            }
            ViewBag.mlist = mlist;

            return PartialView(tb);
        }


        public ActionResult SaveModifyMation(HttpPostedFileBase file,Text tb)
        {    //获取数据库原始数据
            var db = new TraverBDEntities();
            var temp = db.Texts.Where(s => s.Id == tb.Id).FirstOrDefault();
            //有图片进行如下处理
            if (file != null)
            {

                string fileName = "~/UploadFiles/Mation" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                try
                {
                    //保存图片到对应目录
                    file.SaveAs(physicsFileName);
                    temp.TextURL = fileName.Replace("~", "");
                }
                catch (Exception ex)
                {
                    return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
                } //用于同名图片处理，设置物理路径

            }
            temp.Textdetails = tb.Textdetails;
            temp.MenuId = tb.MenuId;
            temp.TextTile = tb.TextTile;
         
            temp.ModifyUserId = 1;
            temp.ModifyTime = DateTime.Now;
            db.Entry<Text>(temp).State = EntityState.Modified;
            int i = db.SaveChanges();
            if (i > 0)
            {
                return Json(new { result = "true", msg = "修改成功" });
            }
            else
            {
                return Json(new { result = "true", msg = "修改失败" });

            }




        }
             public ActionResult MationText(int id)
        {
            var db = new TraverBDEntities();
            var obj = db.Texts.Where(s => s.Id == id).FirstOrDefault();
            return View(obj);
        }

    }
}
