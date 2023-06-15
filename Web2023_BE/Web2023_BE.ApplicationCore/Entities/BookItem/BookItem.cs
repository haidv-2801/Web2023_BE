using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel;
using Nest;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể sách mượn
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(BookID)), Description("bookitems")]
    [DisplayName("Book")]
    public class BookItem : Book
    {
        #region Property
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Display(Name = "Loại tài liệu")]
        public int BookFormat { get; set; }

        [Display(Name = "Thời gian xuất bản")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Số bản ghi")]
        public int Amount { get; set; }

        [Display(Name = "Số bản ghi đã đặt trước")]
        public int Reserved { get; set; }

        [Display(Name = "Số bản ghi đã cho mượn")]
        public int Loaned { get; set; }

        [Display(Name = "Số bản ghi có sẵn")]
        public int Available { get; set; }
        #endregion
    }
}
