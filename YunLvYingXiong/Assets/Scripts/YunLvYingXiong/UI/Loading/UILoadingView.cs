//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILoadingView : BaseView{
    UILoadingViewCtrl _iCtrl;

    Slider m_LoadingSlider;

    AsyncOperation m_Aso;

    float m_PreProgress;

    float m_MinLoadingSeconds = 2;

    public override string PrefabPath()
    {
        return "UIPrefab/Loading/UILoadingView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UILoadingViewCtrl)iCtrl;

        m_LoadingSlider = this.transform.Find("progress").GetComponent<Slider>();

        m_Aso = SceneManager.LoadSceneAsync(SceneNames.SceneMain);
        m_Aso.allowSceneActivation = false;
    }

    public override void OnUpdate() {
        m_MinLoadingSeconds -= Time.deltaTime;

        float progress = m_Aso.progress;

        if (Mathf.Approximately(progress, 0.9f))
        {
            progress = 1.0f;
        }

        if (m_LoadingSlider.value != progress)
        {
            m_LoadingSlider.value = Mathf.Lerp(m_LoadingSlider.value, progress, Time.deltaTime * 2);
            if (Mathf.Abs(m_LoadingSlider.value - progress) < 0.01f)
            {
                m_LoadingSlider.value = progress;
            }
        }

        if (Mathf.Approximately(progress,1.0f) && m_MinLoadingSeconds <= 0)
        {
            m_Aso.allowSceneActivation = true;
            _iCtrl.Close();
        }
    }
}
