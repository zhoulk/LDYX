//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

public class UIGuanKaMenuViewCtrl : BaseCtrl
{
    public override void Start(params object[] args)
    {
        this.view = ViewManager.Instance.CreateView(this, PanelNames.UIGuanKaMenu, args);
    }

    public override void Show(params object[] args)
    {
        this.view.OnResume(args);
    }

    public void Close()
    {
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIGuanKaMenu);
        CtrlManager.Instance.OpenCtrl(CtrlNames.UIMain);
    }

    public void ShowGuanKaDetail(GuanKa guanKa)
    {
        CtrlManager.Instance.OpenCtrl(CtrlNames.UIGuanKaDetail, guanKa);
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIGuanKaMenu);
    }
}
