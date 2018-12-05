//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:38:35
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CtrlManager : Singleton<CtrlManager> {

    private Dictionary<string, BaseCtrl> ctrlDic = new Dictionary<string, BaseCtrl>();

    /// <summary>
    /// 打开 ctrl
    /// </summary>
    /// <param name="ctrlName"></param>
    /// <param name="args"></param>
    public void OpenCtrl(string ctrlName, params object[] args)
    {
        if (!ctrlDic.ContainsKey(ctrlName))
        {
            string file = Assembly.GetExecutingAssembly().GetName().Name;
            Assembly assembly = Assembly.Load(file);
            BaseCtrl ctrl = (BaseCtrl)assembly.CreateInstance(ctrlName);
            ctrl.ctrlName = ctrlName;
            ctrl.Start(args);

            ctrlDic.Add(ctrlName, ctrl);
        }
    }

    /// <summary>
    /// 关闭 ctrl
    /// </summary>
    /// <param name="ctrlName"></param>
    public void CloseCtrl(string ctrlName)
    {
        if (ctrlDic.ContainsKey(ctrlName))
        {
            BaseCtrl ctrl = ctrlDic[ctrlName];
            if (ctrl.view != null)
            {
                ViewManager.Instance.RemoveView(ctrl.view.viewName);
            }
            ctrl.OnDestroy();

            ctrlDic.Remove(ctrlName);
        }
    }
}
