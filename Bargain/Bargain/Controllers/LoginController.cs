using Bargain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bargain.Controllers
{
    public class LoginController : Controller
    {
        DBMaster dbMaster = new DBMaster();
        string sqltxt = "";

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            sqltxt = " select * from Member where Email = '" + frm["Email"] + "' and Password  ='" + frm["Password"] + "'";
            DataTable dt = dbMaster.GetData(sqltxt);
            if (dt.Rows.Count > 0)
            {
                Session["MemberId"] = Convert.ToString(dt.Rows[0]["Id"]);
                Session["Name"] = Convert.ToString(dt.Rows[0]["FirstName"]);

               // return RedirectToAction("Index", "Dashboard", new { area = "MemberPanel" });
                return RedirectToAction("Index", "Home");

            }
            else
            {
              
                TempData["msg"] = "<script>alert('Email or Password is Wrong');window.location='../Home/Index';</script>";
                ViewBag.FailMessage = "Email or Password is Wrong";
            }
                //ViewBag.FailMessage = "Email or Password is Wrong";

            return RedirectToAction("Index", "Home");
            //~/HOME/NoResults
        }


    }
}