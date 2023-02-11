using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class BookOrderViewDTO : BaseEntity
    {
        #region Property
        public Guid AccountID { get; set; }
        public Guid BookOrderID { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string BookOrderCode { get; set; }

        public string BookOrderInformation { get; set; }

        public int OrderStatus { get; set; }

        public string Note { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime DueDate { get; set; }
        #endregion
    }
}
