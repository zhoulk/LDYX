/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：KCP 客户端
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace LTGame.Network
{
    /// <summary>
    /// KCP 客户端
    /// </summary>
    public class KClient : AKSocket
    {
        /// <summary>
        /// 会话id
        /// </summary>
        UInt32 conv;

        /// <summary>
        /// 信道
        /// </summary>
        KChannel channel;

        /// <summary>
        /// 构建KCP客户端
        /// </summary>
        /// <param name="conv">会话id</param>
        /// <param name="port">本地端口</param>
        public KClient(UInt32 conv, int port) : base()
        {
            this.conv = conv;
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            client.BeginReceive(OnRecive, null);
        }

        /// <summary>
        /// 链接远端
        /// </summary>
        /// <param name="remoteHostname"> 远端地址</param>
        /// <param name="remotePort">远端端口</param>
        public void Connect(string remoteHostname, int remotePort)
        {
            //建立KCP,并绑定回调
            channel = new KChannel(conv, client, remoteHostname, remotePort) { OnRecive = OnReciveCallBack };
            channel.OnConnectState = OnConnectState;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            channel?.Update();
        }

        /// <summary>
        /// 发送消息接口
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public void Send(byte[] bytes)
        {
            channel?.Send(bytes);
        }

        private void OnRecive(IAsyncResult ar)
        {
            if (client == null) return;

            try
            {
                byte[] buf = client.EndReceive(ar, ref remoteEndPoint);

                if (buf == null || buf.Length <= 0)
                {
                    Debug.LogError("Recive buff is null");
                    return;
                }

                if (buf.Length < KCP.IKCP_OVERHEAD) return;

                UInt32 ts = 0;
                UInt32 conv = 0;
                byte cmd = 0;

                KCP.ikcp_decode32u(buf, 0, ref conv);

                if (this.conv != conv) return;

                KCP.ikcp_decode8u(buf, 4, ref cmd);

                if (cmd == KCP.IKCP_CMD_ACK)
                {
                    //拦截ack包中的时间戳，作为ping值计算
                    KCP.ikcp_decode32u(buf, 8, ref ts);
                    App.Trigger(EventName.Ping, (SystemTime.Clock() - ts));
                }

                //推进kcp处理
                channel.Input(buf);
                client.BeginReceive(OnRecive, null);
            }
            catch (Exception)
            {
                Debug.LogError("host is closed.");
                Dispose();
            }
        }

        /// <summary>
        /// 链接状态
        /// </summary>
        /// <param name="conv"></param>
        /// <param name="state"></param>
        private void OnConnectState(uint conv, bool state)
        {
            if (!state)
                App.Trigger(EventName.ConnectState, new ConnectException(ConnectState.Disconnected));
        }

        public void Dispose()
        {
            channel.Dispose();
            client?.Close();
            client = null;
        }

        public KChannel Channel
        {
            get
            {
                return channel;
            }
        }
    }
}

