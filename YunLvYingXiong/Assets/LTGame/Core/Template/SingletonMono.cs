using UnityEngine;

namespace LTGame
{
    /// <summary>
    /// 单例模板
    /// </summary>
    public abstract class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T mInstance = null;

        public static T Instance
        {
            get
            {
                return mInstance;
            }
            set
            {
                mInstance = value;
            }
        }

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
                mInstance.InitSingletonMono();
            }
        }

        public virtual void InitSingletonMono() { }
    }
}

