/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：摇杆信息类
 * 
 * ------------------------------------------------------------------------------*/

namespace LTGame
{
    /// <summary>
    /// 摇杆
    /// </summary>
    public class Rocker : ADevice
    {
        public float X;
        public float Y;

        public Rocker(byte id)
        {
            this.DevID = id;
        }
    }
}
