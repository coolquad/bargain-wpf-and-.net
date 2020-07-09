using Bargain.Areas.MemberPanel.DataAccess;
using Bargain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bargain.Controllers
{
    public class BargainsController : Controller
    {
        // GET: Bargains
        public ActionResult Index()
        {
            BargainModel objBargain = new BargainModel();

            BargainsData objDB = new BargainsData(); //calling class DBdata

            objBargain.listBargainModel = objDB.AllBargainsdata("2");
            objBargain.list2BargainModel = objDB.AllBargainsList();
            //  AllBargainsdata

            return View(objBargain);
        }


        public ActionResult Competition()
        {
            BargainModel model = new BargainModel();

            BargainsData obj = new BargainsData(); 

            model.listBargainModel = obj.AllBargainsdata("4");
            //  AllBargainsdata

            return View(model);
        }



    }
}