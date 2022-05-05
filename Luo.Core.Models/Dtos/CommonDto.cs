using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos
{
    public  class CommonDto
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ResultData { get; set; }
    }
    public class CommonDto<T>
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T ResultData { get; set; }
    }
}
