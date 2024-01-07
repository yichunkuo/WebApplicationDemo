using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDemo.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "請填寫姓名")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "長度應介於 {2} 到 {1} 字元")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "請填寫Email")]
        [EmailAddress(ErrorMessage = "請輸入有效的Email地址")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請填寫帳號")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "帳號長度應介於 {2} 到 {1} 字元")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請填寫密碼")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "密碼長度應介於 {2} 到 {1} 字元，且複雜度四選三")]
        [Display(Name = "密碼")]
        public string Pwd { get; set; }

        [Required(ErrorMessage = "請上傳申請資格附檔")]
        [Display(Name = "申請資格附檔")]
        public HttpPostedFileBase QualificationAttachment { get; set; }

        [Display(Name = "狀態")]
        public string Status { get; set; } 

        [Required(ErrorMessage = "您必須同意個人資料蒐集同意事項")]
        [Display(Name = "個資蒐集同意事项")]
        public bool Agreement { get; set; }

        public OrgViewModel org { get; set; }

        public string ErrorMessage { get; set; } 

    }

}