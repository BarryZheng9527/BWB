using FairyGUI;

class GameEventHandler : EventDispatcher
{
    protected static EventDispatcher messenger;

    public static EventDispatcher Messenger
    {
        get
        {
            if (messenger == null)
            {
                messenger = new EventDispatcher();
            }
            return messenger;
        }
    }
}