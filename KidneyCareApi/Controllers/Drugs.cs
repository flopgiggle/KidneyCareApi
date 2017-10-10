using System;
using KidneyCareApi.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Web.Http;
using Antlr.Runtime.Tree;
using AutoMapper;
using KidneyCareApi.BLL;
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
    [RoutePrefix("api/drugs")]
    public class DrugsController : ApiController
    {

        [HttpGet]
        [Route("getAllDrugs")]
        public ResultPakage<string> GetAllDrugs()
        {
            Db db = new Db();
            List<DrugsGroup> DrugsGroup = new List<DrugsGroup>();
            var allDrugs = db.Drugs.ToList();
            //遍历一级分类
            allDrugs.Select(a => a.DrugGroup).GroupBy(a => a).ToList().ForEach(a =>
            {
                
                DrugsGroup group = new DrugsGroup();
                group.GroupTowList = new List<DrugsGroupTow>();
                group.GroupName = a.Key;
                //遍历二级分类
                allDrugs.Where(b => b.DrugGroup == a.Key).GroupBy(g=>new {g.DrugGroup,g.DrugGroupTwo,g.DrugGroupTwoLogogram}).ForEach(c =>
                {
                    DrugsGroupTow dgt = new DrugsGroupTow();
                    dgt.DrugsList = new List<Drugs>();
                    dgt.GroupName = c.Key.DrugGroupTwo;
                    dgt.Logogram = c.Key.DrugGroupTwoLogogram;
                    //遍历二级分类下的每一种药物，并设置Dto到二级分类中
                    allDrugs.Where(b =>b.DrugGroup==a.Key && b.DrugGroupTwo == c.Key.DrugGroupTwo).ForEach(d =>
                    {
                        dgt.DrugsList.Add(new Drugs(){DrugCode = d.DrugCode,DrugName = d.DrugName});
                    });
                    group.GroupTowList.Add(dgt);
                });
                DrugsGroup.Add(group);
            });

            

            return Util.ReturnOkResult(JsonConvert.SerializeObject(DrugsGroup));
        }

        [HttpPost]
        [Route("savePatientDrugs")]
        public ResultPakage<bool> SavePatientDrugs(RecordPatientDrugs recordPatientDrugs)
        {
            Db db = new Db();
            //var dbDrugs = drugs.MapToList<PatientsDrug>();
            var drugs = recordPatientDrugs.Drugs;
            var patientId = recordPatientDrugs.PatientId;
            var currentRecordTime = recordPatientDrugs.RecordTime;
            string batchNum = Guid.NewGuid().ToString();
            var dateTime = DateTime.Now;
            //判定是否有任何记录
            bool noAnyRecord = !db.PatientsDrugs.Any(a => a.PatientId == patientId);
            //判定当前日期是否是最新的用药记录，如果当前日期为最新用药记录，则把之前的用药全部设置为非激活状态
            bool currentIsNew = db.PatientsDrugs.Any(a => a.PatientId == patientId && a.IsActive == true && a.RecordTime <= currentRecordTime) || noAnyRecord;


            //如果没有新的记录写入，则直接设置以前的所有记录为非激活状态，并且返回
            if (recordPatientDrugs.Drugs==null || recordPatientDrugs.Drugs.Count == 0)
            {
                //如果没有写入记录，而且为历史用药，则不必写入数据库，表明什么都没操作
                if (currentIsNew)
                {
                    //历史用药设置为非激活状态
                    db.PatientsDrugs.Where(a => a.PatientId == patientId && a.IsActive == true).ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdateTime = dateTime;
                    });
                    db.SaveChanges();
                    return Util.ReturnOkResult(true);
                }
                else
                {
                    return Util.ReturnOkResult(true);
                }
            }

            if (currentIsNew)
            {
                //历史用药设置为非激活状态
                db.PatientsDrugs.Where(a => a.PatientId == patientId && a.IsActive == true).ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdateTime = dateTime;
                });
            }

            //写入新的用药信息
            drugs.ForEach(a =>
            {
                a.CreateTime = dateTime;
                a.IsActive = currentIsNew;
                a.RecordBatch = batchNum;
            });
            db.PatientsDrugs.AddRange(drugs);
            db.SaveChanges();
            return Util.ReturnOkResult(true);
        }

        [HttpGet]
        [Route("getHistoryDrugs/{patientId}")]
        public ResultPakage<List<PatientDrugsHistory>> GetHistoryDrugs(int patientId)
        {
            Db db = new Db();
            var drugsGroup =  db.PatientsDrugs.Where(a => a.PatientId == patientId).OrderByDescending(a => a.RecordTime).Select(a=>new {a.PatientId,a.RecordBatch,a.RecordTime,a.DrugCode,a.DrugName,a.Remark,a.CreateTime})
                .GroupBy(a => new {a.RecordBatch,a.RecordTime}).OrderByDescending(a=>a.Key.RecordTime);
            List<PatientDrugsHistory> returnList = new List<PatientDrugsHistory>();
            
            drugsGroup.ForEach(a =>
            {
                PatientDrugsHistory hisoty = new PatientDrugsHistory();
                hisoty.RecordTime = (a.Key.RecordTime??DateTime.Now).ToString("yyyy-MM-dd");
                a.ForEach(b =>
                {
                    hisoty.Drugs += b.DrugName + " " + b.Remark + ";";
                    hisoty.CreateTime = b.CreateTime;
                });
                
                returnList.Add(hisoty);
            });
            var list = returnList.OrderByDescending(a => a.RecordTime).ThenByDescending(a=>a.CreateTime).ToList();
            return Util.ReturnOkResult(list);
        }


    }
}
