using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.WebServerConsole
{
    public enum HttpStatusCode
    {
        // 信息性状态码
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,

        // 成功状态码
        OK = 200,
        Created = 201,
        Accepted = 202,
        NoContent = 204,

        // 重定向状态码
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        NotModified = 304,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        // 客户端错误状态码
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        PayloadTooLarge = 413,
        UriTooLong = 414,
        UnsupportedMediaType = 415,
        RangeNotSatisfiable = 416,
        ExpectationFailed = 417,

        // 服务器错误状态码
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,
        VariantAlsoNegotiates = 506
    }

    public static class HttpStatusCodeExtensions
    {
        private static readonly Dictionary<HttpStatusCode, string> _statusCodeDescriptions = new Dictionary<HttpStatusCode, string>
    {
        { HttpStatusCode.Continue, "继续" },
        { HttpStatusCode.SwitchingProtocols, "切换协议" },
        { HttpStatusCode.Processing, "处理中" },
        { HttpStatusCode.OK, "请求成功" },
        { HttpStatusCode.Created, "创建成功" },
        { HttpStatusCode.Accepted, "已接受" },
        { HttpStatusCode.NoContent, "无内容" },
        { HttpStatusCode.MultipleChoices, "多种选择" },
        { HttpStatusCode.MovedPermanently, "永久移动" },
        { HttpStatusCode.Found, "找到" },
        { HttpStatusCode.NotModified, "未修改" },
        { HttpStatusCode.TemporaryRedirect, "临时重定向" },
        { HttpStatusCode.PermanentRedirect, "永久重定向" },
        { HttpStatusCode.BadRequest, "错误的请求" },
        { HttpStatusCode.Unauthorized, "未授权" },
        { HttpStatusCode.Forbidden, "禁止访问" },
        { HttpStatusCode.NotFound, "未找到" },
        { HttpStatusCode.MethodNotAllowed, "方法不允许" },
        { HttpStatusCode.NotAcceptable, "不可接受" },
        { HttpStatusCode.ProxyAuthenticationRequired, "代理认证要求" },
        { HttpStatusCode.RequestTimeout, "请求超时" },
        { HttpStatusCode.Conflict, "冲突" },
        { HttpStatusCode.Gone, "已消失" },
        { HttpStatusCode.LengthRequired, "需要长度" },
        { HttpStatusCode.PreconditionFailed, "前提条件失败" },
        { HttpStatusCode.PayloadTooLarge, "负载太大" },
        { HttpStatusCode.UriTooLong, "URI太长" },
        { HttpStatusCode.UnsupportedMediaType, "不支持的媒体类型" },
        { HttpStatusCode.RangeNotSatisfiable, "范围无法满足" },
        { HttpStatusCode.ExpectationFailed, "期望失败" },
        { HttpStatusCode.InternalServerError, "内部服务器错误" },
        { HttpStatusCode.NotImplemented, "未实现" },
        { HttpStatusCode.BadGateway, "网关错误" },
        { HttpStatusCode.ServiceUnavailable, "服务不可用" },
        { HttpStatusCode.GatewayTimeout, "网关超时" },
        { HttpStatusCode.HttpVersionNotSupported, "HTTP版本不支持" },
        { HttpStatusCode.VariantAlsoNegotiates, "变体也协商" },
    };

        public static string GetDescription(this HttpStatusCode statusCode)
        {
            return _statusCodeDescriptions.ContainsKey(statusCode) ? _statusCodeDescriptions[statusCode] : "未知状态码";
        }
    }

    // 使用示例
    public class HttpResponse
    {
        public int StatusCode { get; set; }
        public string? StatusDescription { get; set; }

        public void SetStatusCode(HttpStatusCode statusCode)
        {
            StatusCode = (int)statusCode;
            StatusDescription = statusCode.GetDescription();
        }
    }

 
 
}
