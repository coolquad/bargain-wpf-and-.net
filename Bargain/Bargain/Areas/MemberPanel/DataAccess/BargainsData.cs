using Bargain.Areas.MemberPanel.Models;
using Bargain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Bargain.Areas.MemberPanel.DataAccess
{
    public class BargainsData
    {
        string sqltxt = "";

        //   Bind Dropdown
        public List<SpecialTypeModel> GetSpecialType()
        {
            SqlConnection con = null;
            DataSet dt = null;
            List<SpecialTypeModel> SpecialTypeModelllist = null;

            try
            {

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                sqltxt = " select * from SpecialType where IsActive=0";
                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataSet();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                SpecialTypeModelllist = new List<SpecialTypeModel>();
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    SpecialTypeModel obj = new SpecialTypeModel();
                    obj.Id = Convert.ToInt32(dt.Tables[0].Rows[i]["ID"]);
                    obj.Descr = Convert.ToString(dt.Tables[0].Rows[i]["Descr"]);
                    SpecialTypeModelllist.Add(obj);
                }

                return SpecialTypeModelllist;

            }
            catch (Exception ex)
            {
                return SpecialTypeModelllist;
            }
        }


        public string InsertData(BargainModel model)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());

                if (con.State == ConnectionState.Closed)
                    con.Open();

                sqltxt = " insert into Bargains (Heading,Description,SpecialType,ImagePath,URL,StartDate,EndDate,Promocode,Tags,Free,PostedBy,IsActive,EntryDate,BargainLike,BargainDislike,ApproveStatus) ";
                sqltxt = sqltxt + " values('" + model.Heading + "','" + model.Description.Replace("'", "&#39;") + "','" + model.SpecialType + "','" + model.ImagePath + "','" + model.URL + "','" + Convert.ToDateTime(model.StartDate).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd") + "',";
                sqltxt = sqltxt + " '" + model.Promocode + "','" + model.Tags + "', '" + model.Free + "','" + model.PostedBy + "',1,getdate(),0,0,0)";

                SqlCommand sc = new SqlCommand(sqltxt, con);
                sc.ExecuteNonQuery();
                con.Close();
                con.Dispose();

                return "Record Submitted successfully";

            }
            catch (Exception e)
            {
                //log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
                return e.Message + e.StackTrace;
            }

        }


        // popular date bargains   All bargains
        public List<BargainModel> AllBargainsdata(string value)
        {
            SqlConnection con = null;
            DataSet dt = null;

            List<BargainModel> BargainModellist = null;

            try
            {

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();

                sqltxt = "SELECT     dbo.Bargains.Id, dbo.Bargains.Heading, dbo.Bargains.Description, dbo.Bargains.SpecialType, dbo.SpecialType.Descr AS Special, dbo.Bargains.ImagePath, dbo.Bargains.StartDate, dbo.Bargains.EndDate, dbo.Bargains.URL, ";
                sqltxt = sqltxt + "  dbo.Bargains.Promocode, dbo.Bargains.Tags, dbo.Bargains.Free, dbo.Bargains.PostedBy, dbo.Member.FirstName, dbo.Bargains.EntryDate,dbo.Bargains.BargainLike,dbo.Bargains.BargainDislike,";
                sqltxt = sqltxt + " case when dbo.Bargains.ApproveStatus=0 then 'Waiting for Approval' else case when dbo.Bargains.ApproveStatus=1 then 'Approved' else 'Rejected' end end Status";
                sqltxt = sqltxt + "  FROM  dbo.Bargains INNER JOIN";
                sqltxt = sqltxt + "   dbo.SpecialType ON dbo.Bargains.SpecialType = dbo.SpecialType.Id INNER JOIN";
                sqltxt = sqltxt + "  dbo.Member ON dbo.Bargains.PostedBy = dbo.Member.Id";
                sqltxt = sqltxt + " where dbo.Bargains.IsActive=1 ";
                if (value == "1")
                    sqltxt = sqltxt + " and cast(dbo.Bargains.EntryDate as date) = cast(getdate() as Date) and dbo.Bargains.PostedBy = " + HttpContext.Current.Session["MemberId"].ToString() + " ";
                else if (value == "2")
                    sqltxt = sqltxt + " and dbo.Bargains.ApproveStatus = 1 ";
                else if (value == "4")
                    sqltxt = sqltxt + " and dbo.Bargains.SpecialType = 2";
                else  // value = 3
                    sqltxt = sqltxt + " and dbo.Bargains.PostedBy = " + HttpContext.Current.Session["MemberId"].ToString() + "";

                sqltxt = sqltxt + " order by dbo.Bargains.Id desc ";
                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataSet();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                BargainModellist = new List<BargainModel>();
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {

                    BargainModel obj = new BargainModel();
                    obj.ID = Convert.ToInt32(dt.Tables[0].Rows[i]["ID"]);
                    obj.Heading = Convert.ToString(dt.Tables[0].Rows[i]["Heading"]);
                    obj.Description = Convert.ToString(dt.Tables[0].Rows[i]["Description"]);
                    obj.SpecialType = Convert.ToInt32(dt.Tables[0].Rows[i]["SpecialType"]);
                    obj.ImagePath = Convert.ToString(dt.Tables[0].Rows[i]["ImagePath"]);
                    string images1 = obj.ImagePath;
                    string[] Images = images1.Split(new char[] { ',' });
                    List<BargainImages> listBagainsImages = new List<BargainImages>();

                    foreach (string Image in Images)
                    {
                        BargainImages newobj = new BargainImages();
                        if (Image != "")
                        {
                            newobj.fileName = Image;
                            listBagainsImages.Add(newobj);
                        }
                    }

                    obj.StartDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["EndDate"]);
                    obj.URL = Convert.ToString(dt.Tables[0].Rows[i]["URL"]);
                    obj.Promocode = Convert.ToString(dt.Tables[0].Rows[i]["Promocode"]);
                    obj.Tags = Convert.ToString(dt.Tables[0].Rows[i]["Tags"]);
                    //obj.Free = Convert.ToString(dt.Tables[0].Rows[i]["Free"]);
                    obj.Special = Convert.ToString(dt.Tables[0].Rows[i]["Special"]);
                    obj.PostedName = Convert.ToString(dt.Tables[0].Rows[i]["FirstName"]);
                    obj.EntryDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["EntryDate"]);
                    obj.BargainLike = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainLike"]);
                    obj.BargainDislike = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainDislike"]);
                    obj.ApproveStatus = Convert.ToString(dt.Tables[0].Rows[i]["Status"]);
                    obj.listBagainsImages = listBagainsImages;
                    BargainModellist.Add(obj);

                }

                return BargainModellist;

            }
            catch (Exception e)
            {
                return BargainModellist;
            }
        }

        // bargains current date  top 2
        public List<BargainModel> AllBargainsList()
        {
            SqlConnection con = null;
            DataSet dt = null;

            List<BargainModel> BargainModellist = null;

            try
            {

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                sqltxt = "SELECT   Top (2)    dbo.Bargains.Id, dbo.Bargains.Heading, dbo.Bargains.Description, dbo.Bargains.SpecialType, dbo.SpecialType.Descr AS Special, dbo.Bargains.ImagePath, dbo.Bargains.StartDate, dbo.Bargains.EndDate, dbo.Bargains.URL, ";
                sqltxt = sqltxt + "  dbo.Bargains.Promocode, dbo.Bargains.Tags, dbo.Bargains.Free, dbo.Bargains.PostedBy, dbo.Member.FirstName, dbo.Bargains.EntryDate,dbo.Bargains.BargainLike,dbo.Bargains.BargainDislike,";
                sqltxt = sqltxt + " case when dbo.Bargains.ApproveStatus=0 then 'Waiting for Approval' else case when dbo.Bargains.ApproveStatus=1 then 'Approved' else 'Rejected' end end Status";
                sqltxt = sqltxt + "  FROM  dbo.Bargains INNER JOIN";
                sqltxt = sqltxt + "   dbo.SpecialType ON dbo.Bargains.SpecialType = dbo.SpecialType.Id INNER JOIN";
                sqltxt = sqltxt + "  dbo.Member ON dbo.Bargains.PostedBy = dbo.Member.Id";
                sqltxt = sqltxt + " where dbo.Bargains.IsActive=1 and dbo.Bargains.ApproveStatus = 1 ";
                sqltxt = sqltxt + " order by dbo.Bargains.Id desc ";
                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataSet();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                BargainModellist = new List<BargainModel>();
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    BargainModel obj = new BargainModel();
                    obj.ID = Convert.ToInt32(dt.Tables[0].Rows[i]["ID"]);
                    obj.Heading = Convert.ToString(dt.Tables[0].Rows[i]["Heading"]);
                    obj.Description = Convert.ToString(dt.Tables[0].Rows[i]["Description"]);
                    obj.SpecialType = Convert.ToInt32(dt.Tables[0].Rows[i]["SpecialType"]);
                    obj.ImagePath = Convert.ToString(dt.Tables[0].Rows[i]["ImagePath"]);
                    string images1 = obj.ImagePath;
                    string[] Images = images1.Split(new char[] { ',' });
                    List<BargainImages> listBagainsImages = new List<BargainImages>();

                    foreach (string Image in Images)
                    {
                        BargainImages newobj = new BargainImages();
                        if (Image != "")
                        {
                            newobj.fileName = Image;
                            listBagainsImages.Add(newobj);
                        }
                    }

                    obj.StartDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["StartDate"]);
                    obj.EndDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["EndDate"]);
                    obj.URL = Convert.ToString(dt.Tables[0].Rows[i]["URL"]);
                    obj.Promocode = Convert.ToString(dt.Tables[0].Rows[i]["Promocode"]);
                    obj.Tags = Convert.ToString(dt.Tables[0].Rows[i]["Tags"]);
                    //obj.Free = Convert.ToString(dt.Tables[0].Rows[i]["Free"]);
                    obj.Special = Convert.ToString(dt.Tables[0].Rows[i]["Special"]);
                    obj.PostedName = Convert.ToString(dt.Tables[0].Rows[i]["FirstName"]);
                    obj.EntryDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["EntryDate"]);
                    obj.BargainLike = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainLike"]);
                    obj.BargainDislike = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainDislike"]);
                    obj.ApproveStatus = Convert.ToString(dt.Tables[0].Rows[i]["Status"]);
                    obj.listBagainsImages = listBagainsImages;
                    BargainModellist.Add(obj);

                }

                return BargainModellist;

            }
            catch (Exception e)
            {
                return BargainModellist;
            }
        }


        public BargainModel GetBargainById(int id)
        {
            SqlConnection con = null;
            DataSet ds = null;
            BargainModel cobj = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());

                //  sqltxt = "select * from Bargains where Id = " + id + " and IsActive=0";

                sqltxt = "SELECT        dbo.Bargains.Id, dbo.Bargains.Heading, dbo.Bargains.Description, dbo.Bargains.SpecialType, dbo.SpecialType.Descr AS Special, dbo.Bargains.ImagePath, dbo.Bargains.StartDate, dbo.Bargains.EndDate, dbo.Bargains.URL, ";
                sqltxt = sqltxt + "  dbo.Bargains.Promocode, dbo.Bargains.Tags, dbo.Bargains.Free, dbo.Bargains.PostedBy, dbo.Member.FirstName, dbo.Bargains.EntryDate,dbo.Bargains.BargainLike,dbo.Bargains.BargainDislike";
                sqltxt = sqltxt + "  FROM  dbo.Bargains INNER JOIN";
                sqltxt = sqltxt + "   dbo.SpecialType ON dbo.Bargains.SpecialType = dbo.SpecialType.Id INNER JOIN";
                sqltxt = sqltxt + "  dbo.Member ON dbo.Bargains.PostedBy = dbo.Member.Id";
                sqltxt = sqltxt + " where  dbo.Bargains.Id = " + id + " and  dbo.Bargains.IsActive=1 ";

                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                con.Close();
                con.Dispose();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cobj = new BargainModel();

                    cobj.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    cobj.Heading = Convert.ToString(ds.Tables[0].Rows[i]["Heading"]);
                    cobj.Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                    cobj.SpecialType = Convert.ToInt32(ds.Tables[0].Rows[i]["SpecialType"]);
                    cobj.ImagePath = Convert.ToString(ds.Tables[0].Rows[i]["ImagePath"]);

                    string images1 = cobj.ImagePath;
                    string[] Images = images1.Split(new char[] { ',' });
                    List<BargainImages> listBagainsImages = new List<BargainImages>();

                    foreach (string Image in Images)
                    {
                        BargainImages newobj = new BargainImages();
                        if (Image != "")
                        {
                            newobj.fileName = Image;
                            listBagainsImages.Add(newobj);
                        }
                    }

                    cobj.URL = Convert.ToString(ds.Tables[0].Rows[i]["URL"]);
                    cobj.StartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["StartDate"]);
                    cobj.EndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString());
                    cobj.Promocode = Convert.ToString(ds.Tables[0].Rows[i]["Promocode"]);
                    cobj.Tags = Convert.ToString(ds.Tables[0].Rows[i]["Tags"]);
                    cobj.Free = Convert.ToBoolean(ds.Tables[0].Rows[i]["Free"]);
                    cobj.Special = Convert.ToString(ds.Tables[0].Rows[i]["Special"]);
                    cobj.PostedName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                    cobj.EntryDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["EntryDate"]);
                    cobj.BargainLike = Convert.ToInt32(ds.Tables[0].Rows[i]["BargainLike"]);
                    cobj.BargainDislike = Convert.ToInt32(ds.Tables[0].Rows[i]["BargainDislike"]);
                    cobj.listBagainsImages = listBagainsImages;
                }

                return cobj;

            }
            catch
            {
                return cobj;
            }

        }


        public string UpdateBargainData(BargainModel model)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());

                if (con.State == ConnectionState.Closed)
                    con.Open();


                sqltxt = " update Bargains set Heading='" + model.Heading + "',Description='" + model.Description + "',SpecialType='" + model.SpecialType + "',URL='" + model.URL + "', ";
                sqltxt = sqltxt + " StartDate='" + Convert.ToDateTime(model.StartDate).ToString("yyyy-MM-dd") + "',EndDate='" + Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd") + "',Promocode='" + model.Promocode + "',Tags='" + model.Tags + "',Free='" + model.Free + "',LastModifyDate=getdate()   ";
                if (model.ImagePath != "")
                    sqltxt = sqltxt + " ,ImagePath='" + model.ImagePath + "' ";
                sqltxt = sqltxt + " where Id='" + model.ID + "' ";

                SqlCommand sc = new SqlCommand(sqltxt, con);
                sc.ExecuteNonQuery();
                con.Close();
                con.Dispose();

                return "Record Update successfully";

            }
            catch (Exception e)
            {
                //log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
                return e.Message + e.StackTrace;
            }

        }


        public string DeleteData(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ConnectionString);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                sqltxt = " update Bargains set IsActive=0 where Id =" + id + " ";
                SqlCommand sc = new SqlCommand(sqltxt, con);
                sc.ExecuteNonQuery();
                con.Close();
                con.Dispose();

                return "Record deleted successfully";
            }
            catch (Exception e)
            {
                // log.WriteLog("Class", true, sqltxt, e.Message + e.StackTrace);
                return e.Message + e.StackTrace;
            }


        }

        // list comment
        public List<BargainsCommentsModel> BargainsCommentParentsList(int id)
        {
            SqlConnection con = null;
            DataSet dt = null;

            List<BargainsCommentsModel> BargainsCommentList = null;

            try
            {

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();

                sqltxt = "  SELECT        dbo.BargainsComments.Id, dbo.BargainsComments.Comment, dbo.Member.FirstName, dbo.BargainsComments.BargainId, dbo.BargainsComments.ParentId, dbo.BargainsComments.CommentDate,dbo.BargainsComments.ApproveStatus";
                sqltxt = sqltxt + " FROM            dbo.Member INNER JOIN";
                sqltxt = sqltxt + "   dbo.BargainsComments ON dbo.Member.Id = dbo.BargainsComments.MemberId";
                sqltxt = sqltxt + "  where   dbo.BargainsComments.ParentId = 0  ";
                if (id != 0)
                    sqltxt = sqltxt + "  and   dbo.BargainsComments.BargainId = " + id + " and dbo.BargainsComments.ApproveStatus=1";
                sqltxt = sqltxt + " order by dbo.BargainsComments.Id desc";


                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataSet();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                BargainsCommentList = new List<BargainsCommentsModel>();
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {

                    BargainsCommentsModel obj = new BargainsCommentsModel();
                    obj.ID = Convert.ToInt32(dt.Tables[0].Rows[i]["ID"]);
                    obj.Comment = Convert.ToString(dt.Tables[0].Rows[i]["Comment"]);
                    obj.FirstName = Convert.ToString(dt.Tables[0].Rows[i]["FirstName"]);
                    obj.BargainId = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainId"]);
                    obj.ParentId = Convert.ToInt32(dt.Tables[0].Rows[i]["ParentId"]);
                    obj.CommentDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["CommentDate"]);
                    obj.ApproveStatus = Convert.ToInt32(dt.Tables[0].Rows[i]["ApproveStatus"]);

                    BargainsCommentList.Add(obj);

                }

                return BargainsCommentList;

            }
            catch (Exception e)
            {
                return BargainsCommentList;
            }
        }

        public List<BargainsCommentsModel> BargainsCommentChildList(int id)
        {
            SqlConnection con = null;
            DataSet dt = null;

            List<BargainsCommentsModel> BargainsCommentList = null;

            try
            {

                con = new SqlConnection(ConfigurationManager.ConnectionStrings["BargainCon"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();

                sqltxt = "  SELECT        dbo.BargainsComments.Id, dbo.BargainsComments.Comment, dbo.Member.FirstName, dbo.BargainsComments.BargainId, dbo.BargainsComments.ParentId, dbo.BargainsComments.CommentDate,dbo.BargainsComments.ApproveStatus";
                sqltxt = sqltxt + " FROM            dbo.Member INNER JOIN";
                sqltxt = sqltxt + "   dbo.BargainsComments ON dbo.Member.Id = dbo.BargainsComments.MemberId";
                sqltxt = sqltxt + "  where  dbo.BargainsComments.ParentId!=0";
                if (id != 0)
                    sqltxt = sqltxt + "  and   dbo.BargainsComments.BargainId = " + id + " and dbo.BargainsComments.ApproveStatus=1";
                sqltxt = sqltxt + " order by dbo.BargainsComments.Id desc";

                SqlCommand cmd = new SqlCommand(sqltxt, con);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                dt = new DataSet();
                da.Fill(dt);
                con.Close();
                con.Dispose();
                BargainsCommentList = new List<BargainsCommentsModel>();
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {

                    BargainsCommentsModel obj = new BargainsCommentsModel();
                    obj.ID = Convert.ToInt32(dt.Tables[0].Rows[i]["ID"]);
                    obj.Comment = Convert.ToString(dt.Tables[0].Rows[i]["Comment"]);
                    obj.FirstName = Convert.ToString(dt.Tables[0].Rows[i]["FirstName"]);
                    obj.BargainId = Convert.ToInt32(dt.Tables[0].Rows[i]["BargainId"]);
                    obj.ParentId = Convert.ToInt32(dt.Tables[0].Rows[i]["ParentId"]);
                    obj.CommentDate = Convert.ToDateTime(dt.Tables[0].Rows[i]["CommentDate"]);
                    obj.ApproveStatus = Convert.ToInt32(dt.Tables[0].Rows[i]["ApproveStatus"]);


                    BargainsCommentList.Add(obj);

                }

                return BargainsCommentList;

            }
            catch (Exception e)
            {
                return BargainsCommentList;
            }
        }



    }
}