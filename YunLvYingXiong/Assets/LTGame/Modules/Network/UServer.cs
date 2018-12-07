/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：UDP 广播服务器
 * 
 * ------------------------------------------------------------------------------*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class UServer
{
    UdpClient client;
    CancellationTokenSource tocken;

    public UServer()
    {
        client = new UdpClient();
        tocken = new CancellationTokenSource();

        string ipAddress = Network.player.ipAddress;

        Task.Run(() =>
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 7777);
            byte[] bytes = Encoding.UTF8.GetBytes(ipAddress + "-3899-" + "Unity-手柄测试服务器");

            while (!tocken.IsCancellationRequested)
            {
                client.Send(bytes, bytes.Length, ep);
                Thread.Sleep(1500);
            }
        }, tocken.Token);
    }

    public void Dispose()
    {
        tocken.Cancel();
        client.Close();
    }
}
