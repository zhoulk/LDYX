using UnityEngine;
using System.Collections;

namespace LTGame
{
    /// <summary>
    /// 单例模板
    /// </summary>
    public abstract class SingletonMonoManager<T> : MonoBehaviour where T : SingletonMonoManager<T>
    {
        static private T mInstance = null;
        static public T Instance
        {
            get
            {
                return mInstance;
            }
        }

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this as T;
                mInstance.InitSingletonMono();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void InitSingletonMono() { }
    }
}


