using KidneyCareApi.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KidneyCareApi
{
    /// <summary>
    /// UploadImages 的摘要说明
    /// </summary>
    public class UploadImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var name = Util.SaveImage("uploadimage", context);
                context.Response.Write("{\"name\":\"" + name + "\"}");
            }
            catch (Exception e)
            {
                context.Response.Write("上传失败！");
                Util.AddLog(new LogInfo
                {
                    Exception = e,
                    Describle = "系统自动拦截异常",
                    RequestUrl = "UploadImage.ashx",
                    RequestInfo = context.ToString()
                });
            }
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