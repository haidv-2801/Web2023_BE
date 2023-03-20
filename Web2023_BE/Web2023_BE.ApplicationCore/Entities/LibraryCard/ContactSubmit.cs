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
    [ConfigTables(TableName = "contact_submit")]
    public class ContactSubmit : BaseEntity
    {
        #region Property
        [Key]
        public Guid ContactSubmitID { get; set; } = Guid.NewGuid();

        public string Content { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Company { get; set; }

        #endregion
    }
}
