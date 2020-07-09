using Bargain.Areas.MemberPanel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bargain.Models
{
    public class BargainModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Required")]
        public int SpecialType { get; set; }

        [Required(ErrorMessage = "Required")]

        
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public string ImagePath { get; set; }




        //public string ImagePath { get; set; }
        public string URL { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Promocode { get; set; }
        public string Tags { get; set; }
      

        public string PostedBy { get; set; }
        public string Special { get; set; }
        public string PostedName { get; set; }
        public DateTime EntryDate { get; set; }

        public int BargainLike { get; set; }
        public int BargainDislike { get; set; }

        public string ApproveStatus { get; set; }
        public int ApproveId { get; set; }
        public Boolean Free { get; set; }

        public int commentID { get; set; }

        public List<SpecialTypeModel> listSpecialTypeModel { get; set; }
        public List<BargainModel> listBargainModel { get; set; }
        public List<BargainModel> list2BargainModel { get; set; }
        public List<BargainImages> listBagainsImages { get; set; }
        public List<BargainsCommentsModel> listBargainsCommentParentsModel { get; set; }
        public List<BargainsCommentsModel> listBargainsCommentChildModel { get; set; }
        public BargainModel()
        {
            listBargainModel = new List<BargainModel>();


        }

    }


    public class BargainImages
    {
        public string fileName { get; set; }
    }

}