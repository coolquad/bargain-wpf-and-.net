using Bargain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

namespace Bargain.Controllers
{
    public class RegisterController : Controller
    {
        [HttpPost]
        public ActionResult Index(FormCollection frm)
        {
            string ID = "";


            SqlConnection con= new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            DataTable dts = new DataTable();
            string sqltxt = "Select * from Member where Email='" + frm["lblEmail"] + "'";
            SqlDataAdapter adap = new SqlDataAdapter(sqltxt, con);
            adap.Fill(dts);

            if (dts.Rows.Count > 0)
            {
                TempData["msg"] = "<script>alert('Email Id Already Exist!!');window.location='../Home/Index';</script>";
                // ViewBag.FailMessage = "Application is Already Received of This UID No.!!";
                //ViewBag.SucessMessage = "";
                return RedirectToAction("../Home", "Index");
            }
            else
            {
                DataTable dts1 = new DataTable();
                 
                    sqltxt = "INSERT INTO [dbo].[Member]([FirstName],[LastName],[Email],[State],[Postcode],[Password],[EntryDate],[IsActive])";
                    sqltxt = sqltxt + " Values('" + frm["lblFirstName"] + "','" + frm["lblLastName"] + "','" + frm["lblEmail"] + "','" + frm["lblState"] + "',";
                    sqltxt = sqltxt + " '" + frm["lblPostcode"] + "','" + frm["lblPassword"] + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm tt") + "',1)";
                    sqltxt = sqltxt + " Select Scope_Identity();";

                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand sc = new SqlCommand(sqltxt, con);
                    ID = Convert.ToString(sc.ExecuteScalar());
                con.Close();
                Session["MemberId"] = Convert.ToString(ID);
                Session["Name"] = Convert.ToString(frm["lblFirstName"]);

                TempData["msg"] = "<script>alert('Registered Successfully!!');window.location='Home/Index';</script>";
              //  return RedirectToAction("Index", "Dashboard", new { area = "MemberPanel" });

                return RedirectToAction("Index", "Home");

                 
            } 


        }
    }
}
