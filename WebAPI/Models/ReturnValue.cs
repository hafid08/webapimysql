using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public abstract class ReturnValue
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
    public class FailedReturnValue : ReturnValue
    {
        public FailedReturnValue(Exception ex)
        {
            SetStatus();
            Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            Data = null;
        }
        public FailedReturnValue(dynamic payload)
        {
            SetStatus();
            Message = "";
            Data = payload;
        }
        public FailedReturnValue(string message)
        {
            SetStatus();
            Message = message;
            Data = null;
        }

        public FailedReturnValue()
        {
            SetStatus();
            Data = null;
        }

        private void SetStatus()
        {
            Status = "failed";
        }

    }
    public class SuccessReturnValue : ReturnValue
    {
        public SuccessReturnValue()
        {
            Status = "success";
            Message = "";
        }
        public SuccessReturnValue(dynamic payload)
        {
            Status = "success";
            Message = "";
            Data = payload;
        }

    }
}
