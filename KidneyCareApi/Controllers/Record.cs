using System;
using System.Collections;
using KidneyCareApi.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using KidneyCareApi.Dal;
using KidneyCareApi.Dto;
using WebGrease;


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

            if (dto.SystolicPressure != "")
            {
                //收缩压
                var systolicPressureData = GetPatientsData(PatientsDataType.SystolicPressure, dto.SystolicPressure, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(systolicPressureData);
            }


            if (dto.DiastolicPressure != "")
            {
                //舒张压
                var diastolicPressureData = GetPatientsData(PatientsDataType.DiastolicPressure, dto.DiastolicPressure, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(diastolicPressureData);
            }


            if (dto.HeartRate != "")
            {
                //心率
                var heartRateData = GetPatientsData(PatientsDataType.HeartRate, dto.HeartRate, dto.RecordTime,
                    dto.RecordDate, patient, datetime);
                db.PatientsDatas.Add(heartRateData);
            }


            if (dto.FastingBloodGlucose != "")
            {
                //空腹血糖
                var fastingBloodGlucoseData = GetPatientsData(PatientsDataType.BloodGlucose, dto.FastingBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.FastingBloodGlucose);
                db.PatientsDatas.Add(fastingBloodGlucoseData);
            }


            if (dto.BreakfastBloodGlucose != "")
            {
                //早餐后血糖
                var breakfastBloodGlucoseData = GetPatientsData(PatientsDataType.BloodGlucose, dto.BreakfastBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.BreakfastBloodGlucose);
                db.PatientsDatas.Add(breakfastBloodGlucoseData);
            }


            if (dto.LunchBloodGlucose != "")
            {
                //午餐后血糖
                var lunchBloodGlucoseData = GetPatientsData(PatientsDataType.BloodGlucose, dto.LunchBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.LunchBloodGlucose);
                db.PatientsDatas.Add(lunchBloodGlucoseData);
            }


            if (dto.DinnerBloodGlucose != "")
            {
                //晚餐后血糖
                var dinnerBloodGlucoseData = GetPatientsData(PatientsDataType.BloodGlucose, dto.DinnerBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.DinnerBloodGlucose);
                db.PatientsDatas.Add(dinnerBloodGlucoseData);
            }


            if (dto.RandomBloodGlucose != "")
            {
                //随机血糖
                var randomBloodGlucoseData = GetPatientsData(PatientsDataType.BloodGlucose, dto.RandomBloodGlucose, dto.RecordTime,
                    dto.RecordDate, patient, datetime, PatientsDataFormType.RandomBloodGlucose);
                db.PatientsDatas.Add(randomBloodGlucoseData);
            }

            db.SaveChanges();

            //var doctor = db.Doctors.First(a => a.Id == dto.BelongToDoctor);
            //var nurse = db.Nurses.First(a => a.Id == dto.BelongToNurse);
            //var hospital = db.Hospitals.First(a => a.Id == dto.BelongToHospital);

            ////创建用户
            //Dal.User user = new User();
            //user.Birthday = dto.Birthday;
            //user.CreateTime = DateTime.Now;
            //user.OpenId = dto.OpenId;
            //user.MobilePhone = dto.MobilePhone;
            //user.Sex = dto.Sex;
            //user.Status = 1;
            //user.OpenId = dto.OpenId;

            //Dal.Patient patient = new Patient();
            //patient.User = user;
            //patient.Hospital = hospital;
            //patient.Doctor = doctor;
            //patient.CreateTime = DateTime.Now;

            //db.Users.Add(user);
            //db.Patients.Add(patient);
            //db.SaveChanges();

            //db.Users.Add(new User(){Doctors = });
            return Util.ReturnOkResult(true);
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

            //Alb
            if (!string.IsNullOrEmpty(datas.Alb))
            {
                var albData = GetPatientsData(PatientsDataType.Alb, datas.Alb, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(albData);
            }

            if (!string.IsNullOrEmpty(datas.BUN))
            {
                //Alb
                var bUnData = GetPatientsData(PatientsDataType.BUN, datas.BUN, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(bUnData);
            }



            if (!string.IsNullOrEmpty(datas.Ca))
            {
                //Ca
                var caData = GetPatientsData(PatientsDataType.Ca, datas.Ca, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(caData);
            }


            if (!string.IsNullOrEmpty(datas.Chol))
            {
                //Chol
                var chol = GetPatientsData(PatientsDataType.Chol, datas.Chol, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(chol);
            }


            if (!string.IsNullOrEmpty(datas.Hb))
            {
                //Alb
                var hb = GetPatientsData(PatientsDataType.Hb, datas.Hb, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(hb);
            }


            if (!string.IsNullOrEmpty(datas.K))
            {
                //Alb
                var k = GetPatientsData(PatientsDataType.K, datas.K, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(k);
            }


            if (!string.IsNullOrEmpty(datas.Na))
            {
                //Alb
                var na = GetPatientsData(PatientsDataType.Na, datas.Na, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(na);
            }


            if (!string.IsNullOrEmpty(datas.P))
            {
                //Alb
                var p = GetPatientsData(PatientsDataType.P, datas.P, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(p);
            }


            if (!string.IsNullOrEmpty(datas.PTH))
            {
                //Alb
                var pth = GetPatientsData(PatientsDataType.PTH, datas.PTH, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(pth);
            }


            if (!string.IsNullOrEmpty(datas.Pro))
            {
                var pro = GetPatientsData(PatientsDataType.Pro, datas.Pro, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(pro);
            }


            if (!string.IsNullOrEmpty(datas.ProICr))
            {
                var proICr = GetPatientsData(PatientsDataType.Pro, datas.ProICr, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(proICr);
            }


            if (!string.IsNullOrEmpty(datas.eGFR))
            {
                var eGfr = GetPatientsData(PatientsDataType.eGFR, datas.eGFR, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(eGfr);
            }


            if (!string.IsNullOrEmpty(datas.TG))
            {
                var tg = GetPatientsData(PatientsDataType.TG, datas.TG, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(tg);
            }


            if (!string.IsNullOrEmpty(datas.Weight))
            {
                var weight = GetPatientsData(PatientsDataType.Weight, datas.Weight, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(weight);
            }


            if (!string.IsNullOrEmpty(datas.UA))
            {
                var ua = GetPatientsData(PatientsDataType.UA, datas.UA, time, dto.ReportDate, patient, datetime, PatientsDataFormType.None, report);
                db.PatientsDatas.Add(ua);
            }

            db.SaveChanges();

            //var doctor = db.Doctors.First(a => a.Id == dto.BelongToDoctor);
            //var nurse = db.Nurses.First(a => a.Id == dto.BelongToNurse);
            //var hospital = db.Hospitals.First(a => a.Id == dto.BelongToHospital);

            ////创建用户
            //Dal.User user = new User();
            //user.Birthday = dto.Birthday;
            //user.CreateTime = DateTime.Now;
            //user.OpenId = dto.OpenId;
            //user.MobilePhone = dto.MobilePhone;
            //user.Sex = dto.Sex;
            //user.Status = 1;
            //user.OpenId = dto.OpenId;

            //Dal.Patient patient = new Patient();
            //patient.User = user;
            //patient.Hospital = hospital;
            //patient.Doctor = doctor;
            //patient.CreateTime = DateTime.Now;

            //db.Users.Add(user);
            //db.Patients.Add(patient);
            //db.SaveChanges();

            //db.Users.Add(new User(){Doctors = });
            //保存成功后，回传ID，用于上传图片用
            return Util.ReturnOkResult(report.Id.ToString());
        }

    }
}
