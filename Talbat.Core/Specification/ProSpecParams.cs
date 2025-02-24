﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Specification
{
   public class ProSpecParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        private int pagesize=5;

        public int PageSize
        {
            get { return pagesize; }
            set { pagesize= value >10 ? 10:value; }
        }
        public int PageIndex { get; set; } = 1;
        private string? search;

        public  string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }


    }
}
