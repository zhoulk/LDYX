/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：自定义多人模式键值,以取代UnityEngine.KeyCode
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using System;

namespace LTGame
{
    public enum KeyCode2 : UInt16
    {
        Up = 19,
        Down = 20,
        Left = 21,
        Right = 22,
        A = 23,
        B = 97,
        X = 99,
        Y = 100,
        L = 102,
        R = 103,

        Up2 = 51,
        Down2 = 47,
        Left2 = 29,
        Right2 = 32,
        A2 = 40,
        B2 = 39,
        X2 = 38,
        Y2 = 37,
        L2 = 49,
        R2 = 43,

        Home = KeyCode.Home
    }
}
