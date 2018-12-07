/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：自定义输入类，支持多人模式，支持虚拟手柄 ，支持键盘输入
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using LTGame.Network;
using UnityEngine;

namespace LTGame
{
    public class LTInput : IInput
    {
        private static List<KeyCode2> singleKeydownPool = new List<KeyCode2>();
        private static List<KeyCode2> singleKeyupPool = new List<KeyCode2>();

        private static List<KeyCode2> keyDownPool = new List<KeyCode2>();
        private static List<KeyCode2> keyPressPool = new List<KeyCode2>();
        private static List<KeyCode2> keyUpPool = new List<KeyCode2>();

        private static Dictionary<int, Rocker> Rockers;
        private static Dictionary<int, Gyro> Gyros;
        private static Dictionary<int, Axis> Axises;

        public LTInput()
        {
            App.GamepadServer?.On(OnGamepadMessage);

            Rockers = new Dictionary<int, Rocker>()
            {
                { 1,new Rocker(1)},
                { 2,new Rocker(2)},
            };

            Gyros = new Dictionary<int, Gyro>()
            {
                { 1,new Gyro(1)},
                { 2,new Gyro(2)},
            };

            Axises = new Dictionary<int, Axis>()
            {
                { 1,new Axis(1)},
                { 2,new Axis(2)},
            };
        }

        public void Update()
        {
            //执行 1P 键值映射
            foreach (var item in Mapping.Table1P)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    keyDownPool.Add(item.Value);
                    keyPressPool.Add(item.Value);
                    singleKeydownPool.Add(item.Value);
                }

                if (Input.GetKeyUp(item.Key))
                {
                    keyUpPool.Add(item.Value);
                    singleKeyupPool.Add(item.Value);

                    if (keyPressPool.Contains(item.Value))
                        keyPressPool.Remove(item.Value);
                }
            }

            //执行 2P 键值映射
            foreach (var item in Mapping.Table2P)
            {
                if (Input.GetKeyDown(item.Key))
                {
                    keyDownPool.Add(item.Value);
                    keyPressPool.Add(item.Value);
                }

                if (Input.GetKeyUp(item.Key))
                {
                    keyUpPool.Add(item.Value);

                    if (keyPressPool.Contains(item.Value))
                        keyPressPool.Remove(item.Value);
                }
            }

            //多人键值统一映射到1P键值
            foreach (var item in Mapping.MultiplayerTable)
            {
                if (GetKeyDown(item.Key))
                {
                    if (!IsMultiplayer)
                    {
                        keyDownPool.Add(item.Value);
                        keyPressPool.Add(item.Value);
                    }

                    singleKeydownPool.Add(item.Value);
                }
                if (GetKeyUp(item.Key))
                {
                    if (!IsMultiplayer)
                    {
                        keyUpPool.Add(item.Value);

                        if (keyPressPool.Contains(item.Value))
                            keyPressPool.Remove(item.Value);
                    }

                    singleKeydownPool.Add(item.Value);
                }
            }

