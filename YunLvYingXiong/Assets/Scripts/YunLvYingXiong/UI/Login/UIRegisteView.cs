//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRegisteView : BaseView{

    UIRegisteViewCtrl _iCtrl;
    Button loginBtn;

    public override string PrefabPath()
    {
        return "UIPrefab/Login/UIRegisteView";
    }

    public override void StartView(params object[] args) {

        _iCtrl = (UIRegisteViewCtrl)iCtrl;

        loginBtn = this.transform.Find("center/loginBtn").GetComponent<Button>();
        loginBtn.onClick.AddListener(OnLoginBtnClick);
    }

    void OnLoginBtnClick()
    {
        _iCtrl.ShowLoginView();
    }
}
