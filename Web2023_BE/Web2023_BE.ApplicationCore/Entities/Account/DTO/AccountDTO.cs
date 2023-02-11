using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class AccountDTO : BaseEntity
    {
        #region Property
        /// <summary>
        /// Id tài khoản
        /// </summary>
        [Key]
        public Guid AccountID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Họ và tên tài khoản
        /// </summary>
        [Display(Name = "Tên người dùng")]
        [IDuplicate]
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        [IDuplicate]
        [IEmailFormat]
        public string Email { get; set; }


        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }


        [Display(Name = "Số điện thoại")]
        [IDuplicate]
        public string PhoneNumber { get; set; }

        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        #endregion
    }
}
