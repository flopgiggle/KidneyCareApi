using System;
using KidneyCareApi.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Antlr.Runtime.Tree;
using KidneyCareApi.Dal;
using KidneyCareApi.Dto;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;


namespace KidneyCareApi.Controllers
{
    /// <summary>
    /// 消息处理接口
    /// </summary>
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {

        private HttpItem GetHttpItem()
        {
            var item = new HttpItem
            {
                Encoding = null, //编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                //Encoding = Encoding.Default,
                Method = "get", //URL     可选项 默认为Get
                Timeout = 100000, //连接超时时间     可选项默认为100000
                ReadWriteTimeout = 30000, //写入Post数据超时时间     可选项默认为30000
                IsToLower = false, //得到的HTML代码是否转成小写     可选项默认转小写
                //Cookie = "",//字符串Cookie     可选项
                UserAgent =
                    "Mozilla/5.0 (Windows NT 10.0; WOW64)AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36", //用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "text/html, application/xhtml+xml, */*", //    可选项有默认值
                ContentType = "text/html", //返回类型    可选项有默认值
                //Referer = "http://www.jd.com",//来源URL     可选项
                Allowautoredirect = true, //是否根据３０１跳转     可选项
                //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
                Connectionlimit = 1024, //最大连接数     可选项 默认为1024
                //Postdata = "C:\\PERKYSU_20121129150608_ScrubLog.txt",//Post数据     可选项GET时不需要写
                //PostDataType = PostDataType.FilePath,//默认为传入String类型，也可以设置PostDataType.Byte传入Byte类型数据
                //ProxyPwd = "123456",//代理服务器密码     可选项
                //ProxyUserName = "administrator",//代理服务器账户名     可选项
                ResultType = ResultType.String //返回数据类型，是Byte还是String
                //PostdataByte = System.Text.Encoding.Default.GetBytes("测试一下"),//如果PostDataType为Byte时要设置本属性的值
                //CookieCollection = new System.Net.CookieCollection(),//可以直接传一个Cookie集合进来
            };
            return item;
        }

        /// <summary>
        /// 向腾讯发起请求获取用户唯一openId
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getUserInfo/{code}/{openId}")]
        public ResultPakage<GetUserInfoDto> GetUserInfo(string code, string openId)
        {
            //HttpClient http = new HttpClient();

            var http = new HttpHelper();
            var item = GetHttpItem();
            item.URL = "https://api.weixin.qq.com/sns/jscode2session?appid=wx941fffa48c073a0d&secret=1b71efd31775ec025045185b951e0296&js_code=" + code + "&grant_type=authorization_code";
            item.Method = "get";
            item.Accept = "image/webp,image/*,*/*;q=0.8";
            item.ResultType = ResultType.Byte;
            var result = http.GetHtml(item).Html;

            Util.AddLog(new LogInfo(){Describle = "code:"+ code+"  result:"+result });

            //在微信获取OpenId
            //var openId = openId;
            //根据OpenId获取用户信息
            var db = new Db();
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();
            var user = patient.User;
            //查询不到信息则返回空用户信息,则创建一个新的用户
            if (user == null)
            {
                //创建用户账户信息
                user = new User();
                user.OpenId = openId;
                user.CreateTime = DateTime.Now;
                user.Status = (int)UserStatusType.Registered;
                db.Users.Add(user);

                //创建病人
                var newPatient = new Patient();
                newPatient.User = user;
                db.Patients.Add(newPatient);
                db.SaveChanges();
            }
            var returnUserInfo = new GetUserInfoDto();
            returnUserInfo.CreateTime = user.CreateTime?.ToString("yyyy-MM-dd");
            returnUserInfo.Birthday = user.Birthday;
            returnUserInfo.UserName = user.UserName;
            returnUserInfo.MobilePhone = user.MobilePhone;
            returnUserInfo.IdCard = user.IdCard;
            returnUserInfo.Sex = user.Sex;
            returnUserInfo.Status = user.Status.ToString();
            Patient returnPatient = new Patient();
            returnUserInfo.Patient = returnPatient;
            returnUserInfo.Patient.BelongToDoctor = patient.BelongToDoctor;
            returnUserInfo.Patient.BelongToHospital = patient.BelongToHospital;
            returnUserInfo.Patient.BelongToNurse = patient.BelongToNurse;
            returnUserInfo.Patient.BindStatus = patient.BindStatus;
            returnUserInfo.Patient.CKDLeave = patient.CKDLeave;
            returnUserInfo.Patient.DiseaseType = patient.DiseaseType;
            return Util.ReturnOkResult(returnUserInfo);


            //var result = http.GetAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx941fffa48c073a0d&secret=1b71efd31775ec025045185b951e0296&js_code="+ code + "&grant_type=authorization_code").Result.Content.ToString();
            //return Util.ReturnOkResult(user.Select(a=>new User{OpenId = a.OpenId,MobilePhone = a.MobilePhone,Sex = a.Sex,Birthday = a.Birthday}).First());


            //var httphelper = new HttpHelper(true, accountInfo.virtualAccount);
            //var item = new HttpItem();
            //item.URL = "https://passport.jd.com/uc/showAuthCode";
            //item.Method = "get";
            //item.Postdata = "{'loginName':" + accountInfo.realAccount + "}";
            //item.PostDataType = PostDataType.String;
            //item.ResultType = ResultType.String;
            ////item.ContentType = "application/json";
            //item.Accept = "application/json";
            //item.Header = header;
            //var rs = httphelper.GetHtml(item);
            //var result = JObject.Parse(rs.Html.Replace("(", "").Replace(")", ""));
            //if (result["verifycode"].Value<string>() != "True")
            //{
            //    return false;
            //}

            //return true;
            //var ret = client.Enable(id);
            //return ret;
        }

        [HttpPost]
        //[OpenApi]
        [Route("updateUserInfo")]
        public ResultPakage<bool> UpdateUserInfo(UserRegistDto dto)
        {
            var db = new Db();
            //获取用户并且设置用户信息
            var user = db.Users.First(a => a.OpenId == dto.OpenId);
            //检查此用户是否已存在
            user.Birthday = dto.Birthday;
            user.CreateTime = DateTime.Now;
            user.OpenId = dto.OpenId;
            user.MobilePhone = dto.MobilePhone;
            user.Sex = dto.Sex;
            user.Status = (int)UserStatusType.Registered;
            user.OpenId = dto.OpenId;
            user.IdCard = dto.IdCard;
            user.UserName = dto.UserName;


            //获取病人并且设置病人信息
            var patient = db.Users.First(a => a.OpenId == dto.OpenId).Patients.First();
            var doctor = db.Doctors.First(a => a.Id == dto.BelongToDoctor);
            var nurse = db.Nurses.First(a => a.Id == dto.BelongToNurse);
            var hospital = db.Hospitals.First(a => a.Id == dto.BelongToHospital);
            patient.User = user;
            patient.CKDLeave = int.Parse(dto.CKDLeave);
            patient.DiseaseType = int.Parse(dto.DiseaseType);
            patient.Hospital = hospital;
            patient.Doctor = doctor;
            patient.Nurse = nurse;
            patient.BindStatus = (hospital == null ? "1" : "0") + (doctor == null ? "1" : "0") +
                                 (nurse == null ? "1" : "0");
            patient.CreateTime = DateTime.Now;
            

            db.SaveChanges();


            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("sendMessage")]
        public ResultPakage<bool> SendMessage(SendMssageDto dto)
        {
            var db = new Db();
            Dal.Message message = new Dal.Message();
            //检查用户角色，如果为医生，则需要转换touser为患者用户id
            var usertype = db.Users.Where(a => a.Id == dto.FromUser).FirstOrDefault().UserType;


            message.ToUser = dto.ToUser;
            if ((int) UserType.Nures == usertype || (int) UserType.Doctor == usertype)
            {
                message.ToUser = db.Patients.Where(a => a.Id == dto.ToUser).Select(a => a.UserId).FirstOrDefault();
            }
            //如果为患者,需要转换touser为医生的userid
            if ((int)UserType.Patient == usertype)
            {

            }
            message.FromUser = dto.FromUser;
            message.Messge = dto.Message;
            message.CreateTime = DateTime.Now;
            message.IsRead = false;

            db.Messages.Add(message);
            db.SaveChanges();
            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("sendfeedback")]
        public ResultPakage<bool> Sendfeedback(SendMssageDto dto)
        {
            var db = new Db();
            Dal.Feedback message = new Dal.Feedback();
            message.UserId = dto.FromUser;
            message.Message = dto.Message;
            message.CreateTime = DateTime.Now;


            db.Feedbacks.Add(message);
            db.SaveChanges();
            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("getMessage")]
        public ResultPakage<List<GetMessageReturnDto>> GetMessage(GetMssageDto dto)
        {
            var db = new Db();

            var returnMessageList = db.Messages.Where(a=>a.FromUser == dto.UserId || a.ToUser == dto.UserId).Select(a=>new {a.CreateTime,a.FromUser,a.ToUser,a.Messge,a.IsRead,a.Id,a.User.UserType}).OrderBy(a=>a.CreateTime).ToList();
            var allNotReadMessage = db.Messages.Where(a => a.ToUser == dto.UserId && a.IsRead == false);
            allNotReadMessage.ForEach(a =>
            {
                a.IsRead = true;
            });
            db.SaveChanges();

            List<GetMessageReturnDto> messageList = new List<GetMessageReturnDto>();
            returnMessageList.ForEach(a =>
            {
                GetMessageReturnDto mesage = new GetMessageReturnDto();
                mesage.ToUser = a.ToUser;
                mesage.FromUser = a.FromUser;
                mesage.Id = a.Id;
                mesage.UserType = a.UserType != null?int.Parse(a.UserType.ToString()):0;
                mesage.CreateTime = a.CreateTime?.ToString("yyyy-MM-dd hh:mm:ss");
                mesage.Messge = a.Messge;
                mesage.IsRead = a.IsRead;
                messageList.Add(mesage);
            });

            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(messageList);
        }

        /// <summary>
        /// 获取指定病人的聊天记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("getMessageForPatient")]
        public ResultPakage<List<GetMessageReturnDto>> GetMessageForPatient(GetMssageDto dto)
        {
            var returnDto = GetMessage(dto);
            var db = new Db();
            var firstOrDefault = db.Patients.FirstOrDefault(a => a.Id == dto.PatientId);
            if (firstOrDefault != null)
            {
                var pUserId = firstOrDefault.User.Id;
                //只查指定两人的对话信息
                returnDto.Result = returnDto.Result.Where(a => (a.ToUser==dto.UserId || a.ToUser==pUserId) && (a.FromUser == dto.UserId || a.FromUser == pUserId)).ToList();
            }
            //db.Users.Add(new User(){Doctors = });
            return returnDto;
        }

    }
}
