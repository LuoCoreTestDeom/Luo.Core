using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Models
{
    

    public class ApiCommonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool State { get; set; } = false;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public string ResponseValue { get; set; }
    }

    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class ApiCommonResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool State { get; set; } = false;
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 开发者信息
        /// </summary>
        public string DevInfo { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T ResponseValue { get; set; }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiCommonResult<T> Success(string msg)
        {
            return ResponseMessage(true, msg, default);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ApiCommonResult<T> Success(string msg, T response)
        {
            return ResponseMessage(true, msg, response);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiCommonResult<T> Fail(string msg)
        {
            return ResponseMessage(false, msg, default);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ApiCommonResult<T> Fail(string msg, T response)
        {
            return ResponseMessage(false, msg, response);
        }
        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="success">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ApiCommonResult<T> ResponseMessage(bool success, string msg, T response)
        {
            return new ApiCommonResult<T>() { Msg = msg, ResponseValue = response, State = success };
        }
    }
}
