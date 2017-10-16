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

    public class CreateCourseDto
    {
        public string RecordTime { get; set; }
        public string Drugs { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
