using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Web2023_BE.ApplicationCore.Enums;
using Web2023_BE.ApplicationCore.Extensions;

namespace Web2023_BE.ApplicationCore.Entities
{
    [ConfigTables(TableName = "LibraryCard")]
    public class LibraryCard : BaseEntity
    {
        #region Property
        [Key]
        public Guid CardID { get; set; } = Guid.NewGuid();

        public Guid AccountID { get; set; }

        public string CardCode { get; set; }

        public int MemberType { get; set; }

        [Display(Name = "Tổng số sách đã mượn")]
        public int TotalBookCheckedOut { get; set; } = 0;

        [Display(Name = "Tổng số sách đang mượn")]
        public int TotalBookCheckingOut { get; set; } = 0;

        public CardStatus CardStatus { get; set; } = CardStatus.CONFIRMING;

        public string Option { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime ExpiredDate { get; set; }
        #endregion
    }
}
