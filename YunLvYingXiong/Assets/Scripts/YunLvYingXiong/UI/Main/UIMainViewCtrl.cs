//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainViewCtrl : BaseCtrl
{
    public override void Start(params object[] args)
    {
        this.view = ViewManager.Instance.CreateView(this, PanelNames.UIMain, args);
    }

    public void ShowGuanKaMenu()
    {
        CtrlManager.Instance.OpenCtrl(CtrlNames.UIGuanKaMenu);
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIMain);
    }
}
