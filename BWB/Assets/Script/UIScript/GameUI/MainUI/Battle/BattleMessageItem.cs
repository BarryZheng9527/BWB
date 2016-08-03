using FairyGUI;
using FairyGUI.Utils;

public class BattleMessageItem : GComponent
{
    private GTextField _MessageText;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _MessageText = GetChild("_MessageText").asTextField;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    public void SetMessage(string message)
    {
        _MessageText.text = message;
    }

    private void AddedToStage()
    {
    }

    private void RemovedFromStage()
    {
    }
}