// /**************************************************************************************************
//  * Author:     Liwenhai
//  * FileName:   ToGo-ToGo.WebApi-AuthorizerFilterAttribute.cs
//  * FrameWork:  4.5.2
//  * CreateDate: 2015-11-15 20:08
//  * UpdateDate: 2015-12-22 15:49
//  * Description:权限检查
//  * 
//  * ************************************************************************************************/

using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KidneyCareApi.Common
{
    /// <summary>
    /// api 授权验证
    /// </summary>
    public class AuthorizerFilterAttribute : ActionFilterAttribute
    {
        private HttpActionContext currentActionContext;

        /// <summary>
        /// 在调用操作方法之前发生。
        /// </summary>
        /// <param name="actionContext">操作上下文。</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            currentActionContext = actionContext;
            //检查App版本号是否有操作权限
            //CheckAppVerson();
            //检查用户操作权限开关
            if (Util.GetConfigByName("IsCheckPermissions") == "true")
            {
                //用户是否有操作权限
                CheckAuthorize(actionContext);
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }

        /// <summary>
        /// 验证app版本是否允许方法接口
        /// 此验证用于app在使用过程中,如果app发送强制升级行为,后台会阻止app访问接口
        /// 避免产生脏数据
        /// </summary>
        private void CheckAppVerson()
        {
            //Step1 检查是否有版本号上报,如果有版本号上报则为app请求,需要验证app版本
            var headers = HttpContext.Current.Request.Headers;
            var appId = headers["AppId"];
            if (appId != null && appId.Length > 3)
            {
                var versonInfo = appId.Split('.');
                //Step2 分平台验证,版本号和服务器端版本号是否一致 
                //AppId 样本范例 1.1.1.0.104 第一位1 Android 第二位1货主端 之后数字为真实版本号
                //当接收请求来自Android
                if (int.Parse(versonInfo[0]) == (int)RequestSource.Android)
                {
                    CheckAndroidRequestVerson(int.Parse(versonInfo[1]), appId.Substring(4));
                }
            }
        }

        /// <summary>
        /// 检查Android版本号是否与服务器一致
        /// </summary>
        private void CheckAndroidRequestVerson(int clientType,string requestVerson)
        {
            //货主端
            if (clientType == (int)ClientType.GoodsOwner)
            {
                IsCurrentAppRequestAllow("hzCompel", "hzVersion", requestVerson);
            }

            //司机端
            if (clientType == (int)ClientType.Driver)
            {
                IsCurrentAppRequestAllow("sjCompel", "sjVersion", requestVerson);
            }

            //合同车端
            if (clientType == (int)ClientType.ContractCar)
            {
                IsCurrentAppRequestAllow("htCompel", "htVersion", requestVerson);
            }
        }

        /// <summary>
        /// 判定app请求的版本号,是否被允许请求
        /// </summary>
        /// <param name="compelConfig"></param>
        /// <param name="versionConfigName"></param>
        /// <param name="requestVerson"></param>
        /// <returns></returns>
        private bool IsCurrentAppRequestAllow(string compelConfig,string versionConfigName,string requestVerson)
        {
            var isForcedUpdate = Util.GetConfigByName(compelConfig);
            var goodsOwnerVerson = Util.GetConfigByName(versionConfigName);
            //如果是强制更新,且版本号不相同
            if (isForcedUpdate == "1" && requestVerson != goodsOwnerVerson)
            {
                //抛出错误信息
                AppVersonError();
                return false;
            }
            return true;
        }



        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="actionContext"></param>
        private void CheckAuthorize(HttpActionContext actionContext)
        {
            //获取访问地址
            var url = actionContext.Request.RequestUri.ToString();
            //获取token
            var token = SSOManager.GetToken();

            if (!string.IsNullOrEmpty(token))
            {
                var user = SSOManager.GetUser();

                //判定用户是否有当前操作的权限
                if (!user.ActionList.Any(a => a.ActionUrl.Contains(url)))
                {
                    AuthorizFail(actionContext, "没有此操作的权限");
                }
            }
            else
            {
                //如果为公开API则直接允许访问
                if (!IsOpenApi(actionContext))
                {
                    AuthorizFail(actionContext);
                }
            }
            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 检查方法是不是openapi
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private bool IsOpenApi(HttpActionContext actionContext)
        {
            var attrs =
                ((ReflectedHttpActionDescriptor)
                    ((System.Web.Http.ApiController) actionContext.ControllerContext.Controller).ActionContext
                        .ActionDescriptor).MethodInfo.CustomAttributes;

            return attrs.Any(item => item.AttributeType == typeof (OpenApiAttribute));
        }

        /// <summary>
        /// 授权失败
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="msg"></param>
        public void AuthorizFail(HttpActionContext actionContext,string msg="")
        {
            actionContext.Response = actionContext.Request.CreateResponse(Util.ReturnFailResult<string>("授权认证失败",Util.ErrorCode.AuthorFail));
        }

        /// <summary>
        /// app版本错误
        /// </summary>
        public void AppVersonError()
        {
            currentActionContext.Response = currentActionContext.Request.CreateResponse(Util.ReturnFailResult<string>("请在个人中心更新版本后再使用", Util.ErrorCode.AppVersonError));
        }

        /// <summary>
        /// 接口调用结束后统一执行savechanges
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //DBSessionFactory.CreateDbSession().SaveChanges();
        }
    }

    /// <summary>
    /// 请求来源
    /// </summary>
    public enum RequestSource
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("来自Android")]
        Android = 1
    }

    /// <summary>
    /// 客户端类型
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// 货主端
        /// </summary>
        [Description("货主端")]
        GoodsOwner = 1,
        /// <summary>
        /// 司机端
        /// </summary>
        [Description("司机端")]
        Driver = 2,
        /// <summary>
        /// 合同车端
        /// </summary>
        [Description("合同车端")]
        ContractCar = 3
    }


}