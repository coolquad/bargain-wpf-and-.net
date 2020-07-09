﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bargain.Models
{
    public class SpecialTypeModel
    {
        public SpecialTypeModel()
        {
            listSpecialTypeModel = new List<SpecialTypeModel>();
        }
        public int Id { get; set; }
        public string Descr { get; set; }
        public DateTime EntryDate { get; set; }
        public bool IsActive { get; set; }
        public List<SpecialTypeModel> listSpecialTypeModel { get; set; }
    }
}