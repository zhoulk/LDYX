using System;

namespace LTGame
{
    /// <summary>
    /// 事件调度器扩展
    /// </summary>
    public static class DispatcherEx
    {
        /// <summary>
        /// 注册一个事件监听器
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="action">事件处理方法</param>
        /// <param name="group">事件分组</param>
        /// <returns>事件对象</returns>
        public static IEvent On(this Dispatcher self, string eventName, Action action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke();
            }, group);
        }

        public static IEvent On<T0>(this Dispatcher self, string eventName, Action<T0> action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            var paramInfos = action.Method.GetParameters();
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke((T0)userParams[0]);
            }, group);
        }

        public static IEvent On<T0, T1>(this Dispatcher self, string eventName, Action<T0, T1> action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            var paramInfos = action.Method.GetParameters();
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke((T0)userParams[0], (T1)userParams[1]);
            }, group);
        }

        public static IEvent On<T0, T1, T2>(this Dispatcher self, string eventName, Action<T0, T1, T2> action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            var paramInfos = action.Method.GetParameters();
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke((T0)userParams[0], (T1)userParams[1], (T2)userParams[2]);
            }, group);
        }

        public static IEvent On<T0, T1, T2, T3>(this Dispatcher self, string eventName, Action<T0, T1, T2, T3> action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            var paramInfos = action.Method.GetParameters();
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke((T0)userParams[0], (T1)userParams[1], (T2)userParams[2], (T3)userParams[3]);
            }, group);
        }

        public static IEvent On<T0, T1, T2, T3, T4>(this Dispatcher self, string eventName, Action<T0, T1, T2, T3, T4> action, object group = null)
        {
            Guard.Requires<ArgumentNullException>(action != null);
            var paramInfos = action.Method.GetParameters();
            return self.On(eventName, (e, userParams) =>
            {
                action.Invoke((T0)userParams[0], (T1)userParams[1], (T2)userParams[2], (T3)userParams[3], (T4)userParams[4]);
            }, group);
        }
    }
}
