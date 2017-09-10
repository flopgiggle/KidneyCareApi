using System;

namespace KidneyCareApi.Common
{
    public class BusinessException:Exception
    {
        public Util.ErrorCode Code { get; set; }

        public BusinessException(Util.ErrorCode code)
        {
            Code = code;
        }

        public BusinessException()
        {
            Code = Util.ErrorCode.CommonError;
        }

        public BusinessException(string message):base(message)
        {
            Code = Util.ErrorCode.CommonError;
        }

        public BusinessException(Util.ErrorCode code,string message)
            : base(message)
        {
            Code = code;
        }
    }
}