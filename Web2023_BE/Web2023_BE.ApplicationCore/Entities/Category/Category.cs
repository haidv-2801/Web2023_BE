using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể thể loại
    /// </summary>
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
        [IDuplicate]
        [IRequired]
        [Display(Name = "Tiêu đề thể loại")]
        public string Title { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [Display(Name = "ID cha của thể loại")]
        public Guid? ParentID { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }
        #endregion
    }
}
