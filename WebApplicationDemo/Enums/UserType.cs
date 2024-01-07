using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplicationDemo.Enums
{
    public enum UserType
    {
        [Description("管理者")]
        Adimn = 1,

        [Description("一般使用者")]
        Normal = 2
    }
}