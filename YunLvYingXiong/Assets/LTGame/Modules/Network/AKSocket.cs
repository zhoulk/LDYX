/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：KCP Socket
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;

namespace LTGame.Network
{
    /// <summary>
    /// KCP Socket
    /// </summary>
    public abstract class AKSocket
    {
        /// <summary>
        /// 下层UDP协议
        /// </summary>
        protected UdpClient client;

        /// <summary>
        /// 远程端点
        /// </summary>
        protected IPEndPoint remoteEndPoint;

        /// <summary>
        /// 下一次刷新时间
        /// </summary>
        protected uint nextUpdateTime;

        /// <summary>
        /// 接收消息回调
        /// </summary>
        public Action<uint, byte[]> OnReciveCallBack;


        public AKSocket()
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        }
    }
}

