using System;
using System.Collections;

public class MessageQueueManager
{
    static private MessageQueueManager instance;

    MessageQueueManager()
    {
    }

    public static MessageQueueManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MessageQueueManager();
            }
            return instance;
        }
    }

    private Queue _MessageQueue = new Queue();

    public void AddMessage(object message)
    {
        _MessageQueue.Enqueue(message);
    }

    public void MessageHandler()
    {
        if (_MessageQueue.Count > 0)
        {
            object message = _MessageQueue.Dequeue();
            MessageDispose(message);
        }
    }

    public void MessageDispose(object message)
    {
        BaseResponse stBaseResponse = (BaseResponse)message;
        switch (stBaseResponse.ID)
        {
            case MessageConstant.CHECK_REGISTER_RESPONSE:
                CheckUserResponse stCheckRegisterResponse = (CheckUserResponse)stBaseResponse;
                NetManager.Instance.RegisterRequest(stCheckRegisterResponse);
                break;
            case MessageConstant.REGISTER_RESPONSE:
                RegisterResponse stRegisterResponse = (RegisterResponse)stBaseResponse;
                NetManager.Instance.RegisterResponse(stRegisterResponse);
                break;
            case MessageConstant.CHECK_LOGIN_RESPONSE:
                CheckUserResponse stCheckLoginResponse = (CheckUserResponse)stBaseResponse;
                NetManager.Instance.LoginRequest(stCheckLoginResponse);
                break;
            case MessageConstant.LOGIN_RESPONSE:
                LoginResponse stLoginResponse = (LoginResponse)stBaseResponse;
                NetManager.Instance.LoginResponse(stLoginResponse);
                break;
            case MessageConstant.CREAT_ROLE_RESPONSE:
                CreatRoleResponse stCreatRoleResponse = (CreatRoleResponse)stBaseResponse;
                NetManager.Instance.CreatRoleResponse(stCreatRoleResponse);
                break;
            case MessageConstant.START_GAME_RESPONSE:
                NetManager.Instance.StartGameResponse();
                break;
            default:
                break;
        }
    }
}