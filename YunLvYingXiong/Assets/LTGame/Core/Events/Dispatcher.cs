using System;
using System.Collections.Generic;
using UnityEngine;

namespace LTGame
{
    /// <summary>
    /// 事件调度器
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        /// <summary>
        /// 分组事件(以对象为单位分组)
        /// </summary>
        private readonly Dictionary<object, List<IEvent>> groups;

        /// <summary>
        /// 普通事件
        /// </summary>
        private readonly Dictionary<string, List<IEvent>> listeners;

        /// <summary>
        /// 触发时间队列
        /// </summary>
        private readonly Queue<Trigger> triggers;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot;

        /// <summary>
        /// 构建事件调度器0
        /// </summary>
        public Dispatcher()
        {
            syncRoot = new object();
            groups = new Dictionary<object, List<IEvent>>();
            listeners = new Dictionary<string, List<IEvent>>();
            triggers = new Queue<Trigger>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            lock (syncRoot)
            {
                while (triggers.Count > 0)
                {
                    Trigger t = triggers.Dequeue();

                    foreach (var listener in GetListeners(t.EventName))
                    {
                        if (listener.UnityGroup != null && listener.UnityGroup.gameObject.activeInHierarchy)
                        {
                            listener.Call(t.EventName, t.Params);
                        }
                        else if (listener.UnityGroup == null)
                        {
                            listener.Call(t.EventName, t.Params);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断给定事件是否存在事件监听器
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>是否存在事件监听器</returns>
        public bool HasListeners(string eventName)
        {
            lock (syncRoot)
            {
                if (listeners.ContainsKey(eventName))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 触发一个事件,并获取事件监听器的返回结果
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">参数</param>
        public void Trigger(string eventName, params object[] args)
        {
            Dispatch(eventName, args);
        }

        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="execution">执行方法</param>
        /// <param name="group">事件分组，如果为<code>Null</code>则不进行分组</param>
        /// <returns>事件对象</returns>
        public IEvent On(string eventName, Action<string, object[]> execution, object group = null)
        {
            Guard.NotEmptyOrNull(eventName, "eventName");
            Guard.Requires<ArgumentNullException>(execution != null);

            lock (syncRoot)
            {
                var result = SetupListen(eventName, execution, group);

                if (group == null)
                {
                    return result;
                }

                List<IEvent> listener;
                if (!groups.TryGetValue(group, out listener))
                {
                    groups[group] = listener = new List<IEvent>();
                }
                listener.Add(result);

                return result;
            }
        }

        /// <summary>
        /// 解除注册的事件监听器
        /// </summary>
        /// <param name="target">
        /// 事件解除目标
        /// <para>如果传入的是字符串(<code>string</code>)将会解除对应事件名的所有事件</para>
        /// <para>如果传入的是事件对象(<code>IEvent</code>)那么解除对应事件</para>
        /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
        /// </param>
        public void Off(object target)
        {
            Guard.Requires<ArgumentNullException>(target != null);

            lock (syncRoot)
            {
                var baseEvent = target as IEvent;
                if (baseEvent != null)
                {
                    Forget(baseEvent);
                    return;
                }

                if (target is string)
                {
                    DismissEventName(target as string);
                }

                DismissTargetObject(target);
            }
        }

        /// <summary>
        /// 调度事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="param">参数</param>
        /// <returns>处理结果</returns>
        private void Dispatch(string eventName, params object[] param)
        {
            Guard.Requires<ArgumentNullException>(eventName != null);

            lock (syncRoot)
            {
                //TODO 优化点,主线程的事件，立刻触发，非主线程，推进队列
                triggers.Enqueue(new Trigger(eventName, param));

                //foreach (var listener in GetListeners(eventName))
                //{
                //    listener.Call(eventName, param);
                //}
            }
        }

        /// <summary>
        /// 获取指定事件的事件列表
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <returns>事件列表</returns>
        private IEnumerable<IEvent> GetListeners(string eventName)
        {
            var outputs = new List<IEvent>();

            List<IEvent> result;
            if (listeners.TryGetValue(eventName, out result))
            {
                outputs.AddRange(result);
            }

            return outputs;
        }

        /// <summary>
        /// 根据普通事件解除相关事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        private void DismissEventName(string eventName)
        {
            List<IEvent> events;
            if (!listeners.TryGetValue(eventName, out events))
            {
                return;
            }

            foreach (var element in events.ToArray())
            {
                Forget(element);
            }
        }

        /// <summary>
        /// 根据Object解除事件
        /// </summary>
        /// <param name="target">事件解除目标</param>
        private void DismissTargetObject(object target)
        {
            List<IEvent> events;
            if (!groups.TryGetValue(target, out events))
            {
                return;
            }

            foreach (var element in events.ToArray())
            {
                Forget(element);
            }
        }

        /// <summary>
        /// 从事件调度器中移除指定的事件监听器
        /// </summary>
        /// <param name="target">事件监听器</param>
        private void Forget(IEvent target)
        {
            lock (syncRoot)
            {
                List<IEvent> events;
                if (target.Group != null && groups.TryGetValue(target.Group, out events))
                {
                    events.Remove(target);
                    if (events.Count <= 0)
                    {
                        groups.Remove(target.Group);
                    }
                }

                ForgetListen(target);
            }
        }

        /// <summary>
        /// 销毁普通事件
        /// </summary>
        /// <param name="target">事件对象</param>
        private void ForgetListen(IEvent target)
        {
            List<IEvent> events;
            if (!listeners.TryGetValue(target.Name, out events))
            {
                return;
            }

            events.Remove(target);
            if (events.Count <= 0)
            {
                listeners.Remove(target.Name);
            }
        }

        /// <summary>
        /// 设定普通事件
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="execution">事件调用方法</param>
        /// <param name="group">事件分组</param>
        /// <returns>监听事件</returns>
        private IEvent SetupListen(string eventName, Action<string, object[]> execution, object group)
        {
            List<IEvent> listener;
            if (!listeners.TryGetValue(eventName, out listener))
            {
                listeners[eventName] = listener = new List<IEvent>();
            }

            var output = new Event(eventName, group, execution);
            listener.Add(output);
            return output;
        }
    }
}