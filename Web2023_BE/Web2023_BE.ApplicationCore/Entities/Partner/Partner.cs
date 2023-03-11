using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Web2023_BE.ApplicationCore.Enums;

namespace Web2023_BE.ApplicationCore.Entities
{
    [Table("Partner")]
    public class Partner : BaseEntity
    {
        #region Property
        [Key]
        public Guid PartnerID { get; set; } = Guid.NewGuid();

        public string Image { get; set; }

        public string Description { get; set; }
        #endregion
    }
}
