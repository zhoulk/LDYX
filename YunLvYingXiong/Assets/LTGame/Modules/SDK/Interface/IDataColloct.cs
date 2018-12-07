/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：数据采集接口,记录游戏时长，等级等数据
 * 
 * ------------------------------------------------------------------------------*/

namespace LTGame.SDK
{
    public interface IDataColloct
    {
        /// <summary>
        /// 上传关卡开始的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        void StartLevel(int level);

        /// <summary>
        /// 上传闯关成功的时间点
        /// </summary>
        /// <param name="level"> 关卡id </param>
        void PassLevel(int level);

        /// <summary>
        /// 上传闯关失败的时间点
        /// </summary>
        /// <param name="level"> 关卡id  </param>
        void FailLevel(int level);

        /// <summary>
        /// 上传玩家等级
        /// </summary>
        /// <param name="value"> 玩家当前等级 </param>
        void SetPlayerGrade(int value);

        /// <summary>
        /// 上传进入页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        void StartPage(string pageName);

        /// <summary>
        /// 上传退出页面的时间点
        /// </summary>
        /// <param name="pageName"> 名称 </param>
        void EndPage(string pageName);

        /// <summary>
        /// 上传单/双人模式的开始时间点
        /// </summary>
        /// <param name="value"> 单人模式1，双人模式 2</param>
        void StartPlayMode(int value);

        /// <summary>
        /// 上传单/双人模式的结束时间点
        /// </summary>
        /// <param name="value">单人模式1，双人模式 2</param>
        void EndPlayMode(int value);

        /// <summary>
        /// 道具消耗接口
        /// </summary>
        /// <param name="item"> 物品名称 </param>
        /// <param name="level"> 关卡id </param>
        /// <param name="number"> 数量 </param>
        /// <param name="goldCoin"> 花费 </param>
        void Use(string item, int level, int number, int goldCoin);
    }
}