using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class AcceptCardMany
    {
        public List<string> IdSelected { get; set; }

        public int CardStatus { get; set; }
    }
}
