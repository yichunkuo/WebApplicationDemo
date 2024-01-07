using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.Util;
using WebApplicationDemo.Enums;
using WebApplicationDemo.Helper;
using WebApplicationDemo.Models;
using System.Transactions;
using System.IO;

namespace WebApplicationDemo.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController()
        {

        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            string errorMes = "";
            if (ModelState.IsValid)
            {
                //檢查檔案
                if (model.QualificationAttachment != null && FileHelper.IsFileValid(model.QualificationAttachment, 5) is false)
                    errorMes += "檔案格式錯誤，或檔案大小大於5M\n";

                //檢查帳號有沒有重複
                var userItem = db.users.FirstOrDefault(x => x.user_account == model.Account);
                if (userItem != null)
                    errorMes += "帳號重複，請重新修改帳號\n";

                //檢查單位
                var orgItem = db.orgs.FirstOrDefault(x => x.org_no == model.org.OrganizationNumber);
                if (orgItem is null && string.IsNullOrEmpty(model.org.OrganizationName))
                {
                    errorMes += "查無此單位，請點選建立單位並填寫單位名稱\n";
                }

                //檢查密碼複雜度
                if (HashHelper.CheckWorkComplex(model.Pwd, 3) is false)
                {
                    errorMes += "密碼複雜度不符合，大小寫、數字、特殊符號要至少三種\n";
                }

                if (errorMes != "")
                {
                    ModelState.AddModelError("ErrorMessage", errorMes);
                    return View(model);
                }
                else
                {
                    using (var scope = new TransactionScope())
                    {
                        try
                        {
                            //建立單位
                            if (orgItem is null)
                            {
                                orgItem = new orgs
                                {
                                    title = model.org.OrganizationName,
                                    org_no = model.org.OrganizationNumber,
                                    created_at = DateTimeOffset.Now
                                };

                                db.orgs.Add(orgItem);
                                db.SaveChanges();
                            }

                            //建立user
                            string salt = HashHelper.GenerateRandomSalt(16);
                            string hash = HashHelper.HashWord(model.Pwd, salt);

                            users user = new users
                            {
                                user_name = model.Name,
                                user_account = model.Account,
                                user_hash = hash,
                                user_salt = salt,
                                email = model.Email,
                                birthday = model.Birthday,
                                org_id = orgItem.id,
                                user_type = (int)UserType.Normal,
                                status = (int)UserStatus.PendingApproval,
                                created_at = DateTimeOffset.Now
                            };

                            db.users.Add(user);
                            db.SaveChanges();

                            //建立附檔
                            apply_file file = new apply_file
                            {
                                user_id = user.id,
                                file_path = FileHelper.SaveUploadedFileToLocal(model.QualificationAttachment, "UploadedFile"),
                                created_at = DateTimeOffset.Now
                            };

                            db.apply_file.Add(file);
                            db.SaveChanges();

                            scope.Complete();

                            TempData["Message"] = "恭喜註冊成功，請等待審查";
                            return RedirectToAction("Index", "Home");

                        }
                        catch (Exception ex)
                        {
                            // 發生異常，寫系統ErrorLog，看用什麼Log套件
                            //
                            //
                            //

                            TempData["Message"] = "註冊發生異常，請聯繫系統管理員";
                            return RedirectToAction("Index", "Home");
                        }
                    }

                }
            }
            else
            {
                //沒攔到的就印出來
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        errorMes += key + "：" + error + "\n";
                    }
                }

                ModelState.AddModelError("ErrorMessage", errorMes);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.users.FirstOrDefault(x => x.user_account == model.Account);
                if (user is null)
                {
                    ModelState.AddModelError("Account", "無此帳號，請重新填寫或註冊");
                    LoginLog(model.Account, "無此帳號");
                    return View(model);
                }

                string hash = HashHelper.HashWord(model.Userword, user.user_salt);
                if (hash != user.user_hash)
                {
                    ModelState.AddModelError("Userword", "密碼錯誤，請重新填寫");
                    LoginLog(model.Account, "密碼錯誤");
                    return View(model);
                }

                switch (user.status)
                {
                    case 0:
                        ModelState.AddModelError("Account", "此帳號已失效請洽管理員");
                        LoginLog(model.Account, "帳號失效");
                        break;
                    case 1:
                        ModelState.AddModelError("Account", "此帳號待審核");
                        LoginLog(model.Account, "帳號待審核");
                        break;
                    case 2:
                        ModelState.AddModelError("Account", "此帳號已核可，開通信已寄至您的信箱");
                        LoginLog(model.Account, "帳號待開通");
                        break;
                    case 3:
                        LoginLog(model.Account, "登入成功");
                        TempData["Message"] = "登入成功！歡迎光臨";
                        return RedirectToAction("Index", "Home");
                    default:
                        break;
                }
            }
            return View(model);
        }


        public ActionResult DownloadFile(int fileId)
        {
            var fileRecord = db.apply_file.Find(fileId);

            if (fileRecord != null)
            {
                // 取得完整的檔案路徑
                string filePath = Path.Combine(Server.MapPath("~"), fileRecord.file_path);

                // 檢查檔案是否存在
                if (System.IO.File.Exists(filePath))
                {
                    // 設置正確的標頭
                    Response.ContentType = MimeMapping.GetMimeMapping(filePath);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));

                    // 寫入檔案內容
                    Response.WriteFile(filePath);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                TempData["Message"] = "下載檔案發生錯誤請聯繫管理員";
            }

            return RedirectToAction("Index", "Home");
        }

        private void LoginLog(string account,string mes)
        {
            syslog log = new syslog();
            log.user_account= account;
            log.login_at= DateTime.Now;
            log.created_at= DateTimeOffset.Now;
            log.ipaddress = GetClientIpAddress();
            log.note = mes;
            db.syslog.Add(log);
            db.SaveChanges();
        }

        private string GetClientIpAddress()
        {
            string ipAddress = string.Empty;

            if (System.Web.HttpContext.Current != null)
            {                
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ipAddress))
                {
                    ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {                    
                    ipAddress = ipAddress.Split(',')[0];
                }
            }
            return ipAddress;
        }


    }
}