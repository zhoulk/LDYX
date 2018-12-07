/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：手柄服务端,统一了Sdk Server,测试脚本Server的使用
 * 
 * ------------------------------------------------------------------------------*/

using System;
using UnityEngine;
using LTGame.Network;

namespace LTGame.SDK
{
    public class GamepadServer : IGamepadServer
    {
        private Action<IMessage> Callback;
        private AndroidJavaClass jc;
        private UServer uServer;
        private KServer kServer;
        private IPackager packager;
        private bool initSdk;

        public GamepadServer()
        {
            try
            {
                jc = new AndroidJavaClass("org.unity.UnityAPIBridge");
            }
            catch (Exception)
            {
                //如果加载sdk失败，则启动脚本Server,且实例化对应的解包器
                uServer = new UServer();
                kServer = new KServer(3899) { OnReciveCallBack = OnReciveMessage };
                packager = new CShaperPackager();
            }
        }

        public void Update()
        {
            kServer?.Update();
        }

        /// <summary>
        /// C# Server回调
        /// </summary>
        /// <param name="conv"></param>
        /// <param name="buf"></param>
        private void OnReciveMessage(uint conv, byte[] buf)
        {
            IMessage msg = packager.Decode(buf);

            if (msg != null)
            {
                Callback?.Invoke(msg);
            }
        }

        /// <summary>
        /// 接收sdk的回调消息
        /// </summary>
        /// <param name="data"></param>
        public void ReciveMessage(string data)
        {
            if (!initSdk)
            {
                //当第一次收到消息回调时，先确定新旧版本的sdk解析器
                if (data.IndexOf('-') > 0)
                    packager = new SdkPackager1();
                else
                    packager = new SdkPackager2();

                initSdk = true;
            }

            IMessage msg = packager.Decode(data);
            if (msg != null)
            {
                Callback?.Invoke(msg);
            }
        }

        public void On(Action<IMessage> callback)
        {
            if (Callback != null) Debug.LogError("GamepadServer Callback is not Null!");

            Callback = callback;
        }

        public void Off()
        {
            Callback = null;
        }

        public void Dispose()
        {
            kServer?.Dispose();
            uServer?.Dispose();
        }
    }
}

