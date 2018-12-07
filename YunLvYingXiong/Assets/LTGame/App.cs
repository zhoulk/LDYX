/*-------------------------------------------------------------------------------
 * 创建者：huangyechuan
 * 修改者列表：
 * 创建日期：2018/11/28
 * 模块描述：框架外观
 * 
 * TODO ,增加Provider设计，后续所有模块依赖继承IProvider接口启动
 * 
 * 1.该框架基于.net4.6 以上,Unity 请在路径 File/Build Settings/Player Setting/Other Setting/Configuration/Script Runtime Version 修改.net版本
 * 2.请在Edit/Project Setting/Script Execution Order 设置App脚本的执行顺序为-999，保证执行顺序第一
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using LTGame;
using LTGame.SDK;
using System;

public class App : MonoBehaviour
{
    /// <summary>
    /// 不可更改，Sdk的回调指定名称
    /// </summary>
    private const string AppName = "App";

    /// <summary>
    /// 是否真正销毁
    /// </summary>
    private bool realDestroy;

    /// <summary>
    /// 保证游戏场景内实例唯一
    /// </summary>
    private static App Instance;

    /// <summary>
    /// 事件调度器,主线程优化，支持多线程
    /// 支持unity场景中 未激活物体不分发
    /// </summary>
    private static Dispatcher Dispatcher;

    private static IInput Input;

    private void Start()
    {
        if (Instance != null)
        {
            realDestroy = false;
            DestroyImmediate(gameObject);
            return;
        }

        this.realDestroy = true;
        this.name = AppName;
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Dispatcher = new Dispatcher();
        SdkTool = new SdkTool();
        DataColloct = new DataColloct();
        GamepadServer = new GamepadServer();
        Input = new LTInput();

        gameObject.AddComponent<AndroidAPIBridge>();

    }

    private void Update()
    {
        Dispatcher?.Update();
        GamepadServer?.Update();
        Input?.Update();
    }

    private void LateUpdate()
    {
        Input?.LateUpdate();
    }

    #region 公开属性

    /// <summary>
    /// 数据采集
    /// </summary>
    public static IDataColloct DataColloct { get; set; }

    /// <summary>
    /// Sdk 工具集，提供支付，二维码扫描下载等功能
    /// </summary>
    public static ISdkTool SdkTool { get; set; }

    /// <summary>
    /// 提供虚拟手柄服务
    /// </summary>
    public static IGamepadServer GamepadServer { get; set; }

    #endregion

    #region 事件注册/触发方法

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="args">参数表</param>
    public static void Trigger(string eventName, params object[] args)
    {
        Dispatcher.Trigger(eventName, args);
    }

    public static IEvent On(string eventName, Action action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    public static IEvent On<T0>(string eventName, Action<T0> action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    public static IEvent On<T0, T1>(string eventName, Action<T0, T1> action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    public static IEvent On<T0, T1, T2>(string eventName, Action<T0, T1, T2> action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    public static IEvent On<T0, T1, T2, T3>(string eventName, Action<T0, T1, T2, T3> action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    public static IEvent On<T0, T1, T2, T3, T4>(string eventName, Action<T0, T1, T2, T3, T4> action, object group = null)
    {
        return Dispatcher.On(eventName, action, group);
    }

    /// <summary>
    /// 解除注册的事件监听器
    /// </summary>
    /// <param name="target">
    /// 事件解除目标
    /// <para>如果传入的是字符串(<code>string</code>)将会解除对应事件名的所有事件</para>
    /// <para>如果传入的是事件对象(<code>IEvent</code>)那么解除对应事件</para>
    /// <para>如果传入的是分组(<code>object</code>)会解除该分组下的所有事件</para>
    /// </param>
    public static void Off(object target)
    {
        Dispatcher.Off(target);
    }

    #endregion

    private void OnDestroy()
    {
        //此处注意对静态对象的销毁，App切换场景时会检测同名对象，并销毁
        if (realDestroy)
        {
            GamepadServer?.Dispose();
        }

        realDestroy = true;
    }
}
