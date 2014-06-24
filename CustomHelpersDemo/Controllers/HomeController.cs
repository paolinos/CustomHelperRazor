using CustomHelpersDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using CustomHelper;
using CustomHelpersDemo.Managers;

namespace CustomHelpersDemo.Controllers
{
    public class HomeController : Controller
    {
        private class JsonDataResult
        {
            public bool error { get; set; }
            public string msg { get; set; }
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        #region --------------------------------------------------      DemoForm

        [HttpGet]
        public ActionResult DemoForm()
        {
            ViewBag.Saved = false;
            return View();
        }
        [HttpPost]
        public ActionResult DemoForm(User model)
        {
            ViewBag.Saved = false;
            if (ModelState.IsValid)
            {
                ViewBag.Saved = true;
                return View();
            }

            return View(model);
        }
        [HttpPost]
        public JsonResult DemoFormAjax(User model)
        {
            if (!Request.IsAjaxRequest())
            {
                return Json(null);
            }

            JsonDataResult jsonResult = new JsonDataResult();
            jsonResult.error = true;
            jsonResult.msg = "Model invalid";

            if (ModelState.IsValid)
            {
                jsonResult.error = false;
                jsonResult.msg = string.Empty;
            }

            return Json(jsonResult);
        }

        #endregion --------------------------------------------------      DemoForm




        #region --------------------------------------------------      Demo Grid
        [HttpGet]
        public ActionResult DemoGrid()
        {
            GameManager.fill();

            return View();
        }
        [HttpGet]
        public JsonResult GetGridList(JqGridData jqGridData)
        {
            //IEnumerable<Game> list = Conversor_ArchivosBusiness.GetAllEnum();
            return CustomJQGridHelper.UpdateJQGrid<Game>(jqGridData, GameManager.GetAll());
        }
        #endregion --------------------------------------------------      Demo Grid




        #region --------------------------------------------------      Demo CheckBoxList
        [HttpGet]
        public ActionResult DemoCheckBoxList()
        {
            return View();
        }

        #endregion --------------------------------------------------      Demo CheckBoxList
    }
}