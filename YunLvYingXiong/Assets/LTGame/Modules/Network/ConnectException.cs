/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：链接异常类
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace LTGame.Network
{
    public enum ConnectState
    {
        Init,
        Connected,
        IPInvaild,
        Disconnected,
        Close,
        RemoteClose
    }

    /// <summary>
    /// 链接异常类
    /// </summary>
    public class ConnectException
    {
        Dictionary<ConnectState, string> connectInfo = new Dictionary<ConnectState, string>
        {
            { ConnectState.Init,"初始化" },
            { ConnectState.Connected,"链接成功" },
            { ConnectState.IPInvaild,"IP地址无效,或非同一局域网" },
            { ConnectState.Disconnected,"目标主机已关闭或网络通讯异常，请检查网络" },
            { ConnectState.RemoteClose,"目标主机已关闭或网络通讯异常，请检查网络" },
            { ConnectState.Close,"链接已断开" },
        };

        public ConnectState State;
        public string Message;

        public ConnectException(ConnectState connectState)
        {
            State = connectState;
            Message = connectInfo[connectState];
        }
    }
}

