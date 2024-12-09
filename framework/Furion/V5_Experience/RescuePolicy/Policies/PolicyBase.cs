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

namespace Furion.RescuePolicy;

/// <summary>
///     策略抽象基类
/// </summary>
/// <typeparam name="TResult">操作返回值类型</typeparam>
public abstract class PolicyBase<TResult> : IExceptionPolicy<TResult>
{
    /// <inheritdoc />
    public string? PolicyName { get; set; }

    /// <inheritdoc />
    public virtual void Execute(Action operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行同步操作方法
        Execute(() =>
        {
            operation();

            return default;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual void Execute(Action<CancellationToken> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行同步操作方法
        Execute(token =>
        {
            operation(token);

            return default;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行异步操作方法
        await ExecuteAsync(async () =>
        {
            await operation();

            return default;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task ExecuteAsync(Func<CancellationToken, Task> operation,
        CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行异步操作方法
        await ExecuteAsync(async token =>
        {
            await operation(token);

            return default;
        }, cancellationToken);
    }

    /// <inheritdoc />
    public virtual TResult? Execute(Func<TResult?> operation, CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteAsync(() => Task.FromResult(operation()), cancellationToken)
            .GetAwaiter()
            .GetResult();
    }

    /// <inheritdoc />
    public virtual TResult? Execute(Func<CancellationToken, TResult?> operation,
        CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteAsync(token => Task.FromResult(operation(token)), cancellationToken)
            .GetAwaiter()
            .GetResult();
    }

    /// <inheritdoc />
    public virtual async Task<TResult?> ExecuteAsync(Func<Task<TResult?>> operation,
        CancellationToken cancellationToken = default)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(operation);

        // 执行异步操作方法
        return await ExecuteAsync(async _ => await operation(), cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TResult?> ExecuteAsync(Func<CancellationToken, Task<TResult?>> operation,
        CancellationToken cancellationToken = default);
}