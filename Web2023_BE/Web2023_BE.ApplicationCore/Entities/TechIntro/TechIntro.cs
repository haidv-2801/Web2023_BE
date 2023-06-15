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
    [ConfigTables(TableName = "technology_introduction")]
    public class TechIntro : BaseEntity
    {
        #region Property
        [Key]
        public Guid TechID { get; set; } = Guid.NewGuid();

        public string Content { get; set; }
 
        public string Image { get; set; }

        public int Order { get; set; } = 0;

        public string Title { get; set; }

        public bool IsShow { get; set; } = false;
        #endregion
    }
}
