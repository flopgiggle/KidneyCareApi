using System.Web.Http.Filters;

namespace KidneyCareApi.Common
{
    /// <summary>
    /// 此标签决定指定的方法,不需要授权认证依然可以让外部调用
    /// 如果已登录用户,则按登录用户权限进行处理
    /// </summary>
    public class OpenApiAttribute : ActionFilterAttribute
    {

    }


}