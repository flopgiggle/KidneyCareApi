/**************************************************************************************************
 * Author:      ChenJing
 * FileName:    Dto
 * FrameWork:   4.5.2
 * CreateDate:  2015/11/24 15:25:06
 * Description:  User显示实体
 * 
 * ************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using KidneyCareApi.Dal;

namespace KidneyCareApi.Dto
{

    public class DrugsGroup
    {
        public string GroupName { get; set; }
        public bool IsFold { get; set; }
        public List<DrugsGroupTow> GroupTowList { get; set; }
    }

    public class DrugsGroupTow
    {
        public string GroupName { get; set; }
        public string Logogram { get; set; }
        public List<Drugs> DrugsList { get; set; }
    }


    public class Drugs
    {
        public int Id { get; set; }
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public string Remark { get; set; }
        public int PatientId { get; set; }
    }

    public class RecordPatientDrugs
    {
        public DateTime RecordTime { get; set; }
        public List<PatientsDrug> Drugs { get; set; }
        public int PatientId { get; set; }
    }

    public class PatientDrugsHistory
    {
        public string RecordTime { get; set; }
        public string Drugs { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
