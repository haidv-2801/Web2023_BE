using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.ApplicationCore.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }
}
