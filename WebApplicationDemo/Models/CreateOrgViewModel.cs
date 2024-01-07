using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationDemo.Models
{
    public class OrgViewModel
    {
        [Required(ErrorMessage = "請填寫單位編號")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "長度應介於 {2} 到 {1} 字元")]
        [Display(Name = "單位編號")]
        public string OrganizationNumber { get; set; }

   
        [Display(Name = "單位名稱")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "長度應介於 {2} 到 {1} 字元")]
        public string OrganizationName { get; set; }
    }
}