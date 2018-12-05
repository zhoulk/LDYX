//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginView : BaseView{
    UILoginViewCtrl _iCtrl;

    Button regBtn;

    public override string PrefabPath()
    {
        return "UIPrefab/Login/UILoginView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UILoginViewCtrl)iCtrl;

        regBtn = this.transform.Find("center/registerBtn").GetComponent<Button>();
        regBtn.onClick.AddListener(OnRegBtnClick);
    }

    void OnRegBtnClick()
    {
        _iCtrl.ShowRegView();
    }
}
