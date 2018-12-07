using UnityEngine;

namespace LTGame
{
    /// <summary>
    /// 普通单例模板,需赋值
    /// </summary>
    public abstract class Singleton<T> : MonoBehaviour
    {
        static protected T mInstance = default(T);

        static public T Instance
        {
            get
            {
                return mInstance;
            }
        }
    }
}
