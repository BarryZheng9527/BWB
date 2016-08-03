using FairyGUI;
using FairyGUI.Utils;
using System.Collections;

public class BattleMessage : GComponent
{
    private GList _MessageList;
    private Queue _MessageQueue;

    public override void ConstructFromXML(XML xml)
    {
        base.ConstructFromXML(xml);
        _MessageList = GetChild("_MessageList").asList;
        _MessageQueue = new Queue();
        onAddedToStage.Add(AddedToStage);
        onRemovedFromStage.Add(RemovedFromStage);
        GameEventHandler.Messenger.AddEventListener(EventConstant.BattleMessage, OnBattleMessage);
    }

    private void AddedToStage()
    {
        UpdateBattleMessageList();
    }

    private void RemovedFromStage()
    {
    }

    /*
     * 收取战斗信息
     */
    private void OnBattleMessage(EventContext context)
    {
        string message = (string)context.data;
        if (_MessageQueue.Count == Constant.BATTLEMESSAGENUM)
        {
            _MessageQueue.Dequeue();
        }
        _MessageQueue.Enqueue(message);
        if (onStage)
        {
            UpdateBattleMessageList();
        }
    }

    /*
     * 更新战斗信息列表
     */
    private void UpdateBattleMessageList()
    {
        _MessageList.RemoveChildrenToPool();
        foreach (string message in _MessageQueue)
        {
            BattleMessageItem battleMessageItem = _MessageList.AddItemFromPool() as BattleMessageItem;
            battleMessageItem.SetMessage(message);
        }
    }
}