using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class AccountClientDTO 
    {
        #region Property
        public Guid AccountID { get; set; }

        public string Email { get; set; }
        
        public string UserName { get; set; }

        public string FullName { get; set; }
    
        public string Avatar { get; set; }
        #endregion
    }
}
