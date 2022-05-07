using Luo.Core.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.IServices
{
    public interface ISharedService
    {
        public List<UserMenuInfoOutput> GetUserMenuInfos(int userId);
    }
}
