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
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Furion.Reflection;

/// <summary>
///     创建对象类型实例属性值访问器
/// </summary>
/// <typeparam name="T">对象类型</typeparam>
public sealed class ObjectPropertyGetter<T> where T : class
{
    /// <summary>
    ///     反射搜索成员方式
    /// </summary>
    internal const BindingFlags _defaultBindingFlags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    ///     对象类型实例属性值访问器集合
    /// </summary>
    internal readonly ConcurrentDictionary<string, Func<object, object?>> _propertyGetters = new();

    /// <summary>
    ///     <inheritdoc cref="ObjectPropertyGetter{T}" />
    /// </summary>
    /// <param name="bindingFlags">反射搜索成员方式</param>
    public ObjectPropertyGetter(BindingFlags? bindingFlags = null) => Initialize(bindingFlags);

    /// <summary>
    ///     初始化对象类型实例属性值访问器
    /// </summary>
    /// <param name="bindingFlags">反射搜索成员方式</param>
    internal void Initialize(BindingFlags? bindingFlags = null)
    {
        // 获取对象类型
        var type = typeof(T);
        var bindingAttr = bindingFlags ?? _defaultBindingFlags;

        // 获取所有符合反射搜索成员方式的属性集合
        var properties = type.GetProperties(bindingAttr).Where(u => u.CanRead).ToList();

        // 遍历属性集合创建属性值访问器并存储到集合中
        foreach (var property in properties)
        {
            _propertyGetters.TryAdd(property.Name, type.CreatePropertyGetter(property));
        }
    }

    /// <summary>
    ///     尝试获取属性值访问器
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <param name="propertyGetter">属性值访问器</param>
    /// <returns>
    ///     <see cref="bool" />
    /// </returns>
    public bool TryGetPropertyGetter(string propertyName, [NotNullWhen(true)] out Func<object, object?>? propertyGetter)
    {
        // 空检查
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        return _propertyGetters.TryGetValue(propertyName, out propertyGetter);
    }

    /// <summary>
    ///     获取属性值访问器
    /// </summary>
    /// <param name="propertyName">属性名称</param>
    /// <returns>
    ///     <see cref="Func{T1, T2}" />
    /// </returns>
    /// <exception cref="ArgumentException"></exception>
    public Func<object, object?> GetPropertyGetter(string propertyName)
    {
        // 尝试获取属性值访问器
        if (!TryGetPropertyGetter(propertyName, out var propertyGetter))
        {
            throw new ArgumentException(
                $"Property `{propertyName}` not found on type `{typeof(T)}`. Ensure that the BindingFlags used allow access to this property.",
                nameof(propertyName));
        }

        return propertyGetter;
    }

    /// <summary>
    ///     获取属性值访问器集合
    /// </summary>
    /// <returns>
    ///     <see cref="IDictionary{TKey,TValue}" />
    /// </returns>
    public IDictionary<string, Func<object, object?>> GetPropertyGetters() =>
        _propertyGetters.ToDictionary(u => u.Key, u => u.Value);

    /// <summary>
    ///     获取属性值
    /// </summary>
    /// <param name="instance"><typeparamref name="T" /> 类型实例</param>
    /// <param name="propertyName">属性名称</param>
    /// <returns>
    ///     <see cref="object" />
    /// </returns>
    public object? GetPropertyValue(object instance, string propertyName)
    {
        // 空检查
        ArgumentNullException.ThrowIfNull(instance);

        return GetPropertyGetter(propertyName)(instance);
    }

    /// <summary>
    ///     获取属性值
    /// </summary>
    /// <param name="instance"><typeparamref name="T" /> 类型实例</param>
    /// <param name="propertyName">属性名称</param>
    /// <typeparam name="TProperty">属性值目标类型</typeparam>
    /// <returns>
    ///     <typeparamref name="TProperty" />
    /// </returns>
    public TProperty? GetPropertyValue<TProperty>(object instance, string propertyName) =>
        (TProperty?)GetPropertyValue(instance, propertyName);
}