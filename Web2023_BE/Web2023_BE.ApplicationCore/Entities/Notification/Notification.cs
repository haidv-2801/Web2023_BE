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
    /// Thực thể thông báo
    /// </summary>
    public class Notification : BaseEntity
    {

        #region Property
        /// <summary>
        /// Id thông báo
        /// </summary>
        [Key]
        public Guid NotificationID { get; set; } = Guid.NewGuid();

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Từ")]
        public Guid From { get; set; } = Guid.Empty;

        [Display(Name = "Đến")]
        public Guid To { get; set; }

        [Display(Name = "Đến Email")]
        public string ToEmail { get; set; }

        [Display(Name = "Trạng thái đọc")]
        public bool IsReaded { get; set; } = false;
        #endregion
    }
}
