using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelPro.Web.Models;
namespace TravelPro.Web.Controllers
{
    public class PlaceController : Controller
    {
        //
        // GET: /Place/
public ActionResult Index(int id)
        {
            ViewBag.MenuId = id;
            return View();
        }

        public ActionResult getScenery(int id)
        {
            List<Scenery> SList = null;
            
            using (var db = new TraverBDEntities())
            {
                SList = db.Sceneries.Where(s => s.MenuId == id).ToList();
            }
            return PartialView(SList);
        }


        public ActionResult getFood(int id)
        {
            List<food> FList = null;

            using (var db = new TraverBDEntities())
            {
                FList = db.foods.Where(s => s.MenuId == id).ToList();
            }
            return PartialView(FList);
        }

        public ActionResult getNews(int id)
        {
            List<Text> NList = null;

            using (var db = new TraverBDEntities())
            {
                NList = db.Texts.Where(s => s.MenuId == id).ToList();
            }
            return PartialView(NList);
        }
    

    }
}
