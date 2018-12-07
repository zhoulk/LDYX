using System;

namespace LTGame
{
    /// <summary>
    /// 事件对象
    /// </summary>
    internal class Event : IEvent
    {
        /// <summary>
        /// 原始事件名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 事件根源对象
        /// </summary>
        public object Group { get; private set; }

        /// <summary>
        /// Unity的根源对象
        /// </summary>
        public UnityEngine.Component UnityGroup { get; private set; }

        /// <summary>
        /// 事件执行器
        /// </summary>
        private readonly Action<string, object[]> execution;

        /// <summary>
        /// 创建一个事件对象
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="group">事件分组</param>
        /// <param name="execution">事件执行器</param>
        public Event(string eventName, object group, Action<string, object[]> execution)
        {
            Name = eventName;
            Group = group;
            UnityGroup = Group as UnityEngine.Component;
            this.execution = execution;
        }

        /// <summary>
        /// 调用事件处理函数
        /// </summary>
        /// <param name="eventName">调用事件的完整名字</param>
        /// <param name="args">事件参数</param>
        public void Call(string eventName, params object[] args)
        {
            execution(eventName, args);
        }
    }
}