using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class Result<T>
    {
        public Result()
        {

        }
        public Result(T? data)
        {
            Status = true;
            Message = string.Empty;
            Data = data;
        }
        public T ? Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
    public class Result
    {
        public Result()
        {
            Status = true;
            Message = string.Empty;
            StatusCode = string.Empty;
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }

    public class CustPagedResult<T> : PagedResult<T>
    {
        public CustPagedResult()
        {
            Status = true;
            Message = string.Empty;
            StatusCode = string.Empty;
        }
        public T? Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}
