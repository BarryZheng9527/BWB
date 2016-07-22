using FairyGUI;
using FairyGUI.Utils;

public class Skill : GComponent
{
    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    private void AddedToStage()
    {
    }

    private void RemovedFromStage()
    {
    }
}