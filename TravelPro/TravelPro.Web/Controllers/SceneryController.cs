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
    public class SceneryController : Controller
    {
        //
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
            int count = db.Sceneries.Count();

            var list = db.Sceneries.OrderBy(s => s.Id)//根据什么排序
                              .Skip((page - 1) * limit)// 跳过多少条数据 page:当前第几页
                              .Take(limit)//提取多少条数据， limit:每一页显示的条数
                              .Select(s => new
                              { //投影一个新的对象
                               Id=  s.Id,
                               SceneryName=  s.SceneryName,
                                 MenuName = s.Menu.MenuName,
                                Scenerydetails= s.Scenerydetails,
                                 SceneryImgUrl = s.SceneryImgUrl,
                                MenuId= s.MenuId,
                                OrderBy= s.OrderBy,
                               CreateUserId =s.CreateUserId,
                             CreateTime  =s.CreateTime
                              }).ToList();//转换成列表
            //根据 layui table 模块的数据格式来创建的一个 匿名对象。
            var obj = new { code = 0, msg = "", count = count, data = list };
            return Json(obj);
        }

        public ActionResult GetPageScenery(int menuid)
        {

            List<Scenery> tbList = null;
            using (var db = new TraverBDEntities())
            {
                tbList = db.Sceneries.Where(t => t.MenuId == menuid).ToList();
            }


            return PartialView(tbList);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddScenery()
        {
            List<Menu> mlist = new List<Menu>();
            using (var db = new TraverBDEntities())
            {
                mlist = db.Menus.Where(s => s.IsDele == 0).ToList();

            }


            return View(mlist);
        }

        [HttpPost]
        public ActionResult SaveScenery(HttpPostedFileBase file, Scenery tb)
        {
            if (file == null)
            {
                return Json(new { result = "false", code = 400, msg = "文件不存在" }, "text/html");
            }

            //用于同名图片处理，设置物理路径
            string fileName = "~/UploadFiles/Scenery/" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
            var physicsFileName = Server.MapPath(fileName);
            try
            {
                //保存图片到对应目录
                file.SaveAs(physicsFileName);
                using (var db = new TraverBDEntities())
                {
                    tb.SceneryImgUrl = fileName.Replace("~", "");
                    tb.CreateTime = DateTime.Now;
                    tb.CreateUserId = 1;
                    db.Sceneries.Add(tb);
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
        public ActionResult DeleScenery(int id)
        {
            int i = 0;
            using (var db = new TraverBDEntities())
            {
                var temp = db.Sceneries.Where(s => s.Id == id).FirstOrDefault();
                db.Sceneries.Remove(temp);
                i = db.SaveChanges();
            }
            var res = new { code = i };
            return Json(res);
        }



        public ActionResult ModifyScenery(int id)
        {
            List<Menu> mlist = null;
            Scenery tb = null;
            using(var db=new TraverBDEntities())
            {mlist=db.Menus.Where(s=>s.IsDele==0).ToList();
            tb = db.Sceneries.Where(t => t.Id == id).FirstOrDefault();

            
            }
            ViewBag.mlist = mlist;

            return PartialView(tb);
        }


        public ActionResult SaveModifyScenery(HttpPostedFileBase file, Scenery tb)
        {    //获取数据库原始数据
            var db = new TraverBDEntities();
            var temp = db.Sceneries.Where(s=>s.Id==tb.Id).FirstOrDefault();
            //有图片进行如下处理
            if (file != null) {

                string fileName = "~/UploadFiles/Scenery" + DateTime.Now.ToString("yyyyMMddHHssmm") + Path.GetFileName(file.FileName);
                var physicsFileName = Server.MapPath(fileName);
                    try
            {
                //保存图片到对应目录
                file.SaveAs(physicsFileName);
                temp.SceneryImgUrl= fileName.Replace("~", "");
            }
                 catch(Exception ex)
            {
                return Json(new { result = "false", code = 500, msg = "保存失败" }, "text/html");
            } //用于同名图片处理，设置物理路径

            }
            temp.Scenerydetails = tb.Scenerydetails;
                    temp.MenuId = tb.MenuId;
                    temp.SceneryName = tb.SceneryName;
                    temp.ModifyUserId = 1;
                    temp.ModifyTime = DateTime.Now;
                    db.Entry<Scenery>(temp).State = EntityState.Modified;
                    int i=db.SaveChanges();
                    if (i > 0)
                    {
                        return Json(new { result = "true", msg = "修改成功" });
                    }
                    else {
                        return Json(new { result = "true", msg = "修改失败" });
                    
                    }

               
            
            
        
        }

        public ActionResult SceneryText(int id)
        {
            var db = new TraverBDEntities();
            var obj = db.Sceneries.Where(s => s.Id == id).FirstOrDefault();
            return View(obj);
        }

    }

    }

