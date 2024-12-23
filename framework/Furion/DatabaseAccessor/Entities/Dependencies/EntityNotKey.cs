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

namespace Furion.DatabaseAccessor;

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
[SuppressSniffer]
public abstract class EntityNotKey : EntityNotKey<MasterDbContextLocator>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2> : PrivateEntityNotKey
    where TDbContextLocator2 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator4">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3, TDbContextLocator4> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
    where TDbContextLocator4 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator4">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator5">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3, TDbContextLocator4, TDbContextLocator5> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
    where TDbContextLocator4 : class, IDbContextLocator
    where TDbContextLocator5 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator4">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator5">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator6">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3, TDbContextLocator4, TDbContextLocator5, TDbContextLocator6> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
    where TDbContextLocator4 : class, IDbContextLocator
    where TDbContextLocator5 : class, IDbContextLocator
    where TDbContextLocator6 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator4">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator5">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator6">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator7">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3, TDbContextLocator4, TDbContextLocator5, TDbContextLocator6, TDbContextLocator7> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
    where TDbContextLocator4 : class, IDbContextLocator
    where TDbContextLocator5 : class, IDbContextLocator
    where TDbContextLocator6 : class, IDbContextLocator
    where TDbContextLocator7 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体依赖基接口
/// </summary>
/// <typeparam name="TDbContextLocator1">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator2">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator3">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator4">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator5">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator6">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator7">数据库上下文定位器</typeparam>
/// <typeparam name="TDbContextLocator8">数据库上下文定位器</typeparam>
[SuppressSniffer]
public abstract class EntityNotKey<TDbContextLocator1, TDbContextLocator2, TDbContextLocator3, TDbContextLocator4, TDbContextLocator5, TDbContextLocator6, TDbContextLocator7, TDbContextLocator8> : PrivateEntityNotKey
    where TDbContextLocator1 : class, IDbContextLocator
    where TDbContextLocator2 : class, IDbContextLocator
    where TDbContextLocator3 : class, IDbContextLocator
    where TDbContextLocator4 : class, IDbContextLocator
    where TDbContextLocator5 : class, IDbContextLocator
    where TDbContextLocator6 : class, IDbContextLocator
    where TDbContextLocator7 : class, IDbContextLocator
    where TDbContextLocator8 : class, IDbContextLocator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    public EntityNotKey(string name) : base(name)
    {
    }
}

/// <summary>
/// 数据库无键实体基类（禁止外部继承）
/// </summary>
[SuppressSniffer]
public abstract class PrivateEntityNotKey : IPrivateEntityNotKey
{
    /// <summary>
    /// 无键实体名
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// 无键实体 schema
    /// </summary>
    private readonly string _schema;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="name">数据库中定义名</param>
    /// <param name="schema"></param>
    public PrivateEntityNotKey(string name, string schema = default)
    {
        _name = name;
        _schema = schema;
    }

    /// <summary>
    /// 获取视图名称
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return _name;
    }

    /// <summary>
    ///  数据库中定义的 Schema
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetSchema()
    {
        return _schema;
    }
}