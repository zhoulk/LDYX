/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Kcp服务端
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace LTGame.Network
{
    public class KServer : AKSocket
    {
        /// <summary>
        /// 信道缓存
        /// </summary>
        private readonly Dictionary<uint, KChannel> channels = new Dictionary<uint, KChannel>();

        private readonly List<uint> removePool = new List<uint>();

        /// <summary>
        /// 接收缓存队列
        /// </summary>
        private SwitchQueue<byte[]> recvQueue;

        /// <summary>
        /// 创建kcp服务端
        /// </summary>
        /// <param name="port"></param>
        public KServer(int port = 7777) : base()
        {
            //创建下层协议,并绑定本地IP，端口
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            client.BeginReceive(OnRecive, null);

            recvQueue = new SwitchQueue<byte[]>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {

            foreach (var c in channels.Values)
            {
                c.Update();
            }

            foreach (var conv in removePool)
            {
                if (channels.ContainsKey(conv))
                {
                    channels.Remove(conv);
                    Debug.Log("断开一个链接，剩余链接" + channels.Count);
                }
            }
            removePool.Clear();

            recvQueue.Switch();
            while (!recvQueue.Empty())
            {
                if (OnReciveCallBack == null)
                    recvQueue.Clear();
                else
                    OnReciveCallBack?.Invoke(0, recvQueue.Pop());
            }
        }

        /// <summary>
        /// 发送消息接口
        /// </summary>
        /// <param name="conv"> 会话id</param>
        /// <param name="buf">数据</param>
        public void Send(uint conv, byte[] buf)
        {
            KChannel channel;

            if (channels.TryGetValue(conv, out channel))
            {
                channel.Send(buf);
            }
        }

        /// <summary>
        /// 接收消息线程
        /// </summary>
        private void OnRecive(IAsyncResult ar)
        {
            try
            {
                remoteEndPoint = null;
                byte[] buf = client.EndReceive(ar, ref remoteEndPoint);

                //如果缓冲区为null 或 长度少于等于0，则断线
                if (buf == null || buf.Length <= 0)
                {
                    Dispose();
                    Debug.LogError("kcp buff is None");
                    return;
                }

                //获取会话conv
                UInt32 conv = 0;
                KCP.ikcp_decode32u(buf, 0, ref conv);

                //查找信道池,没有则建立新的

                KChannel channel;
                if (!channels.TryGetValue(conv, out channel))
                {
                    channel = new KChannel(conv, client, remoteEndPoint) { OnRecive = OnRecive };
                    channel.OnConnectState = OnConnectState;
                    channels.Add(conv, channel);
                }

                channel.Input(buf);

                //再次调用异步接收
                client.BeginReceive(OnRecive, null);
            }
            catch (Exception)
            {
                //TODO 触发此处异常，一般由于客户端主动关闭致使S端无法返回数据包到C端。
                //TODO 服务器在此类异常中不需要关闭，只需要处理掉断开连接的C端
            }
        }

        /// <summary>
        /// channel消息回调
        /// </summary>
        /// <param name="conv"></param>
        /// <param name="bytes"></param>
        private void OnRecive(uint conv, byte[] bytes)
        {
            recvQueue.Push(bytes);
        }

        /// <summary>
        /// 链接状态
        /// </summary>
        /// <param name="conv"></param>
        /// <param name="state"></param>
        private void OnConnectState(uint conv, bool state)
        {
            if (channels.ContainsKey(conv))
            {
                removePool.Add(conv);
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            foreach (var c in channels.Values)
            {
                c.Dispose();
            }

            client.Close();
        }
    }
}
