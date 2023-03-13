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
    [ConfigTables(TableName = "carousel")]
    public class Carousel : BaseEntity
    {
        #region Property
        [Key]
        public Guid CarouselID { get; set; } = Guid.NewGuid();

        public string CarouselContent { get; set; }

        public string CarouselText { get; set; }
 
        public CarouselSection CarouselSection { get; set; }

        public bool IsShow { get; set; } = true;
        #endregion
    }
}
