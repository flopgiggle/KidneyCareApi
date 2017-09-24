/**************************************************************************************************
 * Author:      ChenJing
 * FileName:    UserType
 * FrameWork:   4.5.2
 * CreateDate:  2015/11/19 10:05:12
 * Description:  枚举-用户类型
 * 
 * ************************************************************************************************/

using System.ComponentModel;

namespace KidneyCareApi.Common
{
    /// <summary>
    /// 枚举-缓存类型,redis使用
    /// </summary>
    public enum CacheKeyGroup
    {
        [Description("Controllers")]
        Controllers = 1,
        [Description("AuthenCode")]
        AuthenCode = 2,
        [Description("Token")]
        Token = 3,
        [Description("JPushMessage")]
        JPushMessage = 4,
        /// <summary>
        /// 城市信息
        /// </summary>
        [Description("Regions")]
        Regions =5,
        /// <summary>
        /// 车辆位置信息
        /// </summary>
        [Description("TruckPosition")]
        TruckPosition = 6
    }

    /// <summary>
    /// 枚举-线程内对象管理类型,callcontext使用
    /// </summary>
    public enum ContextKey
    {
        [Description("TogoUserInfo")]
        UserInfo = 1,
        [Description("TogoDbContext")]
        TogoDbContext = 2,
        [Description("TogoDbSession")]
        TogoDbSession = 3
    }
}
