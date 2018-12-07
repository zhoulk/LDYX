/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：C# 手柄测试Server 的解包器
 * 
 * ------------------------------------------------------------------------------*/

namespace LTGame.Network
{
    public class CShaperPackager : IPackager
    {
        MessageHeader header;
        MessageKeyboard keyboard;
        MessageRocker rocker;
        MessageGyro gyro;

        public CShaperPackager()
        {
            header = new MessageHeader();
            keyboard = new MessageKeyboard();
            rocker = new MessageRocker();
            gyro = new MessageGyro();
        }

        public virtual IMessage Decode(byte[] bytes)
        {
            header.Clear();
            header.Decode(bytes, 0);

            switch (header.GetMessageType())
            {
                case MessageType.Keyboard:
                    keyboard.Clear();
                    keyboard.Decode(bytes, 0);
                    return keyboard;

                case MessageType.Rocker:
                    rocker.Clear();
                    rocker.Decode(bytes, 0);
                    return rocker;

                case MessageType.Gyro:
                    gyro.Clear();
                    gyro.Decode(bytes, 0);
                    return gyro;
            }

            return null;
        }

        public virtual IMessage Decode(string data) { return null; }
    }

}
