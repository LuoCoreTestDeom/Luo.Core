using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common.BlazorWasm
{
    /// <summary>
	/// 操作LocalStorage帮助类
	/// </summary>
	public class LocalStorageHelper
    {
        private readonly IJSRuntime _jsRuntime;
        public LocalStorageHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        /// <summary>
        /// 设置LocalStorage
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetLocalStorage(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("LocalStorageSet", key, value);
        }
        /// <summary>
        /// 获取LocalStorage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetLocalStorage(string key)
        {
            return await _jsRuntime.InvokeAsync<string>("LocalStorageGet", key);
        }
    }
}
