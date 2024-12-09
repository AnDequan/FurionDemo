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

using System.Text;

namespace Furion.HttpRemote;

/// <summary>
///     Server-Sent Events 事件流格式
/// </summary>
/// <remarks>参考文献：https://developer.mozilla.org/zh-CN/docs/Web/API/Server-sent_events/Using_server-sent_events#%E5%AD%97%E6%AE%B5。</remarks>
public sealed class ServerSentEventsData
{
    /// <summary>
    ///     消息数据构建器
    /// </summary>
    internal readonly StringBuilder _dataBuffer;

    /// <summary>
    ///     消息数据缓存字段
    /// </summary>
    internal string? _cachedData;

    /// <summary>
    ///     <inheritdoc cref="ServerSentEventsData" />
    /// </summary>
    internal ServerSentEventsData() => _dataBuffer = new StringBuilder();

    /// <summary>
    ///     事件类型
    /// </summary>
    /// <remarks>
    ///     一个用于标识事件类型的字符串。如果指定了这个字符串，浏览器会将具有指定事件名称的事件分派给相应的监听器；网站源代码应该使用 <c>addEventListener()</c>
    ///     来监听指定的事件。如果一个消息没有指定事件名称，那么 <c>onmessage</c> 处理程序就会被调用。
    /// </remarks>
    public string? Event { get; internal set; }

    /// <summary>
    ///     消息
    /// </summary>
    /// <remarks>消息的数据字段。当 <c>EventSource</c> 接收到多个以 <c>data</c>: 开头的连续行时，会将它们连接起来，在它们之间插入一个换行符。末尾的换行符会被删除。</remarks>
    public string Data => _cachedData ??= _dataBuffer.ToString();

    /// <summary>
    ///     事件 ID
    /// </summary>
    /// <remarks>事件 ID，会成为当前 <c>EventSource</c> 对象的内部属性“最后一个事件 ID 的属性值。</remarks>
    public string? Id { get; internal set; }

    /// <summary>
    ///     重新连接的时间
    /// </summary>
    /// <remarks>重新连接的时间。如果与服务器的连接丢失，浏览器将等待指定的时间，然后尝试重新连接。这必须是一个整数，以毫秒为单位指定重新连接的时间。如果指定了一个非整数值，该字段将被忽略。</remarks>
    public int Retry { get; internal set; }

    /// <summary>
    ///     追加消息数据
    /// </summary>
    /// <param name="value">消息数据</param>
    internal void AppendData(string? value)
    {
        _dataBuffer.Append(value);
        _cachedData = null;
    }
}