﻿// ------------------------------------------------------------------------
// 版权信息
// 版权归百小僧及百签科技（广东）有限公司所有。
// 所有权利保留。
// 官方网站：https://baiqian.com
//
// 许可证信息
// Furion 项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。
// 许可证的完整文本可以在源代码树根目录中的 LICENSE-APACHE 和 LICENSE-MIT 文件中找到。
// 官方网站：https://furion.net
//
// 使用条款
// 使用本代码应遵守相关法律法规和许可证的要求。
//
// 免责声明
// 对于因使用本代码而产生的任何直接、间接、偶然、特殊或后果性损害，我们不承担任何责任。
//
// 其他重要信息
// Furion 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。
// 有关 Furion 项目的其他详细信息，请参阅位于源代码树根目录中的 COPYRIGHT 和 DISCLAIMER 文件。
//
// 更多信息
// 请访问 https://gitee.com/dotnetchina/Furion 获取更多关于 Furion 项目的许可证和版权信息。
// ------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Furion.HttpRemote;

/// <summary>
///     HTTP 远程请求选项
/// </summary>
public sealed class HttpRemoteOptions
{
    /// <summary>
    ///     默认 JSON 序列化配置
    /// </summary>
    /// <remarks>参考文献：https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json/configure-options。</remarks>
    public static readonly JsonSerializerOptions JsonSerializerOptionsDefault = new(JsonSerializerOptions.Default)
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    /// <summary>
    ///     默认请求内容类型
    /// </summary>
    public string? DefaultContentType { get; set; } = Constants.TEXT_PLAIN_MIME_TYPE;

    /// <summary>
    ///     默认文件下载保存目录
    /// </summary>
    public string? DefaultFileDownloadDirectory { get; set; }

    /// <summary>
    ///     请求分析工具日志级别
    /// </summary>
    /// <remarks>默认值为 <see cref="LogLevel.Warning" /></remarks>
    public LogLevel ProfilerLogLevel { get; set; } = LogLevel.Warning;

    /// <summary>
    ///     JSON 序列化配置
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonSerializerOptionsDefault;

    /// <summary>
    ///     自定义 HTTP 声明式 <see cref="IHttpDeclarativeExtractor" /> 集合提供器
    /// </summary>
    /// <value>返回多个包含实现 <see cref="IHttpDeclarativeExtractor" /> 集合的集合。</value>
    internal IReadOnlyList<Func<IEnumerable<IHttpDeclarativeExtractor>>>? HttpDeclarativeExtractors { get; set; }

    /// <summary>
    ///     指示是否配置（注册）了日志程序
    /// </summary>
    internal bool IsLoggingRegistered { get; set; }
}