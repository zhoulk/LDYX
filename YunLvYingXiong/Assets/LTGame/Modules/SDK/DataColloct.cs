/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：数据采集实现
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;

namespace LTGame.SDK
{
    public class DataColloct : IDataColloct
    {
        private AndroidJavaClass jc;

        public DataColloct()
        {
            try
            {
                jc = new AndroidJavaClass("org.unity.UnityAPIBridge");
            }
            catch (Exception)
            {

            }
        }

        public DataColloct(AndroidJavaClass jc)
        {
            this.jc = jc;
        }

        public void StartLevel(int level)
        {
            jc?.CallStatic("startLevel", level);
        }

        public void PassLevel(int level)
        {
            jc?.CallStatic("finishLevel", level);
        }

        public void FailLevel(int level)
        {
            jc?.CallStatic("failLevel", level);
        }

        public void SetPlayerGrade(int value)
        {
            jc?.CallStatic("setPlayerLevel", value);
        }

        public void StartPage(string pageName)
        {
            jc?.CallStatic("onPageStart", pageName);
        }

        public void EndPage(string pageName)
        {
            jc?.CallStatic("onPageEnd", pageName);
        }

        public void StartPlayMode(int value)
        {
            jc?.CallStatic("onMultiplayerStart", "", value);
        }

        public void EndPlayMode(int value)
        {
            jc?.CallStatic("onMultiplayerEnd", "", value);
        }

        public void Use(string item, int level, int number, int goldCoin)
        {
            jc?.CallStatic("use", item, level, number, goldCoin);
        }
    }
}
