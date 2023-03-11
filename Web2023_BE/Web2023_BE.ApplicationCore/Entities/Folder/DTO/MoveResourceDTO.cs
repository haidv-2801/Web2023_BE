using System;
using System.Collections.Generic;
using System.Text;
using Web2023_BE.ApplicationCore.Enums;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class MoveResourceDTO
    {

        public string DestinationFolderID { get; set; }

        public List<string> ListIdsFolderIgnore { get; set; }

        public List<string> ListIdsFolderSelect { get; set; }

        public List<string> ListIdsResourceIgnore { get; set; }

        public List<string> ListIdsResourceSelect { get; set; }

        public ModuleType ModuleType { get; set; }

        public PagingRequest PagingRequest { get; set; }
    }
}
