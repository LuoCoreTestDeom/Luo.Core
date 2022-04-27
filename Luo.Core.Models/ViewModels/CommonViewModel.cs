using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.ViewModels
{
    public class CommonViewModel
    {
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string Msg { get; set; }
        public string ResultData { get; set; }
    }
   
    public class CommonViewModel<T>
    {
        public bool Status { get; set; }
        public string StatusCode { get; set; }
        public string Msg { get; set; }
        public T ResultData { get; set; }
    }
}
