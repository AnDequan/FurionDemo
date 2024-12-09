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

using Furion.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace Furion.HttpRemote.Extensions;

/// <summary>
///     <see cref="HttpContext" /> 拓展类
/// </summary>
public static partial class HttpContextExtensions
{
    /// <summary>
    ///     忽略在转发时需要跳过的响应标头列表。
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Content-Type: </term>
    ///             <description>
    ///                 非标准的 <c>Content-Type</c> 值（例如 <c>text/plain; charset=utf-8</c>
    ///                 ）可能会导致“No output formatter was found for content types 'text/plain; charset=utf-8, text/plain;
    ///                 charset=utf-8' to write the response.”错误。忽略此标头以防止此类错误。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Content-Length: </term>
    ///             <description>
    ///                 若响应标头中包含 <c>Content-Length</c>，且其值与实际响应体大小不符，则可能引发“Error while copying content to a
    ///                 stream.”。忽略此标头有助于解决因长度不匹配引起的错误。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Transfer-Encoding: </term>
    ///             <description>当响应标头包含 <c>Transfer-Encoding: chunked</c> 时，可能导致响应处理过程无限期挂起。忽略此标头可避免该问题。</description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal static HashSet<string> _ignoreResponseHeaders =
    [
        "Content-Type", "Content-Length", "Connection", "Transfer-Encoding", "Keep-Alive", "Upgrade", "Proxy-Connection"
    ];

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static HttpResponseMessage Forward(this HttpContext? httpContext, string? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method),
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static HttpResponseMessage Forward(this HttpContext? httpContext, HttpMethod httpMethod,
        string? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward(httpContext, httpMethod,
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static HttpResponseMessage Forward(this HttpContext? httpContext, Uri? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method), requestUri,
            configure, completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static HttpResponseMessage Forward(this HttpContext? httpContext, HttpMethod httpMethod,
        Uri? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(httpMethod);

        // 创建 HttpContextForwardBuilder 实例
        var httpContextForwardBuilder = CreateForwardBuilder(httpContext, httpMethod, requestUri, forwardOptions);

        // 构建 HttpRequestBuilder 实例
        var httpRequestBuilder = httpContextForwardBuilder.Build(configure);

        // 获取 IHttpRemoteService 实例
        var httpRemoteService = httpContext.RequestServices.GetRequiredService<IHttpRemoteService>();

        // 发送 HTTP 远程请求
        var httpResponseMessage =
            httpRemoteService.Send(httpRequestBuilder, completionOption, httpContext.RequestAborted);

        // 根据配置选项将 HttpResponseMessage 信息转发到 HttpContext 中
        ForwardResponseMessage(httpContext, httpResponseMessage, httpContextForwardBuilder.ForwardOptions);

        return httpResponseMessage;
    }

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static Task<HttpResponseMessage> ForwardAsync(this HttpContext? httpContext, string? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method),
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static Task<HttpResponseMessage> ForwardAsync(this HttpContext? httpContext, HttpMethod httpMethod,
        string? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync(httpContext, httpMethod,
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static Task<HttpResponseMessage> ForwardAsync(this HttpContext? httpContext, Uri? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method), requestUri,
            configure, completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpResponseMessage" />
    /// </returns>
    public static async Task<HttpResponseMessage> ForwardAsync(this HttpContext? httpContext, HttpMethod httpMethod,
        Uri? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(httpMethod);

        // 创建 HttpContextForwardBuilder 实例
        var httpContextForwardBuilder = CreateForwardBuilder(httpContext, httpMethod, requestUri, forwardOptions);

        // 构建 HttpRequestBuilder 实例
        var httpRequestBuilder = await httpContextForwardBuilder.BuildAsync(configure);

        // 获取 IHttpRemoteService 实例
        var httpRemoteService = httpContext.RequestServices.GetRequiredService<IHttpRemoteService>();

        // 发送 HTTP 远程请求
        var httpResponseMessage = await httpRemoteService.SendAsync(httpRequestBuilder, completionOption,
            httpContext.RequestAborted);

        // 根据配置选项将 HttpResponseMessage 信息转发到 HttpContext 中
        ForwardResponseMessage(httpContext, httpResponseMessage, httpContextForwardBuilder.ForwardOptions);

        return httpResponseMessage;
    }

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static HttpRemoteResult<TResult> Forward<TResult>(this HttpContext? httpContext, string? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward<TResult>(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method),
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static HttpRemoteResult<TResult> Forward<TResult>(this HttpContext? httpContext, HttpMethod httpMethod,
        string? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward<TResult>(httpContext, httpMethod,
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static HttpRemoteResult<TResult> Forward<TResult>(this HttpContext? httpContext, Uri? requestUri = null,
        Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        Forward<TResult>(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method), requestUri,
            configure, completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static HttpRemoteResult<TResult> Forward<TResult>(this HttpContext? httpContext, HttpMethod httpMethod,
        Uri? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(httpMethod);

        // 创建 HttpContextForwardBuilder 实例
        var httpContextForwardBuilder = CreateForwardBuilder(httpContext, httpMethod, requestUri, forwardOptions);

        // 构建 HttpRequestBuilder 实例
        var httpRequestBuilder = httpContextForwardBuilder.Build(configure);

        // 获取 IHttpRemoteService 实例
        var httpRemoteService = httpContext.RequestServices.GetRequiredService<IHttpRemoteService>();

        // 发送 HTTP 远程请求
        var result = httpRemoteService.Send<TResult>(httpRequestBuilder, completionOption, httpContext.RequestAborted);

        // 根据配置选项将 HttpResponseMessage 信息转发到 HttpContext 中
        ForwardResponseMessage(httpContext, result.ResponseMessage, httpContextForwardBuilder.ForwardOptions);

        return result;
    }

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static Task<HttpRemoteResult<TResult>> ForwardAsync<TResult>(this HttpContext? httpContext,
        string? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync<TResult>(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method),
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static Task<HttpRemoteResult<TResult>> ForwardAsync<TResult>(this HttpContext? httpContext,
        HttpMethod httpMethod, string? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync<TResult>(httpContext, httpMethod,
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute), configure,
            completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static Task<HttpRemoteResult<TResult>> ForwardAsync<TResult>(this HttpContext? httpContext,
        Uri? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null) =>
        ForwardAsync<TResult>(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method), requestUri,
            configure, completionOption, forwardOptions);

    /// <summary>
    ///     转发 <see cref="HttpContext" /> 到新的 HTTP 远程地址
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="configure">自定义配置委托</param>
    /// <param name="completionOption">
    ///     <see cref="HttpCompletionOption" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <typeparam name="TResult">转换的目标类型</typeparam>
    /// <returns>
    ///     <see cref="HttpRemoteResult{TResult}" />
    /// </returns>
    public static async Task<HttpRemoteResult<TResult>> ForwardAsync<TResult>(this HttpContext? httpContext,
        HttpMethod httpMethod, Uri? requestUri = null, Action<HttpRequestBuilder>? configure = null,
        HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead,
        HttpContextForwardOptions? forwardOptions = null)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(httpMethod);

        // 创建 HttpContextForwardBuilder 实例
        var httpContextForwardBuilder = CreateForwardBuilder(httpContext, httpMethod, requestUri, forwardOptions);

        // 构建 HttpRequestBuilder 实例
        var httpRequestBuilder = await httpContextForwardBuilder.BuildAsync(configure);

        // 获取 IHttpRemoteService 实例
        var httpRemoteService = httpContext.RequestServices.GetRequiredService<IHttpRemoteService>();

        // 发送 HTTP 远程请求
        var result = await httpRemoteService.SendAsync<TResult>(httpRequestBuilder, completionOption,
            httpContext.RequestAborted);

        // 根据配置选项将 HttpResponseMessage 信息转发到 HttpContext 中
        ForwardResponseMessage(httpContext, result.ResponseMessage, httpContextForwardBuilder.ForwardOptions);

        return result;
    }

    /// <summary>
    ///     创建 <see cref="HttpContextForwardBuilder" /> 实例
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpContextForwardBuilder" />
    /// </returns>
    public static HttpContextForwardBuilder CreateForwardBuilder(this HttpContext? httpContext, HttpMethod httpMethod,
        string? requestUri = null, HttpContextForwardOptions? forwardOptions = null) =>
        CreateForwardBuilder(httpContext, httpMethod,
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute),
            forwardOptions);

    /// <summary>
    ///     创建 <see cref="HttpContextForwardBuilder" /> 实例
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpContextForwardBuilder" />
    /// </returns>
    public static HttpContextForwardBuilder CreateForwardBuilder(this HttpContext? httpContext,
        string? requestUri = null,
        HttpContextForwardOptions? forwardOptions = null) =>
        CreateForwardBuilder(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method),
            string.IsNullOrWhiteSpace(requestUri) ? null : new Uri(requestUri, UriKind.RelativeOrAbsolute),
            forwardOptions);

    /// <summary>
    ///     创建 <see cref="HttpContextForwardBuilder" /> 实例
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpMethod">请求方式</param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpContextForwardBuilder" />
    /// </returns>
    public static HttpContextForwardBuilder CreateForwardBuilder(this HttpContext? httpContext, HttpMethod httpMethod,
        Uri? requestUri = null, HttpContextForwardOptions? forwardOptions = null) =>
        new(httpContext, httpMethod, requestUri, forwardOptions);

    /// <summary>
    ///     创建 <see cref="HttpContextForwardBuilder" /> 实例
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="requestUri">请求地址。若为空则尝试从请求标头 <c>X-Forward-To</c> 中获取目标地址。</param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    /// <returns>
    ///     <see cref="HttpContextForwardBuilder" />
    /// </returns>
    public static HttpContextForwardBuilder CreateForwardBuilder(this HttpContext? httpContext, Uri? requestUri = null,
        HttpContextForwardOptions? forwardOptions = null) =>
        new(httpContext, Helpers.ParseHttpMethod(httpContext?.Request.Method), requestUri, forwardOptions);

    /// <summary>
    ///     根据配置选项将 <see cref="HttpResponseMessage" /> 信息转发到 <see cref="HttpContext" /> 中
    /// </summary>
    /// <param name="httpContext">
    ///     <see cref="HttpContext" />
    /// </param>
    /// <param name="httpResponseMessage">
    ///     <see cref="HttpResponseMessage" />
    /// </param>
    /// <param name="forwardOptions">
    ///     <see cref="HttpContextForwardOptions" />
    /// </param>
    internal static void ForwardResponseMessage(HttpContext httpContext, HttpResponseMessage httpResponseMessage,
        HttpContextForwardOptions forwardOptions)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(httpResponseMessage);
        ArgumentNullException.ThrowIfNull(forwardOptions);

        // 获取 HttpResponse 实例
        var httpResponse = httpContext.Response;

        // 检查是否配置了响应状态码转发
        if (forwardOptions.WithResponseStatusCode)
        {
            httpResponse.StatusCode = (int)httpResponseMessage.StatusCode;
        }

        // 检查是否配置了响应标头转发
        if (forwardOptions.WithResponseHeaders)
        {
            ForwardHttpHeaders(httpResponse, httpResponseMessage.Headers);
        }

        // 检查是否配置了响应内容标头转发
        if (forwardOptions.WithResponseContentHeaders)
        {
            ForwardHttpHeaders(httpResponse, httpResponseMessage.Content.Headers);
        }

        // 调用用于在转发响应之前执行自定义操作
        forwardOptions.OnForward?.Invoke(httpContext, httpResponseMessage);
    }

    /// <summary>
    ///     转发 HTTP 标头
    /// </summary>
    /// <param name="httpResponse">
    ///     <see cref="HttpResponse" />
    /// </param>
    /// <param name="httpHeaders">
    ///     <see cref="HttpHeaders" />
    /// </param>
    internal static void ForwardHttpHeaders(HttpResponse httpResponse, HttpHeaders httpHeaders)
    {
        // 逐条更新响应标头
        foreach (var (key, values) in httpHeaders)
        {
            // 忽略特定响应标头
            if (key.IsIn(_ignoreResponseHeaders, StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            httpResponse.Headers[key] = values.ToArray();
        }
    }
}