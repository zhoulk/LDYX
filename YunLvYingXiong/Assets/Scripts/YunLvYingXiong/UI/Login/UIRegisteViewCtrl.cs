//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRegisteViewCtrl : BaseCtrl
{
    public override void Start(params object[] args)
    {
        this.view = ViewManager.Instance.CreateView(this, PanelNames.UIRegiste, args);
    }

    public void ShowLoginView()
    {
        CtrlManager.Instance.OpenCtrl(CtrlNames.UILogin);
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIRegiste);
    }
}
