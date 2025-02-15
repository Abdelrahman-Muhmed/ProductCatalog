using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog_BLL.Helpers.Exceptions
{
    public class BusinessException : Exception
    {
        public int ErrorCode { get; set; }

        public BusinessException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
