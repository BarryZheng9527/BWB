using FairyGUI;
using FairyGUI.Utils;

public class Attr : GComponent
{
    private GTextField _AttrName;
    private GTextField _AttrValue;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _AttrName = GetChild("_AttrName").asTextField;
        _AttrValue = GetChild("_AttrValue").asTextField;
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
    }

    public void SetNameAndValue(string name, string value)
    {
        _AttrName.text = name;
        _AttrValue.text = value;
    }

    private void AddedToStage()
    {
    }

    private void RemovedFromStage()
    {
    }
}