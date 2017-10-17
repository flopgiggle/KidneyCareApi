using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using KidneyCareApi.Common;

namespace KidneyCareApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //注册授权认证配置
            config.Filters.Add(new AuthorizerFilterAttribute());


            //config.Formatters.Insert(0, new System.Net.Http.Formatting.XmlMediaTypeFormatter());
        }
    }
}
