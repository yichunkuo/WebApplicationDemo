using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplicationDemo.Enums
{
    public enum UserStatus
    {
        [Description("帳號失效")]
        AccountDisabled = 0,

        [Description("待審核")]
        PendingApproval = 1,

        [Description("核可")]
        Approved = 2,

        [Description("啟用")]
        Active = 3
    }
}