using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class PagingRequest
    {
        public string Sort { get; set; }

        public string Columns { get; set; }

        public string Filter { get; set; }

        public int Delta { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
