namespace LTGame
{
    /// <summary>
    /// 事件对象
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 原始事件名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 事件分组
        /// </summary>
        object Group { get; }

        /// <summary>
        /// Unity事件分组
        /// </summary>
        UnityEngine.Component UnityGroup { get; }

        /// <summary>
        /// 调用事件
        /// </summary>
        /// <param name="eventName">完整的事件名</param>
        /// <param name="args">参数</param>
        /// <returns>事件结果</returns>
        void Call(string eventName, params object[] args);
    }
}