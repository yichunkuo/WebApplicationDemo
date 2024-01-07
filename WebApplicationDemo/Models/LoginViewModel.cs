using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDemo.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "請填寫帳號")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "帳號長度應介於 {2} 到 {1} 字元")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請填寫密碼")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "密碼長度應介於 {2} 到 {1} 字元")]
        [Display(Name = "密碼")]
        public string Userword { get; set; }

    }
}