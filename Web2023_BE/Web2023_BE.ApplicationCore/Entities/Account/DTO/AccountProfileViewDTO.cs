using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    /// <summary>
    /// Thực thể người dùng
    /// </summary>
    [Table("AccountProfileViewDTO")]
    public class AccountProfileViewDTO : Account
    {
        #region
        public bool IsMember { get; set; } = false;
        #endregion
    }
}
