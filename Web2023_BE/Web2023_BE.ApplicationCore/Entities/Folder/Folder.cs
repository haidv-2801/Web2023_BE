using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Web2023_BE.ApplicationCore.Enums;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class Folder : BaseEntity
    {

        [Key]
        [IDuplicate]
        [IRequired]
        [Display(Name = "ID folder")]
        public Guid FolderID { get; set; }

        [IDuplicate]
        [IRequired]
        [Display(Name = "Tên folder")]
        public string FolderName { get; set; }

        [Display(Name = "ID cha")]
        public Guid ParentID { get; set; }

        [Display(Name = "Kiểu folder")]
        public ModuleType ModuleType { get; set; }

    }
}