using Nelibur.ObjectMapper;
using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using Web2023_BE.ApplicationCore.Entities;

namespace Web2023_BE.ApplicationCore.Extensions
{
    public static class TinyMapperExtension
    {
        public static void Bind()
        {
            TinyMapper.Bind<Account, AccountClientDTO>();
        }
    }
}
