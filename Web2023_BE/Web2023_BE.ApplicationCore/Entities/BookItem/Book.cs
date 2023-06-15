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
    /// Thực thể bạn đọc
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(BookID)), Description("books")]
    public class Book : BaseEntity
    {

        #region Property
        /// <summary>
        /// Id bạn đọc
        /// </summary>
        [Key]
        [IDuplicate]
        [Display(Name = "ID sách")]
        public Guid BookID { get; set; } = Guid.NewGuid();

        [Display(Name = "Thể loại sách")]
        public Guid CategoryID { get; set; } = Guid.Empty;

        [IDuplicate]
        [Display(Name = "Mã sách")]
        public string BookCode { get; set; }

        [Display(Name = "Tên sách")]
        public string BookName { get; set; }

        [Display(Name = "Mô tả sách")]
        public string Description { get; set; }

        [Display(Name = "Nhà xuất bản")]
        public string Publisher { get; set; }

        [Display(Name = "Tác giả")]
        public string Author { get; set; }

        [Display(Name = "Mã ngôn ngữ")]
        public string LanguageCode { get; set; }

        [Display(Name = "Đường dẫn ảnh")]
        public string Image { get; set; }

        [Display(Name = "Đường dẫn tệp")]
        public string File { get; set; }

        public bool IsPrivate { get; set; }

        public int BookType { get; set; } = 0;
        #endregion
    }
}
