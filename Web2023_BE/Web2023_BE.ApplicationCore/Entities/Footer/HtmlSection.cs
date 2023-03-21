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
    [ConfigTables(TableName = "html_section", UniqueColumns = "HtmlSectionType")]
    public class HtmlSection : BaseEntity
    {
        #region Property
        [Key]
        public Guid SectionID { get; set; } = Guid.NewGuid();

        public string Content { get; set; }
            
        public HtmlSectionType HtmlSectionType { get; set; }
        #endregion
    }
}
