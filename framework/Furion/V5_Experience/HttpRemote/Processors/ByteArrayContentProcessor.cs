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

using System.Net.Http.Headers;
using System.Text;

namespace Furion.HttpRemote;

/// <summary>
///     字节数组内容处理器
/// </summary>
public class ByteArrayContentProcessor : HttpContentProcessorBase
{
    /// <inheritdoc />
    public override bool CanProcess(object? rawContent, string contentType) =>
        rawContent is (ByteArrayContent or byte[]) and not (FormUrlEncodedContent or StringContent);

    /// <inheritdoc />
    public override HttpContent? Process(object? rawContent, string contentType, Encoding? encoding)
    {
        // 尝试解析 HttpContent 类型
        if (TryProcess(rawContent, contentType, encoding, out var httpContent))
        {
            return httpContent;
        }

        // 检查是否是字节数组类型
        if (rawContent is byte[] bytes)
        {
            // 初始化 ByteArrayContent 实例
            var byteArrayContent = new ByteArrayContent(bytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(contentType)
            {
                CharSet = encoding?.BodyName
            };

            return byteArrayContent;
        }

        throw new InvalidOperationException(
            $"Expected a byte array, but received an object of type `{rawContent.GetType()}`.");
    }
}