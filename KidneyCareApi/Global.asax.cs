using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using KidneyCareApi.Common;
using KidneyCareApi.Dto;

namespace KidneyCareApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //请求异常拦截处理
            GlobalConfiguration.Configuration.Filters.Add(new ApiExceptionFilterAttribute());
            //开启独立线程对日志进行处理，解决日志在并发情况下的线程安全问题
            QueueProcess.LogInfoQueueProcess();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Dal.PatientsData, CurrentInfoListDto>();
            });
        }
    }
}
