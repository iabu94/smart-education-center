using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smart_education_center.Common
{
    public class Enum
    {
        public enum IsActive
        {
            YES = 1,
            NO = 0
        }

        public enum IsDeleted
        {
            YES = 1,
            NO = 0
        }

        public enum ResultCode
        {
            SUCCESS = 1,
            FAILED = 2 
        }
    }
}