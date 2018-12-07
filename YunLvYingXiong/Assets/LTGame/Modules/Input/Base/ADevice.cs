/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：输入设备抽象类
 * 
 * ------------------------------------------------------------------------------*/
namespace LTGame
{
    /// <summary>
    /// 抽象设备类
    /// </summary>
    public abstract class ADevice : IDevice
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public byte DevID;

        public virtual void Update() { }
    }
}
