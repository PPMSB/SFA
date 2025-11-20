using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNet.pod.DTOs
{
    public class ResultDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

    }
}