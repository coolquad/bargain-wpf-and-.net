using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bargain.Areas.MemberPanel.Models
{
    public class BargainsCommentsModel
    {
        public int ID { get; set; }
        public int ParentId { get; set; }
        public int MemberId { get; set; }
        public int BargainId { get; set; }
        public string Comment { get; set; }
        public string FirstName { get; set; }

        public int ApproveStatus { get; set; }

        public DateTime CommentDate { get; set; }
       
    }
}