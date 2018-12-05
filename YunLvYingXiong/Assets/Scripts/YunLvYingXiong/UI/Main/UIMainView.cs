//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainView : BaseView{
    UIMainViewCtrl _iCtrl;

    public override string PrefabPath()
    {
        return "UIPrefab/Main/UIMainView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UIMainViewCtrl)iCtrl;

        
    }
}
