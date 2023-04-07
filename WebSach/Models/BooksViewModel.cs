using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSach.Models
{
    public class BooksViewModel
    {
        public Books book;
        public int? chapterid;
        public Chapter chapter;
        public List<Chapter> Chapters;

    }
}