/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Android 桥接类,提供Android 调用 Unity的方法
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LTGame.SDK
{
    /// <summary>
    /// Android 桥接类
    /// </summary>
    public class AndroidAPIBridge : MonoBehaviour
    {
        /// <summary>
        /// 手柄消息回调
        /// </summary>
        /// <param name="data"></param>
        public void NetKeyEvent(string data)
        {
            App.GamepadServer.ReciveMessage(data);
        }

        /// <summary>
        /// 支付回调
        /// </summary>
        /// <param name="data"></param>
        public void PaymentBack(string data)
        {
            App.Trigger(EventName.PaymentResult, data);
        }
    }
}
