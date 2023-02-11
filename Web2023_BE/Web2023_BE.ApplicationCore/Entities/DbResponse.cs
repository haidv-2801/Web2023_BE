using Web2023_BE.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class DbResponse
    {
        public object Data { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }
    }
}
