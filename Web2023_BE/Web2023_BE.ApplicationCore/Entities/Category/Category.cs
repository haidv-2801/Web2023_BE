using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể thể loại
    /// </summary>
    [ConfigTables(TableName = "category", UniqueColumns = "Title;Type")]
    public class Category : BaseEntity
    {
        #region Property
        /// <summary>
        /// Id thể loại
        /// </summary>
        [Key]
        [IDuplicate]
        [IRequired]
        [Display(Name = "Mã thể loại")]
        public Guid CategoryID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã thể loại
        /// </summary>
        [IRequired]
        [Display(Name = "Tiêu đề thể loại")]
        public string Title { get; set; }


        /// <summary>
        /// Mã thể loại
        /// </summary>
        public string Title_en { get; set; }

        /// <summary>
        /// Loại là sản phẩm, dự án
        /// </summary>
        public int Type { get; set; }


        #endregion
    }
}
