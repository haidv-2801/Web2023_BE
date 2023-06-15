using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Web2023_BE.ApplicationCore.Enums;
using Web2023_BE.ApplicationCore.Extensions;
using Web2023_BE.Entities;

namespace Web2023_BE.ApplicationCore.Entities
{
    [ConfigTables(TableName = "partner")]
    public class Partner : BaseEntity
    {
        #region Property
        [Key]
        public Guid PartnerID { get; set; } = Guid.NewGuid();

        public string Image { get; set; }

        public string Description { get; set; }

        public PartnerType PartnerType { get; set; }
        #endregion
    }
}
