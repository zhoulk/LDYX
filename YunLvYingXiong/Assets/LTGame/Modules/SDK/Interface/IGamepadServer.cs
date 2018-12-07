/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：手柄服务器接口
 * 
 * ------------------------------------------------------------------------------*/

using System;
using LTGame.Network;

namespace LTGame.SDK
{
    public interface IGamepadServer
    {
        /// <summary>
        /// 更新
        /// </summary>
        void Update();

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="data"></param>
        void ReciveMessage(string data);

        /// <summary>
        /// 注入消息回调
        /// </summary>
        /// <param name="callback"></param>
        void On(Action<IMessage> callback);

        /// <summary>
        /// 注销回调
        /// </summary>
        /// <param name="callback"></param>
        void Off();

        /// <summary>
        /// 销毁
        /// </summary>
        void Dispose();
    }
}
