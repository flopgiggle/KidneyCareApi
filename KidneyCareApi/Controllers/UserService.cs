using System;
using KidneyCareApi.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using KidneyCareApi.Dal;
using KidneyCareApi.Dto;
using WebGrease.Css.Extensions;


namespace KidneyCareApi.Controllers
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    [RoutePrefix("api/user")]
    public class UserInfoController : ApiController
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
        [Route("getUserInfo/{code}")]
        public ResultPakage<User> GetUserInfo(string code)
        {
            //HttpClient http = new HttpClient();

            var http = new HttpHelper();
            var item = GetHttpItem();
            item.URL = "https://api.weixin.qq.com/sns/jscode2session?appid=wx941fffa48c073a0d&secret=1b71efd31775ec025045185b951e0296&js_code=" + code + "&grant_type=authorization_code";
            item.Method = "get";
            item.Accept = "image/webp,image/*,*/*;q=0.8";
            item.ResultType = ResultType.Byte;
            var result = http.GetHtml(item).Html;

            //在微信获取OpenId
            var openId = "123456";
            //根据OpenId获取用户信息
            Db db = new Db();
            var user = db.Users.Where(a => a.OpenId == openId);
            //查询不到信息则返回空用户信息
            if (!user.Any())
            {
                return Util.ReturnOkResult(new User(){});
            }

            //var result = http.GetAsync("https://api.weixin.qq.com/sns/jscode2session?appid=wx941fffa48c073a0d&secret=1b71efd31775ec025045185b951e0296&js_code="+ code + "&grant_type=authorization_code").Result.Content.ToString();
            //return Util.ReturnOkResult(user.Select(a=>new User{OpenId = a.OpenId,MobilePhone = a.MobilePhone,Sex = a.Sex,Birthday = a.Birthday}).First());
            var oldUser = user.First();
            var user1 = new User();
            user1.Status = 1;
            return Util.ReturnOkResult(user1);

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

        /// <summary>
        /// 供货商创建更新
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("CreateUpdateSupplier")]
        //public ResultPakage<Supplier> CreateUpdateSupplier(SupplierDto user)
        //{
        //    var ret = client.CreateUpdateSupplier(user);
        //    return ret;
        //}

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("regist")]
        public ResultPakage<bool> Regist(UserRegistDto dto)
        {
            Db db = new Db();
            //检查此用户是否已存在
            if (db.Users.Any(a => a.OpenId == dto.OpenId))
            {
                return Util.ReturnFailResult<bool>("用户已存在");
            }

            var doctor = db.Doctors.First(a => a.Id == dto.BelongToDoctor);
            var nurse = db.Nurses.First(a => a.Id == dto.BelongToNurse);
            var hospital = db.Hospitals.First(a => a.Id == dto.BelongToHospital);

            //创建用户
            Dal.User user = new User();
            user.Birthday = dto.Birthday;
            user.CreateTime = DateTime.Now;
            user.OpenId = dto.OpenId;
            user.MobilePhone = dto.MobilePhone;
            user.Sex = dto.Sex;
            user.Status = 1;
            user.OpenId = dto.OpenId;

            Dal.Patient patient = new Patient();
            patient.User = user;
            patient.Hospital = hospital;
            patient.Doctor = doctor;
            patient.CreateTime = DateTime.Now;

            db.Users.Add(user);
            db.Patients.Add(patient);
            db.SaveChanges();

            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
        }

        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string GetNameByCode(int code)
        {
           return ((PatientsDataType) code).GetEnumDes();
        }

        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        private string GetNameByFormType(int formType)
        {
            return ((PatientsDataType)formType).GetEnumDes();
        }

        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        private string GetNameByReportType(int reportType)
        {
            return ((ReportType)reportType).GetEnumDes();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCurrentDayInfoList/{queryDate}/{openId}")]
        public ResultPakage<CurrentInfoReturnDto> GetCurrentDayInfoList(string queryDate,string openId)
        {
            Db db = new Db();
            CurrentInfoReturnDto returnDto = new CurrentInfoReturnDto();
            
            List <List<CurrentInfoListDto>> MyRecord = new List<List<CurrentInfoListDto>>();
            List <List<CurrentInfoListDto>> MyReport = new List<List<CurrentInfoListDto>>();

            //根据openid 查询病人信息
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();
            //var patient = db.Patients.First(a => a.User.OpenId == openId);
            //病人当天的所有自我记录信息
            var patientData = db.PatientsDatas.Where(a => a.RecordDate == queryDate && a.ReportId == null).Select(a => new { a.DataCode, a.DataValue ,a.CreateTime,a.RecordTime,a.FormType }).ToList();
            //病人当天的所有的报告信息
            List< CurrentInfoListDto > reportList = new List<CurrentInfoListDto>();  

            var reportData = db.Reports.Where(a => a.ReportDate == queryDate && a.PatientId == patient.Id).Select(a=>new{a.CreateTime,a.ReportType,a.ReportDate}).ToList();
            var reportDetailDatas = db.Reports.Where(a=> a.ReportDate == queryDate && a.PatientId == patient.Id).SelectMany(a => a.PatientsDatas.Select(b=>new { b.DataCode, b.DataValue, b.CreateTime, b.RecordTime})).ToList();
            reportData.ForEach(a =>
            {
                CurrentInfoListDto recordDto= new CurrentInfoListDto();
                recordDto.ReportName = GetNameByReportType(a.ReportType??1);
                recordDto.CreateTime = a.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                reportList.Add(recordDto);
            });

            reportDetailDatas.ForEach(item =>
            {
                CurrentInfoListDto oneReturnDto = new CurrentInfoListDto();
                oneReturnDto.DataValue = item.DataValue;
                oneReturnDto.CreateTime = item.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                oneReturnDto.DataCode = item.DataCode;
                oneReturnDto.RecordTime = item.RecordTime;
                oneReturnDto.DataName = GetNameByCode(item.DataCode ?? 9);
                reportList.Add(oneReturnDto);
            });

            reportList.GroupBy(a => a.CreateTime).ForEach(a =>
            {
                List<CurrentInfoListDto> MyRecordList = new List<CurrentInfoListDto>();
                a.ForEach(item =>
                {
                    MyRecordList.Add(item);
                });
                MyReport.Add(MyRecordList);
            });



        //合并数据
        //Step1 病人当天数据按创建时间进行分组
            patientData.GroupBy(a => a.CreateTime).ForEach(a =>
            {
                List<CurrentInfoListDto> MyRecordList = new List<CurrentInfoListDto>();
                a.ForEach(item =>
                {
                    CurrentInfoListDto oneReturnDto = new CurrentInfoListDto();
                    oneReturnDto.DataValue = item.DataValue;
                    oneReturnDto.CreateTime = item.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                    oneReturnDto.DataCode = item.DataCode;
                    oneReturnDto.RecordTime = item.RecordTime;
                    if (item.FormType != 0)
                    {
                        oneReturnDto.DataName = GetNameByFormType(item.FormType ?? 0);
                    }
                    else
                    {
                        oneReturnDto.DataName = GetNameByCode(item.DataCode??9);
                    }
                    MyRecordList.Add(oneReturnDto);
                });
                MyRecord.Add(MyRecordList);
            });
            //Step2 病人当天数据按创建时间进行分组

            returnDto.MyRecord = MyRecord;
            returnDto.MyReport = MyReport;


            return Util.ReturnOkResult(returnDto);
        }

        /// <summary>
        /// 查询报告历史记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getReportHistory/{year}/{openId}")]
        public ResultPakage<List<ReportHistoryReturnDto>> GetReportHistory(string year, string openId)
        {
            Db db = new Db();
            List<ReportHistoryReturnDto> returnDto = new List<ReportHistoryReturnDto>();
            var startDate = year + "-" + "01" + "-"+"01";
            var endDate = year + "-" + "12" + "-" + "31";
            //查询病人历史的数据
            //根据openid 查询病人信息
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();
            //病人当年的所有的报告信息
            var reportData = db.Reports.Where(a => a.ReportDate.CompareTo(startDate)>0 && a.ReportDate.CompareTo(endDate)<0 && a.PatientId == patient.Id).Select(a => new { a.CreateTime, a.ReportType, a.ReportDate }).ToList();


            var reportDetailDat2as10 = db.PatientsDatas
                .Where(a => a.PatientId == patient.Id).ToList();

            var reportDetailDa44tas10 = db.PatientsDatas
                .Where(a =>a.Report.PatientId == patient.Id).ToList();

            var reportDetailDatas10 = db.PatientsDatas
                .Where(a => a.PatientId == patient.Id && a.Report.PatientId == patient.Id).ToList();

            //获取当年的所有指标记录,如果一天中有重复的则取最新的一次结果
            var reportDetailDatas1 = db.PatientsDatas
                .Where(a => a.PatientId == patient.Id && a.Report.PatientId == patient.Id)
                .GroupBy(b => new {b.RecordDate, b.DataCode}).ToList();

            var reportDetailDatas12= db.PatientsDatas
                .Where(a => a.PatientId == patient.Id && a.Report.PatientId == patient.Id)
                .GroupBy(b => b.RecordDate).ToList();


            //获取当年的所有指标记录,如果一天中有重复的则取最新的一次结果
            var reportDetailDatas = db.PatientsDatas.Where(a => a.PatientId == patient.Id && a.Report.PatientId == patient.Id)
                .GroupBy(b => new {b.RecordDate, b.DataCode}).Select(c => new
                {
                    RecordDate = c.Max(x => x.RecordDate),
                    RecordTime = c.Max(x => x.RecordTime),
                    DataCode = c.Max(x => x.DataCode),
                    DataValue = c.Max(x => x.DataValue),
                    CreateTime = c.Max(x => x.CreateTime)
                }).ToList();

            //var reportDetailDatas = db.Reports.Where(a => a.ReportDate.CompareTo(startDate) > 0 && a.ReportDate.CompareTo(endDate) < 0 && a.PatientId == patient.Id)
               //                               .SelectMany(a => a.PatientsDatas.GroupBy(b=>new {b.RecordDate,b.DataCode})
                //                                        .Select(c => new { RecordDate = c.Max(x => x.RecordDate), RecordTime =c.Max(x=>x.RecordTime), DataCode = c.Max(x => x.DataCode), DataValue = c.Max(x => x.DataValue), CreateTime = c.Max(x => x.CreateTime) })).ToList();

            //根据指标类型进行数据分类
            reportDetailDatas.GroupBy(a => a.DataCode).ForEach(a =>
            {
                ReportHistoryReturnDto historyDto = new ReportHistoryReturnDto();
                List<string> Xxdata = new List<string>();
                List<string> Values = new List<string>();
                a.OrderBy(b=>b.RecordTime).ForEach(item =>
                {
                    Xxdata.Add(item.RecordDate);
                    Values.Add(item.DataValue);
                    historyDto.Name = GetNameByCode(item.DataCode ?? 9);
                    historyDto.UnitName = "待定";
                    historyDto.DataCode = "code"+item.DataCode;
                });
                historyDto.Values = Values;
                historyDto.Xdata = Xxdata;
                returnDto.Add(historyDto);
            });

            return Util.ReturnOkResult(returnDto);
        }
    }
}
