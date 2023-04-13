using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class ResponseModel<T>
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int Empid { get; set; }
    }
}
