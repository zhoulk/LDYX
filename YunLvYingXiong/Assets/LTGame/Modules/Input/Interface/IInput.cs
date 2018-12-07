/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：输入接口
 * 
 * ------------------------------------------------------------------------------*/

namespace LTGame
{
    /// <summary>
    /// 输入接口
    /// </summary>
    public interface IInput
    {
        void Update();
        void FixedUpdate();
        void LateUpdate();
    }
}

