// /**************************************************************************************************
//  * Author:     Liwenhai
//  * FileName:   ToGo-ToGo.WebApi-APIExceptionFilterAttribute .cs
//  * FrameWork:  4.5.2
//  * CreateDate: 2015-12-20 20:08
//  * UpdateDate: 2015-12-23 11:29
//  * Description:异常信息过滤
//  * 
//  * ************************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace KidneyCareApi.Common
{
    /// <summary>
    /// 异常信息过滤
    /// </summary>
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            //业务异常
            var businessException = context.Exception as BusinessException;
            if (businessException != null)
            {
                BusinessException exception = businessException;
                context.Response = context.Request.CreateResponse
                (
                    HttpStatusCode.OK,
                                    //Util.ErrorCode.TryParse(exception.Code,true,out type);
                    Util.ReturnFailResult<string>(businessException.Message, exception.Code)
                );
                context.Response.Headers.Add("BusinessExceptionCode", exception.Code.ToString());
                context.Response.Headers.Add("BusinessExceptionMessage", exception.Message);
            }
            //其它异常
            else
            {
                context.Response = context.Request.CreateResponse
                (
                    HttpStatusCode.OK,
                    Util.ReturnFailResult<string>(context.Exception.Message,Util.ErrorCode.AutoCatchError)
                );
            }

            var requestInfo = "";
            var requestUrl = "";

            try
            {

                if (HttpContext.Current.Request.RequestType == "POST")
                {
                    //获取当前传入Http输入流
                    var stream = HttpContext.Current.Request.InputStream;  
                    var length = stream.Length;
                    //对当前输入流进行指定字节数的二进制读取
                    var data = HttpContext.Current.Request.BinaryRead((int)length); 
                    //解码为UTF8编码形式的字符串 
                    requestInfo = Encoding.UTF8.GetString(data);
                }

                requestUrl = HttpContext.Current.Request.Url.ToString();
                if (requestInfo.Length > 10000)
                {
                    requestInfo = requestInfo.Substring(0, 9999);
                }

                requestInfo = "\r\n   Header:" + HttpContext.Current.Request.Headers+ " \r\n   Content:" + requestInfo + " " ;
            }
            catch (Exception)
            {
            }

            //添加异常日志入队列，请求立即返回，避免阻塞
            QueueProcess.LogInfoQueue.Enqueue(new LogInfo
            {
                Exception = context.Exception,Describle = "系统自动拦截异常",
                RequestUrl = requestUrl,
                RequestInfo = requestInfo
            });

        }
    }
}