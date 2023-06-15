using Web2023_BE.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web2023_BE.ApplicationCore.Entities
{
    public class AccountLoginViewDTO : Account
    {
        #region Property
        List<Role> roles;
        #endregion
    }
}