            //更新轴向逻辑
            foreach (var a in Axises.Values)
            {
                a.Update();
            }
        }

        public void FixedUpdate()
        {

        }

        public void LateUpdate()
        {
            keyDownPool.Clear();
            keyUpPool.Clear();

            singleKeydownPool.Clear();
            singleKeyupPool.Clear();
        }

        #region 静态公开方法

        /// <summary>
        /// 是否开启多人模式
        /// 非多人模式，所有按键映射为1P 键值
        /// </summary>
        public static bool IsMultiplayer { get; set; }

        /// <summary>
        /// 检测任意键
        /// </summary>
        /// <returns></returns>
        public static bool AnyKey()
        {
            return keyDownPool.Count > 0;
        }

        /// <summary>
        /// 检测按键按下，支持根据devID 映射其他键值。
        /// 受IsMultiplayer参数影响
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="devID"> 需要映射的devID </param>
        /// <returns></returns>
        public static bool GetKeyDown(KeyCode2 kc, int devID = 1)
        {
            if (devID > 1)
            {
                return keyDownPool.Contains(Mapping.DevIdTable[devID][kc]);
            }

            return keyDownPool.Contains(kc);
        }

        /// <summary>
        /// 检测按键长按，支持根据devID 映射其他键值。
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="devID"> 需要映射的devID </param>
        /// <returns></returns>
        public static bool GetKey(KeyCode2 kc, int devID = 1)
        {
            if (devID > 1)
            {
                return keyPressPool.Contains(Mapping.DevIdTable[devID][kc]);
            }

            return keyPressPool.Contains(kc);
        }

        /// <summary>
        /// 检测按键弹起，支持根据devID 映射其他键值。
        /// 受IsMultiplayer参数影响
        /// </summary>
        /// <param name="kc"> 键值 </param>
        /// <param name="devID"> 需要映射的devID </param>
        /// <returns></returns>
        public static bool GetKeyUp(KeyCode2 kc, int devID = 1)
        {
            if (devID > 1)
            {
                return keyUpPool.Contains(Mapping.DevIdTable[devID][kc]);
            }

            return keyUpPool.Contains(kc);
        }

        /// <summary>
        /// 检测按键按下，其他P键值统一反馈为1P的键值。
        /// 不受IsMultiplayer参数影响
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static bool GetKeyDownSingle(KeyCode2 kc)
        {
            return singleKeydownPool.Contains(kc);
        }

        /// <summary>
        /// 检测按键弹起，其他P键值统一反馈为1P的键值。
        /// 不受IsMultiplayer参数影响
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static bool GetKeyUpSingle(KeyCode2 kc)
        {
            return singleKeyupPool.Contains(kc);
        }

        /// <summary>
        /// 获取摇杆
        /// </summary>
        /// <param name="devID"> 1P = 1，2P = 2 </param>
        /// <returns></returns>
        public static Rocker Rocker(int devID = 1)
        {
            return Rockers[devID];
        }

        /// <summary>
        /// 获取陀螺仪
        /// </summary>
        /// <param name="devID">1P = 1，2P = 2 </param>
        /// <returns></returns>
        public static Gyro Gyro(int devID = 1)
        {
            return Gyros[devID];
        }

        /// <summary>
        /// 获取轴向信息
        /// </summary>
        /// <param name="devID">1P = 1，2P = 2</param>
        /// <returns></returns>
        public static Axis GetAxis(int devID = 1)
        {
            return Axises[devID];
        }

        #endregion

        /// <summary>
        /// 接收手柄消息
        /// </summary>
        /// <param name="msg"></param>
        private void OnGamepadMessage(IMessage msg)
        {
            switch (msg.GetMessageType())
            {
                case MessageType.Keyboard:
                    MessageKeyboard kb = msg as MessageKeyboard;

                    if (kb.State == KeyboardState.KeyDown)
                    {
                        keyDownPool.Add(kb.KeyCode);
                        keyPressPool.Add(kb.KeyCode);
                    }
                    else if (kb.State == KeyboardState.KeyUp)
                    {
                        keyUpPool.Add(kb.KeyCode);

                        if (keyPressPool.Contains(kb.KeyCode))
                            keyPressPool.Remove(kb.KeyCode);
                    }
                    break;

                case MessageType.Rocker:
                    MessageRocker r = msg as MessageRocker;

                    if (r.State == KeyboardState.KeyDown)
                    {
                        keyDownPool.Add(r.KeyCode);
                        keyPressPool.Add(r.KeyCode);
                    }
                    else if (r.State == KeyboardState.KeyUp)
                    {
                        keyUpPool.Add(r.KeyCode);

                        if (keyPressPool.Contains(r.KeyCode))
                            keyPressPool.Remove(r.KeyCode);
                    }

                    Rocker rocker;
                    if (Rockers.TryGetValue(r.DevID, out rocker))
                    {
                        rocker.X = r.Rx;
                        rocker.Y = r.Ry;
                    }
                    break;

                case MessageType.Gyro:
                    MessageGyro g = msg as MessageGyro;

                    Gyro gyro;
                    if (Gyros.TryGetValue(g.DevID, out gyro))
                    {
                        gyro.Gravity = g.Gravity;
                        gyro.Attitude = g.Attitude;
                        gyro.RotationRate = g.RotationRate;
                        gyro.UserAcceleration = g.UserAcceleration;
                    }
                    break;
            }
        }
    }
}
