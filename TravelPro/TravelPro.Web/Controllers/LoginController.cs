using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPro.Web.Models;

namespace TravelPro.Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult DoLogin(string account, string pwd)
        {
            var db = new TraverBDEntities();
            //int result= db.AdminInfoes.Count(a => a.LoginAccout == account && a.Pwd == pwd);
            Admin result = db.Admins.Where(a => a.Admin1 == account && a.Pwd == pwd).FirstOrDefault();
            if (result == null)
            {
                var obj = new { code = 0, msg = "用户名或密码错误" };
               return Json(obj);
            }
            else
            {
                Session["Admin"] = result;
                var obj = new { code = 1, msg = "登陆成功", url = "/Home/Index" };
                return Json(obj);
            }


        }


        //退出系统
        public ActionResult Loginout()
        {
            //清除登录信息
            Session["Admin"] = null;
            //返回登陆页面
            return RedirectToAction("index");


        }
   
    }
}
