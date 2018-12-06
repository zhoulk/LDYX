//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

public class UIBattleViewCtrl : BaseCtrl
{
    public override void Start(params object[] args)
    {
        this.view = ViewManager.Instance.CreateView(this, PanelNames.UIBattle, args);
    }

    public void Close()
    {
        CtrlManager.Instance.CloseCtrl(CtrlNames.UIBattle);
    }

}
