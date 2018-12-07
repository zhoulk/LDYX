/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：Udp客户端
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UClient
{
    /// <summary>
    /// 消息回调
    /// </summary>
    public Action<byte[]> Callback;

    /// <summary>
    /// Udp客户端
    /// </summary>
    private UdpClient client;

    /// <summary>
    /// 收到的数据的网络端点(来源)
    /// </summary>
    private IPEndPoint listenEndPoint;

    public UClient(int port = 7777)
    {
        client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
        client.BeginReceive(OnRecive, null);
    }

    private void OnRecive(IAsyncResult ar)
    {
        try
        {
            byte[] buf = client.EndReceive(ar, ref listenEndPoint);

            //如果缓冲区为null 或 长度少于等于0，则释放
            if (buf == null || buf.Length <= 0)
            {
                Dispose();
                Debug.LogError("udp buff is None");
                return;
            }

            Callback?.Invoke(buf);

            //再次调用异步接收
            client.BeginReceive(OnRecive, null);
        }
        catch (Exception)
        {
            Dispose();
        }
    }

    public void Dispose()
    {
        client.Close();
    }
}
