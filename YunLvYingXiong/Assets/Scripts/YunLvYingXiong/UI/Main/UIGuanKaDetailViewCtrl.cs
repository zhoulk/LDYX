//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

public class UIGuanKaDetailViewCtrl : BaseCtrl
{
    public override void Start(params object[] args)
    {
        this.view = ViewManager.Instance.CreateView(this, PanelNames.UIGuanKaDetail, args);
    }

    public void Close()
    {
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIGuanKaDetail);
        CtrlManager.Instance.OpenCtrl(CtrlNames.UIGuanKaMenu);
    }

    public void ShowBattleView(GuanKa guanKa)
    {
        CtrlManager.Instance.OpenCtrl(CtrlNames.UIBattle, guanKa);
    }
}
