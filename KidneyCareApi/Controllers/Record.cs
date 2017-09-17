using System;
using System.Collections;
using KidneyCareApi.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using KidneyCareApi.BLL;
using KidneyCareApi.Dal;
using KidneyCareApi.Dto;
using Newtonsoft.Json;
using WebGrease;
using WebGrease.Css.Extensions;


namespace KidneyCareApi.Controllers
{
    /// <summary>
    /// 用户管理服务
    /// </summary>
    [RoutePrefix("api/record")]
    public class RecordController : ApiController
    {
        public PatientsData GetPatientsData(PatientsDataType dataType,string value,string recordTime,string recordData,Patient patient,DateTime createDateTime, PatientsDataFormType formType = PatientsDataFormType.None,Dal.Report report = null)
        {
            PatientsData data = new PatientsData();
            data.DataCode = (int)dataType;
            data.DataValue = value;
            data.RecordTime = recordTime;
            data.RecordDate = recordData;
            data.Patient = patient;
            data.FormType = (int)formType;
            data.CreateTime = createDateTime;
            if (report != null)
            {
                data.Report = report;
            }
            return data;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("add")]
        public ResultPakage<bool> Add(MyRecordDto dto)
        {
            Db db = new Db();
            var datetime = DateTime.Now;

            //根据openid 查询病人信息
            var patient = db.Patients.First(a => a.User.OpenId == dto.OpenId);
            List<PatientsData> patientsData = new List<PatientsData>();
            if (dto.SystolicPressure != "")
            {
                //收缩压
                var systolicPressureData = GetPatientsData(PatientsDataType.SystolicPressure, dto.SystolicPressure, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(systolicPressureData);
                patientsData.Add(systolicPressureData);
            }


            if (dto.DiastolicPressure != "")
            {
                //舒张压
                var diastolicPressureData = GetPatientsData(PatientsDataType.DiastolicPressure, dto.DiastolicPressure, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(diastolicPressureData);
                patientsData.Add(diastolicPressureData);
            }


            if (dto.HeartRate != "")
            {
                //心率
                var heartRateData = GetPatientsData(PatientsDataType.HeartRate, dto.HeartRate, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(heartRateData);
                patientsData.Add(heartRateData);
            }

            if (dto.UrineVolume != "")
            {
                //尿量
                var UrineVolume = GetPatientsData(PatientsDataType.UrineVolume, dto.UrineVolume, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(UrineVolume);
                patientsData.Add(UrineVolume);
            }

            if (dto.BodyWeight != "")
            {
                //体重
                var BodyWeight = GetPatientsData(PatientsDataType.Weight, dto.BodyWeight, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(BodyWeight);
                patientsData.Add(BodyWeight);
            }




            if (dto.FastingBloodGlucose != "")
            {
                //空腹血糖
                var FBG = GetPatientsData(PatientsDataType.FBG, dto.FastingBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(FBG);
                patientsData.Add(FBG);
            }


            if (dto.BreakfastBloodGlucose != "")
            {
                //早餐后血糖
                var breakfastBloodGlucoseData = GetPatientsData(PatientsDataType.PBG, dto.BreakfastBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.BreakfastBloodGlucose);
                db.PatientsDatas.Add(breakfastBloodGlucoseData);
                patientsData.Add(breakfastBloodGlucoseData);
            }


            if (dto.LunchBloodGlucose != "")
            {
                //午餐后血糖
                var lunchBloodGlucoseData = GetPatientsData(PatientsDataType.PBG, dto.LunchBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.LunchBloodGlucose);
                db.PatientsDatas.Add(lunchBloodGlucoseData);
                patientsData.Add(lunchBloodGlucoseData);
            }


            if (dto.DinnerBloodGlucose != "")
            {
                //晚餐后血糖
                var dinnerBloodGlucoseData = GetPatientsData(PatientsDataType.PBG, dto.DinnerBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.DinnerBloodGlucose);
                db.PatientsDatas.Add(dinnerBloodGlucoseData);
                patientsData.Add(dinnerBloodGlucoseData);
            }

            if (dto.RandomBloodGlucose != "")
            {
                //随机血糖
                var RBG = GetPatientsData(PatientsDataType.RBG, dto.RandomBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(RBG);
                patientsData.Add(RBG);
            }

            //判定是否有异常存在，如果有异常则写入最近异常信息
            if (IsExceptionData(patientsData, patient))
            {
                patient.LastExceptionDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            db.SaveChanges();

            return Util.ReturnOkResult(true);
        }

        /// <summary>
        /// 男	 肌酐≦79.56  56-122	ml/min/1.73m2	141*power((to_number(肌酐/79.56）,(-0.411))*power((0.993),年龄）       其中power是指开方，to_number是指取整数
        //  男   肌酐>79.56			141*power((to_number(肌酐/79.56）, (-1.209))*power((0.993),年龄）
        //  女   肌酐≦61.88			144*power((to_number(肌酐/61.88）, (-0.329))*power((0.993),年龄）
        //  女   肌酐>61.88			144*power((to_number(肌酐/61.88）, (-1.209))*power((0.993),年龄）
        /// </summary>
        /// <param name="SCr"></param>
        /// <param name="sex"></param>
        /// <returns></returns>
        public string GetEGFR(double SCr,int sex,int age)
        {
            //141*power((to_number(肌酐/79.56）,(-0.411))*power((0.993),年龄）       其中power是指开方，to_number是指取整数
            if (sex == 0 && SCr <=79.56)
            {
                return Math.Round((141*Math.Pow((int) (SCr / 79.56), -0.411) * Math.Pow((0.993), age)),2).ToString(CultureInfo.InvariantCulture);
            }

            //141*power((to_number(肌酐/79.56）,(-1.209))*power((0.993),年龄）
            if (sex == 0 && SCr > 79.56)
            {
                return Math.Round((141 * Math.Pow((int)(SCr / 79.56), -1.209) * Math.Pow((0.993), age)),2).ToString(CultureInfo.InvariantCulture);
            }

            //144*power((to_number(肌酐/61.88）,(-0.329))*power((0.993),年龄）
            if (sex == 1 && SCr <= 61.88)
            {
                return Math.Round((141 * Math.Pow((int)(SCr / 61.88), -0.329) * Math.Pow((0.993), age)), 2).ToString(CultureInfo.InvariantCulture);
            }

            //144*power((to_number(肌酐/61.88）,(-1.209))*power((0.993),年龄）
            if (sex == 1 && SCr > 61.88)
            {
                return Math.Round((141 * Math.Pow((int)(SCr / 61.88), -1.209) * Math.Pow((0.993), age)), 2).ToString(CultureInfo.InvariantCulture);
            }
            return "";
        }

        /// <summary>
        /// 新增报告
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        //[OpenApi]
        [Route("addReport")]
        public ResultPakage<string> AddReport(ReportDto dto)
        {
            Db db = new Db();
            var datetime = DateTime.Now;

            //根据openid 查询病人信息
            var patient = db.Patients.First(a => a.User.OpenId == dto.OpenId);
            
            Dal.Report report = new Report();
            report.ReportDate = dto.ReportDate;
            report.ReportType = int.Parse(dto.ReportType);
            report.ReportMark = dto.ReportMark;
            report.ImageUrl = "";
            report.CreateTime = datetime;
            report.Patient = patient;
            db.Reports.Add(report);

            var datas = dto.MedicalIndicators;
            var time = "00:00";
            var brithday = patient.User.Birthday;
            int y1 = DateTime.Parse(brithday).Year;
            int y2 = DateTime.Now.Year;
            int age = y2 - y1;
            var sex = patient.User.Sex;
            List<PatientsData> patientsData = new List<PatientsData>();
            //pro
            if (!string.IsNullOrEmpty(datas.Pro))
            {
                var pro = GetPatientsData(PatientsDataType.Pro, datas.Pro, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(pro);
                patientsData.Add(pro);
            }

            if (!string.IsNullOrEmpty(datas.ERY))
            {
                var ERY = GetPatientsData(PatientsDataType.ERY, datas.ERY, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(ERY);
                patientsData.Add(ERY);
            }

            if (!string.IsNullOrEmpty(datas.LEU))
            {
                var LEU = GetPatientsData(PatientsDataType.LEU, datas.LEU, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(LEU);
                patientsData.Add(LEU);
            }

            if (!string.IsNullOrEmpty(datas.Upr))
            {
                //Upr
                var Upr = GetPatientsData(PatientsDataType.Upr, datas.Upr, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(Upr);
                patientsData.Add(Upr);
            }
            if (!string.IsNullOrEmpty(datas.ProICr))
            {
                var proICr = GetPatientsData(PatientsDataType.ProICr, datas.ProICr, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(proICr);
                patientsData.Add(proICr);
            }
            if (!string.IsNullOrEmpty(datas.UAICr))
            {
                var UAICr = GetPatientsData(PatientsDataType.UAICr, datas.UAICr, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(UAICr);
                patientsData.Add(UAICr);
            }
            if (!string.IsNullOrEmpty(datas.BUN))
            {
                //Alb
                var bUnData = GetPatientsData(PatientsDataType.BUN, datas.BUN, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(bUnData);
                patientsData.Add(bUnData);
            }
            //UA
            if (!string.IsNullOrEmpty(datas.UA))
            {
                var UA = GetPatientsData(PatientsDataType.UA, datas.UA, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(UA);
                patientsData.Add(UA);
            }
            //SCr
            if (!string.IsNullOrEmpty(datas.SCr))
            {
                var SCr = GetPatientsData(PatientsDataType.SCr, datas.SCr, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(SCr);
                patientsData.Add(SCr);

                //根据肌酐 eGfr自动计算
                var eGfr = GetPatientsData(PatientsDataType.eGFR,GetEGFR(double.Parse(datas.SCr),int.Parse(sex) ,age), time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(eGfr);
                patientsData.Add(eGfr);
            }
            //Alb
            if (!string.IsNullOrEmpty(datas.Alb))
            {
                var albData = GetPatientsData(PatientsDataType.Alb, datas.Alb, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(albData);
                patientsData.Add(albData);
            }
            //TG
            if (!string.IsNullOrEmpty(datas.TG))
            {
                var tg = GetPatientsData(PatientsDataType.TG, datas.TG, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(tg);
                patientsData.Add(tg);
            }
            if (!string.IsNullOrEmpty(datas.Chol))
            {
                //Chol
                var chol = GetPatientsData(PatientsDataType.Chol, datas.Chol, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(chol);
                patientsData.Add(chol);
            }
            if (!string.IsNullOrEmpty(datas.Na))
            {
                //Alb
                var na = GetPatientsData(PatientsDataType.Na, datas.Na, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(na);
                patientsData.Add(na);
            }
            if (!string.IsNullOrEmpty(datas.K))
            {
                //Alb
                var k = GetPatientsData(PatientsDataType.K, datas.K, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(k);
                patientsData.Add(k);
            }
            if (!string.IsNullOrEmpty(datas.P))
            {
                //Alb
                var p = GetPatientsData(PatientsDataType.P, datas.P, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(p);
                patientsData.Add(p);
            }
            if (!string.IsNullOrEmpty(datas.Ca))
            {
                //Ca
                var caData = GetPatientsData(PatientsDataType.Ca, datas.Ca, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(caData);
                patientsData.Add(caData);
            }
            if (!string.IsNullOrEmpty(datas.Hb))
            {
                //Alb
                var hb = GetPatientsData(PatientsDataType.Hb, datas.Hb, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(hb);
                patientsData.Add(hb);
            }
            if (!string.IsNullOrEmpty(datas.WBC))
            {
                var WBC = GetPatientsData(PatientsDataType.WBC, datas.WBC, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(WBC);
                patientsData.Add(WBC);
            }
            if (!string.IsNullOrEmpty(datas.PLT))
            {
                var PLT = GetPatientsData(PatientsDataType.PLT, datas.PLT, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(PLT);
                patientsData.Add(PLT);
            }
            if (!string.IsNullOrEmpty(datas.PTH))
            {
                //Alb
                var pth = GetPatientsData(PatientsDataType.PTH, datas.PTH, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(pth);
                patientsData.Add(pth);
            }

            if (!string.IsNullOrEmpty(datas.Weight))
            {
                var weight = GetPatientsData(PatientsDataType.Weight, datas.Weight, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(weight);
                patientsData.Add(weight);
            }

            //判定是否有异常存在，如果有异常则写入最近异常信息
            if (IsExceptionData(patientsData, patient))
            {
                patient.LastExceptionDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            db.SaveChanges();

            //保存成功后，回传ID，用于上传图片用
            return Util.ReturnOkResult(report.Id.ToString());
        }

        //检查是否有异常指标
        public bool IsExceptionData(List<PatientsData> datas,Dal.Patient patient)
        {
            PatientInfo patientInfo = new PatientInfo();
            var indicator = patientInfo.GetIndicatorInfo(patient.Hospital.Id.ToString(), patient.Id);
            var isException = false;
            datas.ForEach(a =>
            {
                var currentInfoListDto = Mapper.Map<CurrentInfoListDto>(a);
                //var currentInfoListDto =
                //   new CurrentInfoListDto() {DataCode = a.DataCode, Unit = a.Unit, DataValue = a.DataValue};
                patientInfo.IndicatorJudge(indicator, currentInfoListDto);
                if (!currentInfoListDto.IsNomoal)
                {
                    isException = true;
                }
            });
            return isException;
        }

    }
}
