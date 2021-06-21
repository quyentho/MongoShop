using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoShop.Models.Account
{
    public class ExternalLoginResponse
    {
        public ExternalLoginResponseStatus ResponseStatus { get; set; }

        public string Message { get; set; }
    }

    public enum ExternalLoginResponseStatus
    {
        Success,
        Error,
        Fail
    }
}
