using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using KidneyCareApi.Common;
using KidneyCareApi.Dal;
using KidneyCareApi.Dto;
using WebGrease.Css.Extensions;

namespace KidneyCareApi.BLL
{
    public class PatientInfo
    {
        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="reportType"></param>
        /// <returns></returns>
        public static string GetNameByReportType(int reportType)
        {
            return ((ReportType)reportType).GetEnumDes();
        }


        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetNameByCode(int code)
        {
            return ((PatientsDataType)code).GetEnumDes();
        }

        /// <summary>
        /// 获取医学代码的名称
        /// </summary>
        /// <param name="formType"></param>
        /// <returns></returns>
        public static string GetNameByFormType(int formType)
        {
            return ((PatientsDataFormType)formType).GetEnumDes();
        }


        public static Expression<Func<T, bool>> True<T>()
        {
            return p => true;
        }

        public ResultPakage<CurrentInfoReturnDto> GetCurrentDayInfoList(string startDate, string endDate, string openId)
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
            //病人当天的所有的报告信息
            var reportList = new List<CurrentInfoListDto>();

            //Expression<Func<SysUser, bool>>

            //病人当天的所有自我记录信息
            var patientData = db.PatientsDatas
                .Where(a => a.RecordDate.CompareTo(startDate) >= 0 && a.RecordDate.CompareTo(endDate) <= 0 &&
                            a.ReportId == null && a.PatientId == patient.Id)
                .Select(a => new {a.DataCode, a.DataValue, a.CreateTime, a.RecordTime, a.FormType, a.RecordDate})
                .OrderByDescending(a => new { a.RecordTime,a.RecordDate,a.CreateTime }).ToList();

            var reportData = db.Reports
                .Where(a => a.ReportDate.CompareTo(startDate) >= 0 && a.ReportDate.CompareTo(endDate) <= 0 &&
                            a.PatientId == patient.Id).Select(a => new {a.Id, a.CreateTime, a.ReportType, a.ReportDate})
                .OrderBy(a => a.Id).OrderByDescending(a => new {a.ReportDate,a.CreateTime}).ToList();

            var reportDetailDatas = db.Reports
                .Where(a => a.ReportDate.CompareTo(startDate) >= 0 && a.ReportDate.CompareTo(endDate) <= 0 &&
                            a.PatientId == patient.Id)
                .SelectMany(a => a.PatientsDatas.Select(b => new {b.DataCode, b.DataValue, b.CreateTime, b.RecordTime}))
                .ToList();

            reportData.ForEach(a =>
            {
                var recordDto = new CurrentInfoListDto();
                recordDto.ReportName = GetNameByReportType(a.ReportType ?? 1);
                recordDto.CreateTime = a.CreateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                recordDto.RecordDate = a.ReportDate;
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
                        oneReturnDto.RecordDate = item.RecordDate;
                        //如果有表单类型0，则说明为自定义表单字段，需要恢复设置的表单值
                        if (item.FormType != 0)
                            oneReturnDto.DataName = GetNameByFormType(item.FormType ?? 0);
                        else
                            oneReturnDto.DataName = GetNameByCode(item.DataCode ?? 9);
                        //判定指标是否正确
                        IndicatorJudge(indicators, oneReturnDto);

                    }

                    //如果类型为收缩压,则找出对应的舒张压，并把舒张压放入收缩压返回值,并设置名称为血压
                    if (item.DataCode == (int)PatientsDataType.SystolicPressure)
                    {
                        //查找对应的舒张压
                        var diastolicPressure = a.FirstOrDefault(x => x.DataCode == (int)PatientsDataType.DiastolicPressure);
                        if (diastolicPressure != null)
                        {
                            oneReturnDto.DataValue = item.DataValue + "/" + diastolicPressure.DataValue;
                        }
                        //对舒张压值进行判定比较
                        var diastolicPressureDto = new CurrentInfoListDto();
                        diastolicPressureDto.DataCode = diastolicPressure.DataCode;
                        diastolicPressureDto.DataValue = diastolicPressure.DataValue;
                        IndicatorJudge(indicators, diastolicPressureDto);

                        oneReturnDto.DataName = "血压";
                        oneReturnDto.IsNomoal = diastolicPressureDto.IsNomoal && oneReturnDto.IsNomoal;

                    }

                    if (item.DataCode != (int)PatientsDataType.DiastolicPressure)
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
                if (indicator.Unit != null)
                {
                    b.Unit = indicator.Unit;
                }
                else
                {
                    b.Unit = "";
                }

                //数据为空直接返回异常
                if (b.DataValue == null)
                {
                    b.IsNomoal = false;
                    return;
                }

                //判断值是否相等
                if (!string.IsNullOrEmpty(indicator.Equal) && b.DataValue == indicator.Equal)
                {
                    b.IsNomoal = true;
                    return;
                }
                if (!string.IsNullOrEmpty(indicator.Equal) && b.DataValue != indicator.Equal)
                {
                    b.IsNomoal = false;
                    return;
                }

                //进行数值比较
                double inputvalue;
                if (double.TryParse(b.DataValue, out inputvalue))
                {
                    //如果指标小于最大值
                    if (string.IsNullOrEmpty(indicator.Max) || double.Parse(b.DataValue) < double.Parse(indicator.Max))
                    {
                        b.IsNomoal = true;
                    }
                    else
                    {
                        b.IsNomoal = false;
                        return;
                    }

                    //如果指标小于最大值
                    if (string.IsNullOrEmpty(indicator.Min) || double.Parse(b.DataValue) > double.Parse(indicator.Min))
                    {
                        b.IsNomoal = true;
                    }
                    else
                    {
                        b.IsNomoal = false;
                        return;
                    }
                }

            }
            else
            {
                b.IsNomoal = true;
                b.Unit = "标准";
            }
        }
    }


}