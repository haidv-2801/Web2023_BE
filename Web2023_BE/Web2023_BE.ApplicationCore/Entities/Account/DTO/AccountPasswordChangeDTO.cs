using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class AccountPasswordChangeDTO : BaseEntity
    {
        #region Property
        /// <summary>
        /// Id tài khoản
        /// </summary>
        [Key]
        public Guid AccountID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Mật khẩu mới")]
        public string Password { get; set; }
        #endregion
    }
}
