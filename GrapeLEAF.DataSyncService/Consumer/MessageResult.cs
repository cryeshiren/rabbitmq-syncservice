using System;

namespace GrapeLEAF.DataSyncService
{
    public class MessageResult
    {
        public Boolean success { get; set; }

        public String msg { get; set; }

        public int code { get; set; }

        public object data { get; set; }

        public MessageResult(bool success, string msg, int code, object data)
        {
            this.success = success;
            this.msg = msg;
            this.code = code;
            this.data = data;
        }


        public MessageResult()
        {
        }

        public static MessageResult Fail(int code, String msg = "")
        {
            return new MessageResult
            {
                success = false,
                msg = msg,
                code = code
            };
        }

        public static MessageResult Success(String data)
        {
            return new MessageResult
            {
                success = true,
                data = data
            };
        }

        public static MessageResult Success(String data, string msg)
        {
            return new MessageResult
            {
                success = true,
                data = data,
                msg = msg
            };
        }

        public static MessageResult Success()
        {
            return Success(string.Empty);
        }


        public static MessageResult<T> Success<T>(T data)
        {
            return new MessageResult<T>
            {
                success = true,
                Data = data
            };
        }

        public static MessageResult<T> Fail<T>(int code, T data, String msg = "")
        {
            return new MessageResult<T>
            {
                success = false,
                msg = msg,
                code = code,
                Data = data
            };
        }
    }

    /// <summary>
    /// 返回参数定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MessageResult<T> : MessageResult
    {
        /// <summary>数据
        /// </summary>
        public T Data { get; set; }

        public MessageResult() { }

        public MessageResult(bool success, string msg, int code, T data)
        {
            this.success = success;
            this.msg = msg;
            this.code = code;
            this.Data = data;
        }

        /// <summary>失败
        /// </summary>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static MessageResult<T> Fail(String msg, int code)
        {
            return new MessageResult<T>
            {
                success = false,
                msg = msg,
                code = code
            };
        }
        /// <summary>失败 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="code">错误code</param>
        /// <param name="msg">定义失败信息</param>
        /// <returns></returns>
        public static MessageResult<T> Fail(T data, int code, String msg = "")
        {
            return new MessageResult<T>
            {
                success = false,
                msg = msg,
                Data = data,
                code = code
            };
        }

        /// <summary>成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <param name="msg">可定义成功信息</param>
        /// <returns></returns>
        public static MessageResult<T> Success(T data, String msg)
        {
            return new MessageResult<T>
            {
                success = true,
                msg = msg,
                Data = data
            };
        }

        /// <summary>成功 
        /// </summary>
        /// <param name="data">要返回的数据泛型</param>
        /// <returns></returns>
        public static MessageResult<T> Success(T data)
        {
            return Success(data, string.Empty);
        }
    }
}
