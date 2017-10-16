using System;
using System.IO;
using System.Linq;
using System.Web;
using KidneyCareApi.Common;
using KidneyCareApi.Dal;

namespace KidneyCareApi
{
    /// <summary>
    /// UploadHandler 的摘要说明
    /// </summary>
    public class UploadImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                context.Response.ContentType = "multipart/form-data";
                //接收上传后的文件
                HttpPostedFile file = context.Request.Files.Count > 0 ? context.Request.Files[0] : null;
                var reportId = int.Parse(context.Request.Form["reportId"]);
                var num = int.Parse(context.Request.Form["num"]);
                //获取文件的保存路径
                string uploadPath = HttpContext.Current.Server.MapPath(Util.GetConfigByName("UploadPath") + "\\");
                //获取当前时间并格式化
                string randomNumber = Guid.NewGuid().ToString();
                var fileName = randomNumber + file.FileName;
                //判断上传的文件是否为空
                if (file != null)
                {
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    //保存文件

                    file.SaveAs(uploadPath + fileName);
                    context.Response.Write(urlconvertor(fileName));

                    using (Db db = new Db())
                    {
                        var report = db.Reports.First(a => a.Id == reportId);
                        if (num == 0)
                        {
                            report.ImageUrl = fileName;
                        }
                        if (num == 1)
                        {
                            report.ImageUrl1 = fileName;
                        }
                        if (num == 2)
                        {
                            report.ImageUrl2 = fileName;
                        }
                        if (num == 3)
                        {
                            report.ImageUrl3 = fileName;
                        }
                        if (num == 4)
                        {
                            report.ImageUrl4 = fileName;
                        }
                        if (num == 5)
                        {
                            report.ImageUrl5 = fileName;
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    context.Response.Write("上传失败！");
                }
            }
            catch (Exception e)
            {
                context.Response.Write("上传失败！");
                Util.AddLog(new LogInfo
                {
                    Exception = e,
                    Describle = "系统自动拦截异常",
                    RequestUrl = "UploadHandler.ashx",
                    RequestInfo = context.ToString()
                });
            }
        }

        private string urlconvertor(string imagesurl1)
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ""); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");
            return imagesurl2;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}