/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：kcp信道
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;

namespace LTGame.Network
{
    /// <summary>
    /// kcp信道
    /// </summary>
    public class KChannel
    {
        /// <summary>
        /// 最大发送缓冲，如果大于此值，则丢弃后续包
        /// </summary>
        protected const int MaxWaitSnd = 64;

        /// <summary>
        /// 判断网络恢复正常的缓冲队列数
        /// </summary>
        protected const int NormalWaitSnd = 32;

        /// <summary>
        /// Udp客户端
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// 收到的数据的网络端点(来源)
        /// </summary>
        private IPEndPoint listenEndPoint;

        /// <summary>
        /// 远程端点
        /// </summary>
        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// Kcp
        /// </summary>
        private KCP kcp;

        /// <summary>
        /// 下一次刷新时间
        /// </summary>
        private uint nextUpdateTime;

        /// <summary>
        /// 会话Id
        /// </summary>
        private readonly uint conv;

        /// <summary>
        /// 断线计时器
        /// </summary>
        private uint timer;

        /// <summary>
        /// 超时时间
        /// </summary>
        private uint outTime = 5000;

        /// <summary>
        /// 链接状态
        /// </summary>
        private bool connected;

        /// <summary>
        /// 链接状态回调
        /// </summary>
        public Action<uint, bool> OnConnectState;

        /// <summary>
        /// 消息回调
        /// </summary>
        public Action<uint, byte[]> OnRecive;

        /// <summary>
        /// KCP信道
        /// </summary>
        /// <param name="conv">会话id</param>
        /// <param name="client">下层协议</param>
        /// <param name="remoteHostname">目标主机地址</param>
        /// <param name="remotePort">目标端口</param>
        public KChannel(uint conv, UdpClient client, string remoteHostname, int remotePort)
        {
            this.conv = conv;
            this.client = client;
            this.remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteHostname), remotePort);
            this.connected = true;
            this.timer = SystemTime.Clock();
            this.OnConnectState?.Invoke(conv, connected);

            InitKcp(conv);
        }

        /// <summary>
        /// KCP信道
        /// </summary>
        /// <param name="conv">会话id</param>
        /// <param name="client">下层协议</param>
        /// <param name="remoteEndPoint">远程端点</param>
        public KChannel(uint conv, UdpClient client, IPEndPoint remoteEndPoint)
        {
            this.conv = conv;
            this.client = client;
            this.remoteEndPoint = remoteEndPoint;
            this.connected = true;
            this.timer = SystemTime.Clock();
            this.OnConnectState?.Invoke(conv, connected);

            InitKcp(conv);
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="data">发送数据</param>
        /// <returns> </returns>
        public bool Send(byte[] data)
        {
            if (kcp == null || !connected) return false;

            if (kcp.WaitSnd() < MaxWaitSnd)
            {
                kcp.Send(data);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 更新Kcp，由外部调用
        /// </summary>
        public void Update()
        {
            Update(SystemTime.Clock());
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        /// <param name="current">时钟</param>
        private void Update(uint current)
        {
            if (kcp == null || !connected)
            {
                return;
            }

            if (SystemTime.Clock() - timer > outTime)
            {
                connected = false;
                OnConnectState?.Invoke(conv, connected);
            }

            if (current < nextUpdateTime)
            {
                return;
            }

            kcp.Update(current);
            nextUpdateTime = kcp.Check(current);
        }

        /// <summary>
        /// 初始化Kcp
        /// </summary>
        /// <param name="conv">会话</param>
        private void InitKcp(uint conv)
        {
            kcp = new KCP(conv, (buf, size) =>
            {
                client.Send(buf, size, remoteEndPoint);
            });

            kcp.NoDelay(1, 1, 2, 1);
            kcp.WndSize(256, 256);
        }

        /// <summary>
        /// 将数据Input进Kcp待处理队列
        /// </summary>
        /// <param name="recvBuf"> 接收到的数据</param>
        public void Input(byte[] recvBuf)
        {
            //刷新超时时间
            timer = SystemTime.Clock();

            //将收到的数据打入kcp中处理
            kcp.Input(recvBuf);

            for (var size = kcp.PeekSize(); size > 0; size = kcp.PeekSize())
            {
                var buffer = new byte[size];
                if (kcp.Recv(buffer) > 0)
                {
                    //从kcp缓存池中取出排列处理后的数据
                    OnRecive.Invoke(conv, buffer);
                }
            }
        }

        /// <summary>
        /// 返回发送缓存队列数量
        /// </summary>
        /// <returns></returns>
        public int WaitSnd()
        {
            if (kcp == null)
                return -1;

            return kcp.WaitSnd();
        }

        /// <summary>
        /// 设置超时断线时间
        /// </summary>
        /// <param name="time"></param>
        public void SetOutTime(uint time)
        {
            this.outTime = time;
        }

        /// <summary>
        /// 释放时
        /// </summary>
        public void Dispose()
        {
            kcp = null;
        }
    }
}

