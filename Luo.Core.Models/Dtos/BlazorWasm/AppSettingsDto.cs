using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models.Dtos.BlazorWasm
{
    [Serializable]
    public class AppSettingsDto
    {
        public string ConnectionString { get; set; }
    }
}
