using System;
using System.Collections.Generic;
using System.Text;

namespace Finbourne_MemoryCache.Models
{
    public class ErrorResult
    {
        public string ExceptionMessage { get; set; }

        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }
    }
}
