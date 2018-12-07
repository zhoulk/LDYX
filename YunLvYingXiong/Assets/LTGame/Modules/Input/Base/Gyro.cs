/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：陀螺仪信息类
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;

namespace LTGame
{
    public class Gyro : ADevice
    {
        /// <summary>
        /// 重力加速度
        /// </summary>
        public Vector3 Gravity;

        /// <summary>
        /// 无重力的加速度
        /// </summary>
        public Vector3 UserAcceleration;

        /// <summary>
        /// 旋转速度
        /// </summary>
        public Vector3 RotationRate;

        /// <summary>
        /// 四元素
        /// </summary>
        public Quaternion Attitude;

        public Gyro(byte id)
        {
            this.DevID = id;
        }
    }
}

