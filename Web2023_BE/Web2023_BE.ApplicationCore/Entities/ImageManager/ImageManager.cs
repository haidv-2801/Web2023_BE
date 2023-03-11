﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class ImageManager : BaseEntity
    {
        [Key]
        [IDuplicate]
        [IRequired]
        [Display(Name = "ID ảnh")]
        public Guid ImageID { get; set; }

        [IDuplicate]
        [IRequired]
        [Display(Name = "Tên ảnh")]
        public string ImageName { get; set; }


        [Display(Name = "Đường dẫn URL")]
        public string Url { get; set; }

        [Display(Name = "ID folder chứa ảnh")]
        public Guid FolderID { get; set; }
    }
}
