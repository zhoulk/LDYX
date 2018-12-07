/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：轴向信息类
 * 
 * ------------------------------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

namespace LTGame
{
    public class Axis : ADevice
    {
        private float vTarget;
        private float hTarget;

        private float vPercent;
        private float hPercent;

        private int vFactor;
        private int hFactor;

        private Dictionary<KeyCode2, int> kc2Value;
        private Dictionary<KeyCode2, int> targetArray;

        public Axis(byte devID)
        {
            this.DevID = devID;

            kc2Value = new Dictionary<KeyCode2, int>()
            {
                { KeyCode2.Up,1},
                { KeyCode2.Down,-1},
                { KeyCode2.Left,-1},
                { KeyCode2.Right,1},
            };

            targetArray = new Dictionary<KeyCode2, int>()
            {
                { KeyCode2.Up,0},
                { KeyCode2.Down,0},
                { KeyCode2.Left,0},
                { KeyCode2.Right,0},
            };
        }

        public override void Update()
        {
            //更新按键状态
            foreach (var k in kc2Value)
            {
                if (LTInput.GetKeyDown(k.Key, DevID))
                {
                    UpdateKeyDown(k.Key, k.Value);
                }

                if (LTInput.GetKeyUp(k.Key, DevID))
                {
                    UpdateKeyUp(k.Key, 0);
                }
            }

            //更新插值逻辑
            UpdateLerp();
        }

        private void UpdateKeyDown(KeyCode2 kc, int value)
        {
            targetArray[kc] = value;

            if (kc == KeyCode2.Left || kc == KeyCode2.Right)
            {
                HorizontalRaw = targetArray[kc];

                //目标不同，则进度清0
                if (hTarget != targetArray[kc])
                    hPercent = 0f;

                hTarget = targetArray[kc];
                hFactor = 1;
            }
            else if (kc == KeyCode2.Up || kc == KeyCode2.Down)
            {
                VerticalRaw = targetArray[kc];

                //目标不同，则进度清0
                if (vTarget != targetArray[kc])
                    vPercent = 0f;

                vTarget = targetArray[kc];
                vFactor = 1;
            }
        }

        private void UpdateKeyUp(KeyCode2 kc, int value)
        {
            targetArray[kc] = value;

            if (kc == KeyCode2.Left)
            {
                HorizontalRaw = targetArray[KeyCode2.Right];

                if (targetArray[KeyCode2.Right] == 0)
                {
                    hFactor = -1;
                }
                else
                {
                    hTarget = targetArray[KeyCode2.Right];
                    hPercent = 0f;
                }
            }
            else if (kc == KeyCode2.Right)
            {
                HorizontalRaw = targetArray[KeyCode2.Left];

                if (targetArray[KeyCode2.Left] == 0)
                {
                    hFactor = -1;
                }
                else
                {
                    hTarget = targetArray[KeyCode2.Left];
                    hPercent = 0f;
                }
            }
            else if (kc == KeyCode2.Up)
            {
                VerticalRaw = targetArray[KeyCode2.Down];

                if (targetArray[KeyCode2.Down] == 0)
                {
                    vFactor = -1;
                }
                else
                {
                    vTarget = targetArray[KeyCode2.Down];
                    vPercent = 0f;
                }
            }
            else if (kc == KeyCode2.Down)
            {
                VerticalRaw = targetArray[KeyCode2.Up];

                if (targetArray[KeyCode2.Up] == 0)
                {
                    vFactor = -1;
                }
                else
                {
                    vTarget = targetArray[KeyCode2.Up];
                    vPercent = 0f;
                }
            }
        }

        private void UpdateLerp()
        {
            Vertical = Mathf.Lerp(0, vTarget, vPercent);
            Horizontal = Mathf.Lerp(0, hTarget, hPercent);

            vPercent += Time.deltaTime * vFactor * 3f;
            hPercent += Time.deltaTime * hFactor * 3f;

            vPercent = Mathf.Clamp(vPercent, 0f, 1f);
            hPercent = Mathf.Clamp(hPercent, 0f, 1f);
        }

        /// <summary>
        /// 获得垂直方向的 输入 -1~1
        /// </summary>
        public float Vertical { get; set; }

        /// <summary>
        /// 获得水平方向的 输入 -1~1
        /// </summary>
        public float Horizontal { get; set; }

        /// <summary>
        /// 获取垂直方向输入，只有-1，0，1三种状态
        /// </summary>
        public int VerticalRaw { get; set; }

        /// <summary>
        /// 获取水平方向输入，只有-1，0，1三种状态
        /// </summary>
        public int HorizontalRaw { get; set; }
    }
}
