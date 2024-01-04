using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationDemo.Models;

namespace WebApplicationDemo.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            // 將模型傳遞到視圖
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            // 處理登入請求的動作

            return View();
        }

        public ActionResult Logout()
        {
            // 登出的相應動作

            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            // 處理註冊請求的動作
            return View();
        }


    }
}