using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationDemo.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// 檢查檔案的邏輯
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileValid(HttpPostedFileBase file, int size)
        {
            var allowedTypes = new[] { "application/pdf", "application/msword", "text/plain" };
            var maxFileSize = size * 1024 * 1024; // MB

            return file != null && allowedTypes.Contains(file.ContentType) && file.ContentLength <= maxFileSize;
        }

        /// <summary>
        /// 存到專案本身
        /// </summary>
        /// <param name="file"></param>
        /// <param name="targetDirectory"></param>
        /// <returns></returns>
        public static string SaveUploadedFileToLocal(HttpPostedFileBase file, string targetDirectory)
        {
            // 取得專案的實際路徑
            string projectPath = HttpContext.Current.Server.MapPath("~");

            // 指定檔案的相對路徑
            string relativePath = Path.Combine(targetDirectory, $"{Guid.NewGuid().ToString()}{Path.GetExtension(file.FileName)}");

            // 指定檔案的完整路徑
            string filePath = Path.Combine(projectPath, relativePath);

            // 檢查目錄是否存在，如果不存在，則創建目錄
            string absoluteDirectoryPath = Path.Combine(projectPath, targetDirectory);
            if (!Directory.Exists(absoluteDirectoryPath))
            {
                Directory.CreateDirectory(absoluteDirectoryPath);
            }

            // 處理上傳檔案的邏輯
            file.SaveAs(filePath);

            return relativePath;
        }

        /// <summary>
        /// 存去FTP要帳密
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ftpServer"></param>
        /// <param name="ftpUsername"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="targetDirectory"></param>      


    }
}