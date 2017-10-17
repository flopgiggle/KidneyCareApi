using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace KidneyCareApi.Common
{
    /// <summary>
    /// 登陆令牌管理
    /// </summary>
    public static class SSOManager
    {
        private static readonly int TokenLifeTime = int.Parse(ConfigurationManager.AppSettings["TokenLifeTime"]);
        //验证码过期时间
        private static readonly int AuthenKeyOutTime = int.Parse(ConfigurationManager.AppSettings["AuthenKeyOutTime"]);
       
        static readonly ICacheHelper cacheHelper = new RedisCacheHelper();

        /// <summary>
        /// 新增token,并且返回生成的token
        /// </summary>
        private static string AddToken()
        {
            string newToken = Util.GetNewToken();
            //var messageVerTime = int.Parse(ConfigurationManager.AppSettings["TokenLifeTime"]);
            //CacheHelper.Add("Token", newToken, DateTime.Now.ToShortTimeString(), TimeSpan.FromMinutes(messageVerTime));
            return newToken;
        }

        /// <summary>
        /// 删除token
        /// </summary>
        /// <param name="token"></param>
        public static bool DeleteToken(string token)
        {
            return cacheHelper.DeleteFromGroup(CacheKeyGroup.Token, token);
        }

        /// <summary>
        /// 判定token是否存
        /// </summary>
        /// <param name="token"></param>
        public static bool IsTokenExist(string token)
        {
            var result = cacheHelper.Get<SSOUserInfo>(CacheKeyGroup.Token, token);
            if (result != null)
            {
                return true;
            }
            return false;
        }

        public static string GetToken()
        {
            if (HttpContext.Current != null)
            {
                var headers = HttpContext.Current.Request.Headers;
                var cookies = HttpContext.Current.Request.Cookies;
                var tokenFromHeaders = headers["Token"];
                var tokenFromCookies = cookies["Token"] != null ? cookies["Token"].Value : "";
                var token = !string.IsNullOrEmpty(tokenFromHeaders) ? tokenFromHeaders : tokenFromCookies;
                return token;
            }
            throw new BusinessException(Util.ErrorCode.UserInfoGetError, "请求已失效");
        }

        /// <summary>
        /// 判断是否登陆
        /// </summary>
        /// <returns></returns>
        public static bool IsLogin()
        {
            try
            {
                var user = GetUser();
                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        public static SSOUserInfo GetUserInfoFromHeader() {
            SSOUserInfo user = new SSOUserInfo();
            if (HttpContext.Current != null)
            {
                var headers = HttpContext.Current.Request.Headers;
                if (headers["userId"] != null) {
                    user.Id = int.Parse(headers["userId"]);
                }
                
            }
            return user;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public static SSOUserInfo GetUser()
        {
            
            //查询当前线程是否已经获取过userinfo,如果获取过则直接返回
            var userInfo = Util.GetFromCallContext<SSOUserInfo>(ContextKey.UserInfo);
            if (userInfo != null)
            {
                return userInfo;
            }
            //获取token
            var token = GetToken();
            //token不为空则开始在redis中查找是否存在用户会话信息
            if (!string.IsNullOrEmpty(token))
            {
                var user = cacheHelper.Get<SSOUserInfo>(CacheKeyGroup.Token, token);
                if (user != null)
                {
                    //记录userinfo到当前线程
                    Util.SetToCallContext(ContextKey.UserInfo, user);
                    return user;
                }
                throw new BusinessException(Util.ErrorCode.UserInfoGetError, "未找到token对应的用户信息");
            }

            throw new BusinessException(Util.ErrorCode.UserInfoGetError, "获取用户信息时,token不能为空");
        }


        public static SSOUserInfo SetUser(SSOUserInfo user)
        {
            //使用时应该存入token到user
            user.Token = AddToken();
            cacheHelper.Add(CacheKeyGroup.Token, user.Token, user, TimeSpan.FromDays(TokenLifeTime));

            return user;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool UpdateUser(SSOUserInfo user)
        {
            var isSuccess = cacheHelper.Add(CacheKeyGroup.Token, GetToken(), user, TimeSpan.FromDays(TokenLifeTime));
            return isSuccess;
        }

        /// <summary>
        /// 存验证码
        /// </summary>
        /// <param name="codeInfo"></param>
        /// <returns></returns>
        public static bool SetAuthenCode(SSOAuthenCodeInfo codeInfo)
        {
            //使用时应该存入token到user
            var ret= cacheHelper.Add(CacheKeyGroup.AuthenCode, codeInfo.PhoneNumber, codeInfo, TimeSpan.FromSeconds(AuthenKeyOutTime));
            return ret;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public static SSOAuthenCodeInfo GetAuthenCode(string keyValue)
        {
            var data = cacheHelper.Get<SSOAuthenCodeInfo>(CacheKeyGroup.AuthenCode, keyValue);
            return data;

        }
        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="keyValue"></param>
        public static void DeleteAuthenInfo(string keyValue)
        {
            cacheHelper.DeleteFromGroup(CacheKeyGroup.AuthenCode, keyValue);
        }

      

    }
    public class SSOAuthenCodeInfo
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string AuthenCode { get; set; }
        /// <summary>
        /// 验证码输入错误次数
        /// </summary>
        public int CodeErrorTimes { get; set; }
        /// <summary>
        /// 获取验证码次数
        /// </summary>
        public int GetCodeTimes { get; set; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime DateTimeNow { get; set; }
    }
    public class SSOUserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ShowName { get; set; }
        public DateTime? CreateTime { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int? UpdateBy { get; set; }
        public int? UserType { get; set; }
        public string MobiePhone { get; set; }
        public string Token { get; set; }
        public bool? IsDelete { get; set; }
        //当前所属的组织机构代码
        public string CurrentOrgCode { get; set; }
        public IEnumerable<SSORole> RoleList { get; set; }
        public IEnumerable<SSOAction> ActionList { get; set; }
        //当前用户组织机构代码
        public IEnumerable<string> Organizations { get; set; }
    }

    public class SSORole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Mark { get; set; }
    }

    public class SSOAction
    {
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string ActionUrl { get; set; }
        public int? ActionType { get; set; }
    }
}