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
    [Table("Footer")]
    [ConfigTables(uniqueColumns: "Content")]
    public class Footer : BaseEntity
    {
        #region Property
        [Key]
        public Guid FooterID { get; set; } = Guid.NewGuid();

        public string Content { get; set; }
        #endregion
    }
}