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
    /// <summary>
    /// User显示实体
    /// </summary>
    public class Dto
    {
        private string userTypeShow;
        /// <summary>
        /// 用户类型显示
        /// </summary>
        public string UserTypeShow
        {
            get { return userTypeShow; }
            set
            {

                userTypeShow = value;
            }
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ShowName { get; set; }
        public int? UserType { get; set; }
        public DateTime? CreateTime { get; set; }
        public string MobiePhone { get; set; }

        public string Token { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class SendMssageDto
    {
        public string Message;
        public int FromUser;
        public int ToUser;
    }

    public class AddPatientCourseEvaluateParamsDto
    {
        public int PatientId;
        public string CoursCode;
        public string CoursName;
        public string CoursStatus;
        public string AttendingDates;
        public int ObjectCode;
        public string ObjectName;
        public int ModeCode;
        public string ModeName;
        public int CognitionCode;
        public string CognitionName;
        public int BehaviorCode;
        public string BehaviorName;
        public string Mark;
    }

    public class GetMssageDto
    {
        public string Index;
        public string PageSize;
        public int UserId;
        public int PatientId;
    }

    /// <summary>
    /// User显示实体
    /// </summary>
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// User显示实体
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 普通患者
        /// </summary>
        [Description("普通患者")]
        Patient = 1,

        /// <summary>
        /// 医生
        /// </summary>
        [Description("医生")]
        Doctor = 2,

        /// <summary>
        /// 护士
        /// </summary>
        [Description("护士")]
        Nures = 3,
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    public class UserRegistDto
    {
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public int? UserType { get; set; }
        //public string MobiePhone { get; set; }
        //public bool? IsDelete { get; set; }
        //public string AuthenCode { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string MobilePhone { get; set; }
        public string Birthday { get; set; }
        public int BelongToHospital { get; set; }
        public string Sex { get; set; }
        public int UserType { get; set; }
        public int BelongToNurse { get; set; }
        public int BelongToDoctor { get; set; }
        public string IdCard { get; set; }
        public string OpenId { get; set; }
        public string CKDLeave { get; set; }
        public string DiseaseType { get; set; }
        public int JobTitle { get; set; }
        public string Profile { get; set; }
        public string WxAvatarUrl { get; set; }
        public double Height { get; set; }

        public List<Dal.PatientsDisease> Disease { get; set; }
    }

    public class GetMessageReturnDto
    {
        public int Id { get; set; }

        public int? ToUser { get; set; }

        public int? FromUser { get; set; }

        public string Messge { get; set; }

        public bool? IsRead { get; set; }

        public string CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public DateTime? CreatePerson { get; set; }

        public DateTime? UpdatePerson { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public int UserType { get; set; }
    }

    /// <summary>
    /// 获取病人列表查询条件
    /// </summary>
    public class GetPatientListParamsDto
    {
        //患者id
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string Age { get; set; }

    }

    public class MyRecordDto
    {
        /// <summary>
        /// 记录日期
        /// </summary>
        public string RecordDate { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public string RecordTime { get; set; }

        /// <summary>
        /// 收缩压
        /// </summary>
        public string SystolicPressure { get; set; }

        /// <summary>
        /// 舒张压
        /// </summary>
        public string DiastolicPressure { get; set; }

        public string HeartRate { get; set; }

        /// <summary>
        /// 空腹血糖
        /// </summary>
        public string FastingBloodGlucose { get; set; }

        /// <summary>
        /// 早餐后血糖
        /// </summary>
        public string BreakfastBloodGlucose { get; set; }

        /// <summary>
        /// 午餐后血糖
        /// </summary>
        public string LunchBloodGlucose { get; set; }

        /// <summary>
        /// 晚餐后血糖
        /// </summary>
        public string DinnerBloodGlucose { get; set; }

        /// <summary>
        /// 随机血糖
        /// </summary>
        public string RandomBloodGlucose { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public string BodyWeight { get; set; }

        /// <summary>
        /// 尿量
        /// </summary>
        public string UrineVolume { get; set; }


        public string OpenId { get; set; }
    }

    public class ReportDto
    {
        /// <summary>
        /// 记录日期
        /// </summary>
        public string ReportDate { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string ReportMark { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public string ReportType { get; set; }

        /// <summary>
        /// 报告图片地址
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 报告图片地址
        /// </summary>
        public List<string> ImageUrls { get; set; }


        /// <summary>
        /// 体重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 尿蛋白定性
        /// </summary>
        public string Proteinuria { get; set; }


        public string OpenId { get; set; }

        public MedicalIndicators MedicalIndicators { get; set; }
    }

    /// <summary>
    /// 用户状态类型，标示用户是否已注册 已填写用户信息 或者已删除
    /// </summary>
    public enum UserStatusType
    {
        /// <summary>
        /// 已注册
        /// </summary>
        [Description("")]
        Registered = 1,

        /// <summary>
        ///     舒张压
        /// </summary>
        [Description("完成绑定医院医护信息")]
        CompleteBinding = 2,
    }

    public enum JobTitleType
    {
        /// <summary>
        /// 住院医师
        /// </summary>
        [Description("住院医师")]
        ResidentDoctor = 1,

        /// <summary>
        /// 主治医师
        /// </summary>
        [Description("主治医师")]
        AttendingPhysician = 2,

        /// <summary>
        /// 副主任医师
        /// </summary>
        [Description("副主任医师")]
        AssociateChiefPhysician = 3,

        /// <summary>
        ///     主任医师
        /// </summary>
        [Description("主任医师")]
        ChiefPhysician = 4,

        /// <summary>
        ///     初级护士
        /// </summary>
        [Description("初级护士")]
        ThePrimaryNurse = 10,

        /// <summary>
        ///     初级护师
        /// </summary>
        [Description("初级护师")]
        ThePrimaryNursePractitioner = 11,

        /// <summary>
        ///     主管护师
        /// </summary>
        [Description("主管护师")]
        IurseInCharge = 12,

        /// <summary>
        ///     副主任护师
        /// </summary>
        [Description("副主任护师")]
        CoChiefSuperintendentNurse = 13,

        /// <summary>
        ///     主任护师
        /// </summary>
        [Description("主任护师")]
        ChiefSuperintendentNurse = 14,
    }

    public enum PatientsDataType
    {
        /// <summary>
        /// 收缩压SBP
        /// </summary>
        [Description("收缩压")]
        SystolicPressure = 11000,

        /// <summary>
        ///     舒张压DBP
        /// </summary>
        [Description("舒张压")]
        DiastolicPressure = 11001,

        /// <summary>
        ///     心率HR
        /// </summary>
        [Description("心率")]
        HeartRate = 11003,

        /// <summary>
        ///     血糖
        /// </summary>
        [Description("血糖")]
        BloodGlucose = 11004,

        /// <summary>
        ///     空腹血糖
        /// </summary>
        [Description("空腹血糖")]
        FBG = 11005,

        /// <summary>
        ///     餐后2h血糖PBG
        /// </summary>
        [Description("餐后2h血糖")]
        PBG = 11006,

        /// <summary>
        ///     随机血糖RBG
        /// </summary>
        [Description("随机血糖")]
        RBG = 11007,

        /// <summary>
        ///     体重BW
        /// </summary>
        [Description("体重")]
        Weight = 6,

        /// <summary>
        ///     尿蛋白定性
        /// </summary>
        [Description("尿蛋白定性")]
        Pro = 7,

        /// <summary>
        ///     尿蛋白肌酐比
        /// </summary>
        [Description("尿蛋白肌酐比")]
        ProICr = 8,

        /// <summary>
        ///     尿素氮
        /// </summary>
        [Description("尿素氮")]
        BUN = 9,

        /// <summary>
        ///     尿酸
        /// </summary>
        [Description("尿酸")]
        UA = 10,

        /// <summary>
        ///     肌酐
        /// </summary>
        [Description("肌酐")]
        SCr = 11,

        /// <summary>
        ///     肾小球滤过率
        /// </summary>
        [Description("肾小球滤过率")]
        eGFR = 12,

        /// <summary>
        ///     血红蛋白
        /// </summary>
        [Description("血红蛋白")]
        Hb = 13,

        /// <summary>
        ///     白蛋白
        /// </summary>
        [Description("白蛋白")]
        Alb = 14,

        /// <summary>
        ///     甘油三酯
        /// </summary>
        [Description("甘油三酯")]
        TG = 15,

        /// <summary>
        ///     胆固醇
        /// </summary>
        [Description("胆固醇")]
        Chol = 16,

        /// <summary>
        ///     钠
        /// </summary>
        [Description("钠")]
        Na = 17,

        /// <summary>
        ///     钾
        /// </summary>
        [Description("钾")]
        K = 18,

        /// <summary>
        ///     磷
        /// </summary>
        [Description("磷")]
        P = 19,

        /// <summary>
        ///     钙
        /// </summary>
        [Description("钙")]
        Ca = 20,

        /// <summary>
        ///     甲状旁腺素
        /// </summary>
        [Description("甲状旁腺素")]
        PTH = 21,

        /// <summary>
        ///     尿量
        /// </summary>
        [Description("尿量")]
        UrineVolume = 22,
        /// <summary>
        ///     尿红细胞
        /// </summary>
        [Description("尿红细胞")]
        ERY = 23,
        /// <summary>
        ///     尿白细胞
        /// </summary>
        [Description("尿白细胞")]
        LEU = 24,
        /// <summary>
        ///     甲状旁腺素
        /// </summary>
        [Description("尿白蛋白肌酐比")]
        UAICr = 25,
        /// <summary>
        ///     白细胞
        /// </summary>
        [Description("白细胞")]
        WBC = 26,
        /// <summary>
        ///     血小板
        /// </summary>
        [Description("血小板")]
        PLT = 27,
        /// <summary>
        ///     尿蛋白定量
        /// </summary>
        [Description("尿蛋白定量")]
        Upr = 28,
        /// <summary>
        ///     体重
        /// </summary>
        [Description("BMI")]
        BMI = 29,

    }

    public class GetUserInfoDto
    {
        public string CreateTime { get; set; }
        public string Birthday { get; set; }
        public string UserName { get; set; }
        public string Sex { get; set; }
        public string Status { get; set; }
        public string IdCard { get; set; }
        public string MobilePhone { get; set; }
        public string Id { get; set; }
        public string BelongToNurseId { get; set; }
        public string BelongToDoctorId { get; set; }
        public string UserType { get; set; }
        public string OpenId { get; set; }
        public string Duty { get; set; }
        public int JobTitle { get; set; }
        public double? Height { get; set; }
        public string Profile { get; set; }
        public bool IsRead{ get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Nurse Nurse { get; set; }
        public List<Dal.PatientsDisease> Disease { get; set; }
    }

    public class GetUserInfoParamsDto
    {
        public string Code { get; set; }
        public string OpenId { get; set; }
        /// <summary>
        /// 客户端类型，判定请求是来自，患者端还是医生端
        /// </summary>
        public string ClientType { get; set; }
    }

    public class MedicalIndicators
    {
        /// <summary>
        ///     体重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        ///     尿蛋白定性
        /// </summary>
        public string Pro { get; set; }

        /// <summary>
        ///     尿蛋白肌酐比
        /// </summary>
        public string ProICr { get; set; }

        /// <summary>
        ///     尿素氮
        /// </summary>
        public string BUN { get; set; }

        /// <summary>
        ///     尿酸
        /// </summary>
        public string UA { get; set; }

        /// <summary>
        ///     肌酐
        /// </summary>

        public string SCr { get; set; }

        /// <summary>
        ///     肾小球滤过率
        /// </summary>
        public string eGFR { get; set; }

        /// <summary>
        ///     血红蛋白
        /// </summary>

        public string Hb { get; set; }

        /// <summary>
        ///     白蛋白
        /// </summary>

        public string Alb { get; set; }

        /// <summary>
        ///     甘油三酯
        /// </summary>

        public string TG { get; set; }

        /// <summary>
        ///     胆固醇
        /// </summary>

        public string Chol { get; set; }

        /// <summary>
        ///     钠
        /// </summary>
        [Description("钠")]
        public string Na { get; set; }

        /// <summary>
        ///     钾
        /// </summary>

        public string K { get; set; }

        /// <summary>
        ///     磷
        /// </summary>
        public string P { get; set; }

        /// <summary>
        ///     钙
        /// </summary>
        public string Ca { get; set; }

        /// <summary>
        ///     甲状旁腺素
        /// </summary>
        public string PTH { get; set; }

        /// <summary>
        ///     尿红细胞
        /// </summary>

        public string ERY { get; set; }

        /// <summary>
        ///     尿白细胞
        /// </summary>
        public string LEU { get; set; }

        /// <summary>
        ///     尿白蛋白肌酐比
        /// </summary>
        public string UAICr { get; set; }

        /// <summary>
        ///     白细胞
        /// </summary>
        public string WBC { get; set; }

        /// <summary>
        ///     血小板
        /// </summary>
        public string PLT { get; set; }

        /// <summary>
        ///     尿蛋白定量
        /// </summary>
        public string Upr { get; set; }
        
    }

    public class UpdatePatientDisease
    {
        public int CDKLeave { get; set; }
        public List<Dal.PatientsDisease> Disease { get; set; }
        public int PatientId { get; set; }
    }

    public enum PatientsDataFormType
    {
        /// <summary>
        /// 空腹血糖
        /// </summary>
        [Description("空腹血糖")]
        FastingBloodGlucose = 1,

        /// <summary>
        ///     早餐后血糖
        /// </summary>
        [Description("早餐后血糖")]
        BreakfastBloodGlucose = 2,

        /// <summary>
        ///     午餐后血糖
        /// </summary>
        [Description("午餐后血糖")]
        LunchBloodGlucose = 3,

        /// <summary>
        ///     晚餐后血糖
        /// </summary>
        [Description("晚餐后血糖")]
        DinnerBloodGlucose = 4,

        /// <summary>
        ///     随机血糖
        /// </summary>
        [Description("随机血糖")]
        RandomBloodGlucose = 5,

        

        /// <summary>
        ///     随机血糖
        /// </summary>
        [Description("无")]
        None = 0,
    }

    public enum ReportType
    {
        /// <summary>
        /// 空腹血糖
        /// </summary>
        [Description("血常规")]
        RoutinebBlood = 1,

        /// <summary>
        ///     尿常规
        /// </summary>
        [Description("尿常规")]
        RoutineUrine = 2,

        /// <summary>
        ///     泌尿系统彩超
        /// </summary>
        [Description("泌尿系统彩超")]
        UrinarySystemColorDopplerUltrasonography = 3,

        /// <summary>
        ///     尿蛋白定量检查
        /// </summary>
        [Description("尿蛋白定量检查")]
        UrineProteinCheck = 4,

        /// <summary>
        ///     生化
        /// </summary>
        [Description("生化")]
        Biochemical = 5,



        /// <summary>
        ///     肾活检
        /// </summary>
        [Description("肾活检")]
        RenalBiopsy = 6,
        /// <summary>
        ///     甲状旁腺激素(PTH)
        /// </summary>
        [Description("甲状旁腺激素(PTH)")]
        PTH = 7,
        /// <summary>
        ///     铁代谢
        /// </summary>
        [Description("铁代谢")]
        IronMetabolism = 8,
        /// <summary>
        ///     铁代谢
        /// </summary>
        [Description("肾病指数")]
        KidneyDiseaseIndex = 10,

    }

    public class CurrentInfoDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OpenId { get; set; }
        public string QueryDate { get; set; }
    }

    public class CurrentInfoReturnDto
    {
        public List<List<CurrentInfoListDto>> MyRecord { get; set; }
        public List<List<CurrentInfoListDto>> MyReport { get; set; }
    }

    public class GetMyRecordHistoryDto
    {
        /// <summary>
        /// 收缩压
        /// </summary>
        public List<string> SystolicPressure { get; set; }
        /// <summary>
        /// 舒张压
        /// </summary>
        public List<string> DiastolicPressure { get; set; }
        /// <summary>
        /// 心率
        /// </summary>
        public List<string> HeartRate { get; set; }
        /// <summary>
        /// 空腹血糖
        /// </summary>
        public List<string> FastingBloodGlucose { get; set; }
        /// <summary>
        /// 早餐后血糖
        /// </summary>
        public List<string> BreakfastBloodGlucose { get; set; }
        /// <summary>
        /// 午餐后血糖
        /// </summary>
        public List<string> LunchBloodGlucose { get; set; }
        /// <summary>
        /// 晚餐后血糖
        /// </summary>
        public List<string> DinnerBloodGlucose { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public List<string> Weight { get; set; }
        /// <summary>
        /// 尿量
        /// </summary>
        public List<string> UrineVolume { get; set; }
        /// <summary>
        /// 随机血糖
        /// </summary>
        public List<string> RandomBloodGlucose { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public List<string> Date { get; set; }
        /// <summary>
        /// BMI
        /// </summary>
        public List<string> BMI { get; set; }
    }

    public class CurrentInfoListDto
    {
        public int? DataCode   { get; set; }
        public string DataValue  { get; set; }
        public string DataName { get; set; }
        public string CreateTime { get; set; }
        public string RecordTime { get; set; }
        public string RecordDate { get; set; }
        public string ReportName { get; set; }
        public string Unit { get; set; }
        public bool IsNomoal { get; set; }
        public DateTime TimeForOrder { get; set; }
    }

    public class  ReportAndHistoryReturnDto{
        public List<ReportHistoryReturnDto> ReportHistory { get; set; }
        public List<ReportDto> ReportItem { get; set; }
    }

    public class ReportHistoryReturnDto
    {
        public List<string> Xdata { get; set; }
        public List<string> Values { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public string DataCode { get; set; }
    }


    /// <summary>
    /// 重置密码
    /// </summary>
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public int? UserType { get; set; }
        public string MobiePhone { get; set; }
        public string AuthenCode { get; set; }
    }

    /// <summary>
    /// 验证码相关
    /// </summary>
    public class AuthenCodeInfoDto
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string AuthenCode { get; set; }
        /// <summary>
        /// 验证码输入错误次数
        /// </summary>
        public int CodeErrorTimes { get; set; }
        /// <summary>
        /// 获取验证码次数
        /// </summary>
        public int GetCodeTimes { get; set; }
    }

}
