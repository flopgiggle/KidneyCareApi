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
                        dgt.DrugsList.Add(new Drugs(){DrugCode = d.DrugCode,DrugsName = d.DrugName});
                    });
                    group.GroupTowList.Add(dgt);
                });
                DrugsGroup.Add(group);
            });

            

            return Util.ReturnOkResult(JsonConvert.SerializeObject(DrugsGroup));
        }

        [HttpPost]
        [Route("savePatientDrugs")]
        public ResultPakage<bool> SavePatientDrugs(List<PatientsDrug> drugs)
        {
            Db db = new Db();
            //var dbDrugs = drugs.MapToList<PatientsDrug>();
            var patientId = drugs[0].PatientId;
            string batchNum = Guid.NewGuid().ToString();
            var dateTime = DateTime.Now;
            //历史用药设置为非激活状态
            db.PatientsDrugs.Where(a=>a.PatientId == patientId && a.IsActive == true).ForEach(a=>
                {
                    a.IsActive = false;
                    a.UpdateTime = dateTime;
                });
            //写入新的用药信息
            drugs.ForEach(a =>
            {
                a.CreateTime = dateTime;
                a.IsActive = true;
                a.RecordBatch = batchNum;
            });
            db.PatientsDrugs.AddRange(drugs);
            db.SaveChanges();
            return Util.ReturnOkResult(true);
        }


    }
}
