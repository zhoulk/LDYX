namespace LTGame
{
    /// <summary>
    /// 触发器
    /// </summary>
    public class Trigger
    {
        /// <summary>
        /// 原始事件名
        /// </summary>
        public string EventName;

        /// <summary>
        /// 参数表
        /// </summary>
        public object[] Params;

        public Trigger(string eventName, object[] param)
        {
            EventName = eventName;
            Params = param;
        }
    }
}

