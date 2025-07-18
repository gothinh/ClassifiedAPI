﻿using System.Linq.Expressions;

namespace Classifieds.Repository
{


    public class TableInfo<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int ItemsCount { get; set; }
    }


    public class TableParameter
    {
        public string SortKey { get; set; } = string.Empty;
        public bool IsAccending { get; set; }
        public string SearchContent { get; set; }
    }

    public class TablePageParameter : TableParameter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}