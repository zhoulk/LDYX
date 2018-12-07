using System;

namespace LTGame
{
    /// <summary>
    /// 事件调度器
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// 判断给定事件是否存在事件监听器
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在事件监听器</returns>
        bool HasListeners(string eventName);

        /// <summary>
        /// 触发一个事件,并获取事件监听器的返回结果
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">参数</param>
        /// <returns>事件结果</returns>
        void Trigger(string eventName, params object[] args);

        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="execution">事件调用方法</param>
        /// <param name="group">事件分组，为<code>Null</code>则不进行分组</param>
        IEvent On(string eventName, Action<string, object[]> execution, object group = null);

        /// <summary>
        /// 更新
        /// </summary>
        void Update();

        /// <summary>
        /// 解除注册的事件监听器
        /// </summary>
        /// <param name="target">
        /// 事件解除目标
        /// <para>如果传入的是字符串(<code>string</code>)将会解除对应事件名的所有事件</para>
        /// <para>如果传入的是事件对象(<code>IEvent</code>)那么解除对应事件</para>
        /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
        /// </param>
        void Off(object target);
    }
}