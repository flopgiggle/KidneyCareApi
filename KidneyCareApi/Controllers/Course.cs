/**
* 命名空间: KidneyCareApi.Controllers
*
* 功 能： 药品模块
* 类 名： UserInfoController
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
* V1.0.0.0 20170801 李闻海 初版
*
*/
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
using System.Web;

namespace KidneyCareApi.Controllers
{

    /// <summary>
    /// 用户管理服务
    /// </summary>
    [RoutePrefix("api/course")]
    public class CourseController : ApiController
    {

        [HttpPost]
        [Route("createCourse")]
        public ResultPakage<bool> CreateCourse(Dal.Course course)
        {
            Db db = new Db();
            course.CreateTime = DateTime.Now;
            db.Courses.Add(course);
            db.SaveChanges();
            return Util.ReturnOkResult(true);
        }

        [HttpPost]
        [Route("uploadImage")]
        public ResultPakage<bool> uploadImage(Dal.Course course)
        {
            Db db = new Db();
            var imageName = Util.SaveImage("UploadCourseImagePath", HttpContext.Current);

            course.CreateTime = DateTime.Now;
            db.Courses.Add(course);
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
