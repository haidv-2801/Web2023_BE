using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class ImageManagerDTO
    {
        public string ImageName { get; set; }
        public string Url { get; set; }
        public Guid FolderID { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
