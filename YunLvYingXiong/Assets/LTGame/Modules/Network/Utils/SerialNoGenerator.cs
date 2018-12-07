/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述： 序列号生成器
 * 
 * ------------------------------------------------------------------------------*/

/// <summary>
/// 序列号生成器
/// </summary>
public static class SerialNoGenerator
{
    private static ushort serialNo;

    /// <summary>
    /// 返回序列号
    /// </summary>
    /// <returns></returns>
    public static ushort SerialNo()
    {
        serialNo++;
        serialNo %= ushort.MaxValue;
        return serialNo;
    }
}
