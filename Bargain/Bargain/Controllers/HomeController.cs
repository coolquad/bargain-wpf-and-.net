using Bargain.Areas.MemberPanel.DataAccess;
using Bargain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Bargain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            BargainModel objBargain = new BargainModel();

            BargainsData objDB = new BargainsData(); //calling class DBdata

            objBargain.listBargainModel = objDB.AllBargainsdata("2");
            objBargain.list2BargainModel = objDB.AllBargainsList();
            //  AllBargainsdata

            return View(objBargain);
        }

        public ActionResult Detail(int id = 0)
        {
            if (Convert.ToString(Session["MemberId"]) == "")
            {
                TempData["msg"] = "<script>alert('Please Login First');window.location='../Home/Index';</script>";
                return Redirect("/Home/Index");
            }

            BargainsData objDB = new BargainsData();
            BargainModel bargainModel = new BargainModel();
            if (id > 0)
            {
                bargainModel = objDB.GetBargainById(id);
                bargainModel.listBargainsCommentParentsModel = objDB.BargainsCommentParentsList(id);
                bargainModel.listBargainsCommentChildModel = objDB.BargainsCommentChildList(id);
              
            }
            return View(bargainModel);
        }

        [HttpPost]
        public ActionResult Detail(FormCollection frm)
       {
            string ID = "";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            DataTable dts1 = new DataTable();          


            string sqltxt = "INSERT INTO [dbo].[BargainsComments]([ParentId],[MemberId],[BargainId],[Comment],[CommentDate],[ApproveStatus])";
            sqltxt = sqltxt + " Values('"+frm["commentID"]+"','" + Convert.ToString(Session["MemberId"]) + "','" + frm["Id"] + "','" + frm["description"] + "',getdate(),0)";

            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sc = new SqlCommand(sqltxt, con);
            ID = Convert.ToString(sc.ExecuteScalar());
            con.Close();
            BargainsData objDB = new BargainsData();
            BargainModel bargainModel = new BargainModel();
            int id = Convert.ToInt32(frm["Id"]);
            if (id > 0)
            {
                bargainModel = objDB.GetBargainById(id);
                bargainModel.listBargainsCommentParentsModel = objDB.BargainsCommentParentsList(id);
                bargainModel.listBargainsCommentChildModel = objDB.BargainsCommentChildList(id);
            }
            TempData["msg"] = "<script>alert('Thanks for Commenting it will be display after approval');</script>";
            return View(bargainModel);

        }



        [HttpPost]
        public JsonResult AjaxMethod(int id, int value, int Oldvalue)
        {
            string ids = "0";
            string sqltxt = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            SqlCommand com = new SqlCommand();

            if (value == 1)
            {
                Oldvalue = Oldvalue + 1;
                sqltxt = " update Bargains set BargainLike='" + Oldvalue + "' where Id=" + id + " ";
            }
            else
            {
                Oldvalue = Oldvalue + 1;
                sqltxt = " update Bargains set BargainDislike='" + Oldvalue + "' where Id=" + id + " ";
            }
            // string sqlQuery = "select ";
            SqlDataAdapter adap = new SqlDataAdapter(sqltxt, con);
            DataTable dt = new DataTable();
            adap.Fill(dt);
            con.Close();
            con.Dispose();

            return Json(Oldvalue);
        }

        public ActionResult Reply(int comId = 0, int bargId = 0)
        {
            BargainsData objDB = new BargainsData();
            BargainModel bargainModel = new BargainModel();
            int id = Convert.ToInt32(bargId);
            if (id > 0)
            {
                bargainModel = objDB.GetBargainById(id);
                bargainModel.listBargainsCommentParentsModel = objDB.BargainsCommentParentsList(id);
                bargainModel.listBargainsCommentChildModel = objDB.BargainsCommentChildList(id);
            }
            bargainModel.commentID = comId;
            return View("Detail", bargainModel);
            //return View(bargainModel);
        }


        public ActionResult LogOff()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult About()
        {         
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        
    }
}