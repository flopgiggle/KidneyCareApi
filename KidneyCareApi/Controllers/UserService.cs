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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        [HttpPost]
        [Route("getUserInfo")]
        public ResultPakage<GetUserInfoDto> GetUserInfo(GetUserInfoParamsDto paramsDto)
        {
            //HttpClient http = new HttpClient();
            if(paramsDto.OpenId == ""){
                var http = new HttpHelper();
                var item = GetHttpItem();
                item.URL = "https://api.weixin.qq.com/sns/jscode2session?appid=wx941fffa48c073a0d&secret=1b71efd31775ec025045185b951e0296&js_code=" + paramsDto.Code + "&grant_type=authorization_code";
                item.Method = "get";
                item.Accept = "image/webp,image/*,*/*;q=0.8";
                item.ResultType = ResultType.Byte;
                var result = http.GetHtml(item).Html;
                var jsonResult = JObject.Parse(result);
                if (jsonResult["openid"] != null)
                {
                    Util.AddLog(new LogInfo() { Describle = "GetUserInfo" + jsonResult["openid"] });
                    paramsDto.OpenId = jsonResult["openid"].Value<string>();
                }
            };

            if (paramsDto.OpenId == "")
            {
                return Util.ReturnFailResult<GetUserInfoDto>("未能获取到openid");
            }

            //在微信获取OpenId
            //var openId = openId;
            //根据OpenId获取用户信息
            var db = new Db();
            var user = db.Users.FirstOrDefault(a => a.OpenId == paramsDto.OpenId);
            //var user = patient.User;
            //查询不到信息则返回空用户信息,则创建一个新的用户
            if (user == null)
            {
                //创建用户账户信息
                user = new User();
                user.OpenId = paramsDto.OpenId;
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
            returnUserInfo.Id = user.Id.ToString();
            returnUserInfo.UserName = user.UserName;
            returnUserInfo.MobilePhone = user.MobilePhone;
            returnUserInfo.IdCard = user.IdCard;
            returnUserInfo.OpenId = user.OpenId;
            returnUserInfo.Sex = user.Sex;
            returnUserInfo.Status = user.Status.ToString();
            var patient = db.Users.First(a => a.OpenId == paramsDto.OpenId).Patients.FirstOrDefault();
            if (patient.Doctor != null)
            {
                returnUserInfo.BelongToDoctorId = patient.Doctor.Id.ToString();
            }
            if (patient.Nurse != null)
            {
                returnUserInfo.BelongToNurseId = patient.Nurse.Id.ToString();
            }
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
            patient.BindStatus = (hospital == null ? "0" : "1") + (doctor == null ? "0" : "1") +
                                 (nurse == null ? "0" : "1");
            patient.CreateTime = DateTime.Now;
            

            db.SaveChanges();


            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
        }

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
            var db = new Db();
            //检查此用户是否已存在
            if (db.Users.Any(a => a.OpenId == dto.OpenId))
                return Util.ReturnFailResult<bool>("用户已存在");

            var doctor = db.Doctors.First(a => a.Id == dto.BelongToDoctor);
            var nurse = db.Nurses.First(a => a.Id == dto.BelongToNurse);
            var hospital = db.Hospitals.First(a => a.Id == dto.BelongToHospital);

            //创建用户
            var user = new User();
            user.Birthday = dto.Birthday;
            user.CreateTime = DateTime.Now;
            user.OpenId = dto.OpenId;
            user.MobilePhone = dto.MobilePhone;
            user.Sex = dto.Sex;
            user.Status = (int)UserStatusType.Registered;
            user.OpenId = dto.OpenId;
            user.IdCard = dto.IdCard;
            user.UserName = dto.UserName;

            var patient = new Patient();
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
            return ((PatientsDataType)code).GetEnumDes();
        }

        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        private string GetNameByFormType(int formType)
        {
            return ((PatientsDataFormType)formType).GetEnumDes();
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
        /// 查询病人指定日期的指标信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getCurrentDayInfoList/{queryDate}/{openId}")]
        public ResultPakage<CurrentInfoReturnDto> GetCurrentDayInfoList(string queryDate, string openId)
        {
            var db = new Db();
            var returnDto = new CurrentInfoReturnDto();

            var MyRecord = new List<List<CurrentInfoListDto>>();
            var MyReport = new List<List<CurrentInfoListDto>>();

            //根据openid 查询病人信息
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();

            //查询当前病人的标准指标信息
            //如果当期病人没有绑定医院信息，则默认使用华西医院的标准指标
            //查询病人医院数据
            var patientHospitalCode = "5101000001";
            if (patient.Hospital != null)
                patientHospitalCode = patient.Hospital.Code;

            var indicators = db.IndicatorsRanges.Where(a => a.Hospital.Code == patientHospitalCode).ToList();

            //var patient = db.Patients.First(a => a.User.OpenId == openId);
            //病人当天的所有自我记录信息
            var patientData = db.PatientsDatas.Where(a => a.RecordDate == queryDate && a.ReportId == null && a.PatientId == patient.Id).Select(a => new { a.DataCode, a.DataValue, a.CreateTime, a.RecordTime, a.FormType }).ToList();
            //病人当天的所有的报告信息
            var reportList = new List<CurrentInfoListDto>();

            var reportData = db.Reports.Where(a => a.ReportDate == queryDate && a.PatientId == patient.Id).Select(a => new { a.CreateTime, a.ReportType, a.ReportDate }).ToList();
            var reportDetailDatas = db.Reports.Where(a => a.ReportDate == queryDate && a.PatientId == patient.Id).SelectMany(a => a.PatientsDatas.Select(b => new { b.DataCode, b.DataValue, b.CreateTime, b.RecordTime })).ToList();
            reportData.ForEach(a =>
            {
                var recordDto = new CurrentInfoListDto();
                recordDto.ReportName = GetNameByReportType(a.ReportType ?? 1);
                recordDto.CreateTime = a.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                reportList.Add(recordDto);
            });

            reportDetailDatas.ForEach(item =>
            {
                var oneReturnDto = new CurrentInfoListDto();
                oneReturnDto.DataValue = item.DataValue;
                oneReturnDto.CreateTime = item.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                oneReturnDto.DataCode = item.DataCode;
                oneReturnDto.RecordTime = item.RecordTime;
                oneReturnDto.DataName = GetNameByCode(item.DataCode ?? 9);
                reportList.Add(oneReturnDto);
            });

            reportList.GroupBy(a => a.CreateTime).ForEach(a =>
            {
                var MyRecordList = new List<CurrentInfoListDto>();
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
                var MyRecordList = new List<CurrentInfoListDto>();
                a.ForEach(item =>
                {
                    var oneReturnDto = new CurrentInfoListDto(); var diastolicPressureValue = "";
                    
                    //如果类型为舒张压,则直接跳过
                    if (item.DataCode != (int)PatientsDataType.DiastolicPressure)
                    {
                        oneReturnDto.CreateTime = item.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                        oneReturnDto.DataCode = item.DataCode;
                        oneReturnDto.DataValue = item.DataValue;
                        oneReturnDto.RecordTime = item.RecordTime;
                        //如果有表单类型0，则说明为自定义表单字段，需要恢复设置的表单值
                        if (item.FormType != 0)
                            oneReturnDto.DataName = GetNameByFormType(item.FormType ?? 0);
                        else
                            oneReturnDto.DataName = GetNameByCode(item.DataCode ?? 9);
                        //判定指标是否正确
                        IndicatorJudge(indicators, oneReturnDto);
                        
                    }

                    //如果类型为收缩压,则找出对应的舒张压，并把舒张压放入收缩压返回值,并设置名称为血压
                    if (item.DataCode == (int) PatientsDataType.SystolicPressure)
                    {
                        //查找对应的舒张压
                        var diastolicPressure = a.FirstOrDefault(x => x.DataCode == (int) PatientsDataType.DiastolicPressure);
                        if (diastolicPressure != null)
                        {
                            oneReturnDto.DataValue = item.DataValue +"/"+ diastolicPressure.DataValue;
                        }
                        //对舒张压值进行判定比较
                        var diastolicPressureDto = new CurrentInfoListDto();
                        diastolicPressureDto.DataCode = diastolicPressure.DataCode;
                        diastolicPressureDto.DataValue = diastolicPressure.DataValue;
                        IndicatorJudge(indicators, diastolicPressureDto);

                        oneReturnDto.DataName = "血压";
                        oneReturnDto.IsNomoal = diastolicPressureDto.IsNomoal && oneReturnDto.IsNomoal;

                    }

                    if (item.DataCode != (int) PatientsDataType.DiastolicPressure)
                    {
                        MyRecordList.Add(oneReturnDto);
                    }
                });
                MyRecord.Add(MyRecordList);
            });
            //Step2 病人当天数据按创建时间进行分组


            //设置指标异常正常数据
            returnDto.MyRecord = MyRecord;
            //returnDto.MyRecord.ForEach(a => a.ForEach(b =>
            //  {
            //      IndicatorJudge(indicators, b);
            //  }));




            returnDto.MyReport = MyReport;
            returnDto.MyReport.ForEach(a => a.ForEach(b =>
            {
                IndicatorJudge(indicators, b);
            }));


            return Util.ReturnOkResult(returnDto);
        }

        private void IndicatorJudge(List<IndicatorsRange> indicators, CurrentInfoListDto b)
        {
            var indicator = indicators.FirstOrDefault(x => x.DataCode == b.DataCode);
            if (indicator != null)
            {
                b.Unit = indicator.Unit;
                //如果指标小于最大值
                if (indicator.Max == null || double.Parse(b.DataValue) < double.Parse(indicator.Max))
                {
                    b.IsNomoal = true;
                }
                else
                {
                    b.IsNomoal = false;
                    return;
                }

                //如果指标小于最大值
                if (indicator.Min == null || double.Parse(b.DataValue) > double.Parse(indicator.Min))
                {
                    b.IsNomoal = true;
                }
                else
                {
                    b.IsNomoal = false;
                    return;
                }
            }
            else
            {
                b.IsNomoal = true;
                b.Unit = "标准";
            }
        }

        /// <summary>
        /// 查询报告历史记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getReportHistory/{year}/{openId}")]
        public ResultPakage<ReportAndHistoryReturnDto> GetReportHistory(string year, string openId)
        {
            var db = new Db();
            var reportAndHistoryReturnDto = new ReportAndHistoryReturnDto();

            var reportHistoryReturnDtos = new List<ReportHistoryReturnDto>();

            var repotList = new List<ReportDto>();

            reportAndHistoryReturnDto.ReportHistory = reportHistoryReturnDtos;
            reportAndHistoryReturnDto.ReportItem = repotList;


            var startDate = year + "-" + "01" + "-" + "01";
            var endDate = year + "-" + "12" + "-" + "31";
            //查询病人历史的数据
            //根据openid 查询病人信息
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();
            //病人当年的所有的报告信息
            var reportData = db.Reports.Where(a => a.ReportDate.CompareTo(startDate) > 0 && a.ReportDate.CompareTo(endDate) < 0 && a.PatientId == patient.Id).Select(a => new { a.CreateTime, a.ReportType, a.ReportDate, a.ImageUrl }).ToList();

            reportData.ForEach(a =>
            {
                var reportDto = new ReportDto();
                reportDto.ReportDate = a.ReportDate;
                reportDto.ReportType = GetNameByReportType(a.ReportType ?? 0);
                reportDto.ImageUrl = a.ImageUrl;
                repotList.Add(reportDto);
            });



            //获取当年的所有指标记录,如果一天中有重复的则取最新的一次结果
            var reportDetailDatas = db.PatientsDatas.Where(a => a.PatientId == patient.Id && a.Report.PatientId == patient.Id)
                .GroupBy(b => new { b.RecordDate, b.DataCode }).Select(c => new
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
                var historyDto = new ReportHistoryReturnDto();
                var Xxdata = new List<string>();
                var Values = new List<string>();
                a.OrderBy(b => b.RecordTime).ForEach(item =>
                  {
                      Xxdata.Add(item.RecordDate);
                      Values.Add(item.DataValue);
                      historyDto.Name = GetNameByCode(item.DataCode ?? 9);
                      historyDto.UnitName = "待定";
                      historyDto.DataCode = "code" + item.DataCode;
                  });
                historyDto.Values = Values;
                historyDto.Xdata = Xxdata;
                reportHistoryReturnDtos.Add(historyDto);
            });

            return Util.ReturnOkResult(reportAndHistoryReturnDto);
        }


        /// <summary>
        /// 查询最近7天的每日记录历史
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getMyRecordHistory/{days}/{openId}")]
        public ResultPakage<GetMyRecordHistoryDto> GetMyRecordHistory(string days, string openId)
        {
            var db = new Db();
            var dto = new GetMyRecordHistoryDto();
            //查询当前日期7天之内的数据
            var startDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

            //查询病人历史的数据
            //根据openid 查询病人信息
            var patient = db.Users.First(a => a.OpenId == openId).Patients.First();

            //病人最近7天的所有的报告信息,每天只取一个记录，且该记录为一天中最新的一个
            var reportDetailDatas = db.PatientsDatas.Where(a => a.PatientId == patient.Id && a.ReportId == null && a.RecordDate.CompareTo(startDate) > 0)
                .Select(c => new
                {
                    c.RecordDate,
                    c.RecordTime,
                    c.DataCode,
                    c.DataValue,
                    c.CreateTime,
                    c.FormType
                }).OrderBy(a => a.RecordDate).ThenBy(a => a.RecordTime).ToList();

            //组装7天的数据。如没有数据则使用null代替，图标判断null会不生成图形
            var SystolicPressure = new List<string>();
            var DiastolicPressure = new List<string>();
            var HeartRate = new List<string>();
            var FastingBloodGlucose = new List<string>();
            var BreakfastBloodGlucose = new List<string>();
            var LunchBloodGlucose = new List<string>();
            var DinnerBloodGlucose = new List<string>();
            var RandomBloodGlucose = new List<string>();
            var Date = new List<string>();

            dto.Date = Date;
            for (var i = 6; i > -1; i--)
            {
                var currentDay = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd");
                var systolicPressure = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.DataCode == (int)PatientsDataType.SystolicPressure).Select(a => a.DataValue).FirstOrDefault();
                SystolicPressure.Add(systolicPressure);

                var diastolicPressure = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.DataCode == (int)PatientsDataType.DiastolicPressure).Select(a => a.DataValue).FirstOrDefault();
                DiastolicPressure.Add(diastolicPressure);

                var heartRate = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.DataCode == (int)PatientsDataType.HeartRate).Select(a => a.DataValue).FirstOrDefault();
                HeartRate.Add(heartRate);

                var fastingBloodGlucose = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.FormType == (int)PatientsDataFormType.FastingBloodGlucose).Select(a => a.DataValue).FirstOrDefault();
                FastingBloodGlucose.Add(fastingBloodGlucose);

                var breakfastBloodGlucose = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.FormType == (int)PatientsDataFormType.BreakfastBloodGlucose).Select(a => a.DataValue).FirstOrDefault();
                BreakfastBloodGlucose.Add(breakfastBloodGlucose);

                var lunchBloodGlucose = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.FormType == (int)PatientsDataFormType.LunchBloodGlucose).Select(a => a.DataValue).FirstOrDefault();
                LunchBloodGlucose.Add(lunchBloodGlucose);

                var dinnerBloodGlucose = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.FormType == (int)PatientsDataFormType.DinnerBloodGlucose).Select(a => a.DataValue).FirstOrDefault();
                DinnerBloodGlucose.Add(dinnerBloodGlucose);

                var randomBloodGlucose = reportDetailDatas.Where(a => a.RecordDate == currentDay && a.FormType == (int)PatientsDataFormType.RandomBloodGlucose).Select(a => a.DataValue).FirstOrDefault();
                RandomBloodGlucose.Add(randomBloodGlucose);

                Date.Add(DateTime.Now.AddDays(-i).ToString("dd") + "日");
            }

            dto.SystolicPressure = SystolicPressure;
            if (dto.SystolicPressure.All(a => a == null))
                dto.SystolicPressure.Add("0");
            dto.DiastolicPressure = DiastolicPressure;
            if (dto.DiastolicPressure.All(a => a == null))
                dto.DiastolicPressure.Add("0");
            dto.HeartRate = HeartRate;
            if (dto.HeartRate.All(a => a == null))
                dto.HeartRate.Add("0");
            dto.FastingBloodGlucose = FastingBloodGlucose;
            if (dto.FastingBloodGlucose.All(a => a == null))
                dto.FastingBloodGlucose.Add("0");
            dto.BreakfastBloodGlucose = BreakfastBloodGlucose;
            if (dto.BreakfastBloodGlucose.All(a => a == null))
                dto.BreakfastBloodGlucose.Add("0");
            dto.LunchBloodGlucose = LunchBloodGlucose;
            if (dto.LunchBloodGlucose.All(a => a == null))
                dto.LunchBloodGlucose.Add("0");
            dto.DinnerBloodGlucose = DinnerBloodGlucose;
            if (dto.DinnerBloodGlucose.All(a => a == null))
                dto.DinnerBloodGlucose.Add("0");
            dto.RandomBloodGlucose = RandomBloodGlucose;
            if (dto.RandomBloodGlucose.All(a => a == null))
                dto.RandomBloodGlucose.Add("0");
            return Util.ReturnOkResult(dto);
        }
    }
}
