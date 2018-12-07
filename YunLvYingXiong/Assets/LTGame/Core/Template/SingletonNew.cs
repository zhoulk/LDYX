using UnityEngine;
using System.Collections;

namespace LTGame
{
    /// <summary>
    /// 单例模板
    /// </summary>
    public abstract class SingletonNew<T> where T : new()
    {
        static protected T mInstance = default(T);
        static public T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new T();
                }
                return mInstance;
            }
            set
            {
                mInstance = value;
            }
        }
    }
}
