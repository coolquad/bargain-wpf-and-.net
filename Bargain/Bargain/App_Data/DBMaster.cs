using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DBMaster
/// </summary>
public class DBMaster
{

    public DBMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void CheckSession()
    {       

        if (Convert.ToString(HttpContext.Current.Session["UserId"].ToString()) == "")
            HttpContext.Current.Response.Redirect("Default.aspx");

        if (Convert.ToString(HttpContext.Current.Session["Role"].ToString()) == "")
            HttpContext.Current.Response.Redirect("Default.aspx");

        if (Convert.ToString(HttpContext.Current.Session["RoleId"].ToString()) == "")
            HttpContext.Current.Response.Redirect("Default.aspx");

        if (Convert.ToString(HttpContext.Current.Session["EmployeeId"].ToString()) == "")
            HttpContext.Current.Response.Redirect("Default.aspx");
       

    }

    public DataTable GetData(string sqltxt)
    {
        try
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataAdapter adap = new SqlDataAdapter(sqltxt, con);
            adap.Fill(dt);
            con.Close();
            con.Dispose();
            //log.WriteLog("Class", true, sqltxt, "GetData");
            return dt;
        }
        catch (Exception e)
        {
            //log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
            throw new ArgumentException(e.Message + e.StackTrace);
        }

    }

    public string SaveData(string sqltxt)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sc = new SqlCommand(sqltxt, con);
            sc.ExecuteNonQuery();
            con.Close();
            con.Dispose();

            //log.WriteLog("Class", false, sqltxt, "Record Submitted Successfully");
            return "Record submitted successfully";
        }
        catch (Exception e)
        {
            //log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
            return e.Message + e.StackTrace;
        }
    }

    public string DeleteData(string sqltxt)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sc = new SqlCommand(sqltxt, con);
            sc.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            //log.WriteLog("Class", true, sqltxt, "Record deleted Successfully");
            return "Record deleted successfully";
        }
        catch (Exception e)
        {
            // log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
            return e.Message + e.StackTrace;
        }
    }

}