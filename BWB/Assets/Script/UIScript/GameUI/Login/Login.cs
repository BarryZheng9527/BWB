using UnityEngine;
using FairyGUI;
using System.Collections;

public class Login : Window
{
    private GTextField _Account;
    private GTextField _Servicer;
    private GTextField _LoginAccount;
    private GTextField _LoginPassWord;
    private GTextField _RegisterAccount;
    private GTextField _RegisterPassWord;
    private GTextField _RegisterPassWord0;

    private GButton _ChangeBtn;
    private GButton _EnterGameBtn;
    private GButton _RememberBtn;
    private GButton _LoginBtn;
    private GButton _RegisterBtn;
    private GButton _OkBtn;
    private GButton _CancelBtn;

    private string _szName;
    private string _szPassWord;

    protected override void OnInit()
    {
        base.OnInit();
        contentPane = UIPackage.CreateObject("Login", "Login").asCom;
        Center();
        modal = true;
        _Account = contentPane.GetChild("_Account").asTextField;
        _Servicer = contentPane.GetChild("_Servicer").asTextField;
        _LoginAccount = contentPane.GetChild("_LoginAccount").asTextField;
        _LoginPassWord = contentPane.GetChild("_LoginPassWord").asTextField;
        _RegisterAccount = contentPane.GetChild("_RegisterAccount").asTextField;
        _RegisterPassWord = contentPane.GetChild("_RegisterPassWord").asTextField;
        _RegisterPassWord0 = contentPane.GetChild("_RegisterPassWord0").asTextField;
        _ChangeBtn = contentPane.GetChild("_ChangeBtn").asButton;
        _ChangeBtn.onClick.Add(OnChangeAccount);
        _EnterGameBtn = contentPane.GetChild("_EnterGameBtn").asButton;
        _EnterGameBtn.onClick.Add(OnEnterGame);
        _RememberBtn = contentPane.GetChild("_RememberBtn").asButton;
        _LoginBtn = contentPane.GetChild("_LoginBtn").asButton;
        _LoginBtn.onClick.Add(OnLogin);
        _RegisterBtn = contentPane.GetChild("_RegisterBtn").asButton;
        _RegisterBtn.onClick.Add(OnRegister);
        _OkBtn = contentPane.GetChild("_OkBtn").asButton;
        _OkBtn.onClick.Add(OnRegisterOk);
        _CancelBtn = contentPane.GetChild("_CancelBtn").asButton;
        _CancelBtn.onClick.Add(OnRegisterCancel);
    }

    protected override void OnShown()
    {
        base.OnShown();
        GameEventHandler.Messenger.AddEventListener(EventConstant.Register, OnRegisterResponse);
        GameEventHandler.Messenger.AddEventListener(EventConstant.Login, OnLoginResponse);
        InitLoginStatus();
    }

    protected override void OnHide()
    {
        base.OnHide();
        GameEventHandler.Messenger.AddEventListener(EventConstant.Register, OnRegisterResponse);
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Login, OnLoginResponse);
    }

    /*
     * 初始状态
     */
    private void InitLoginStatus()
    {
        _szName = PlayerPrefs.GetString("LastName");
        _szPassWord = PlayerPrefs.GetString("LastPassWord");
        if (_szName != "")
        {
            NetManager.Instance.CheckLoginRequest(_szName, _szPassWord);
        }
        else
        {
            contentPane.GetController("change").SetSelectedIndex(1);
        }
    }

    /*
     * 切换账号
     */
    private void OnChangeAccount()
    {
        contentPane.GetController("change").SetSelectedIndex(1);
        _LoginAccount.text = _Account.text;
        _LoginPassWord.text = "";
        _RememberBtn.selected = true;
    }

    /*
     * 进入游戏
     */
    private void OnEnterGame()
    {
        Hide();
        GUIManager.Instance.OpenCreatRole();
    }

    /*
     * 登陆
     */
    private void OnLogin()
    {
        string szName = _LoginAccount.text;
        string szPassWord = _LoginPassWord.text;
        if (szName == "" || szPassWord == "")
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(ErrorConstant.ERROR_100003));
        }
        else
        {
            NetManager.Instance.CheckLoginRequest(szName, szPassWord);
        }
    }

    /*
     * 登陆结果
     */
    public void OnLoginResponse(EventContext context)
    {
        LoginResponse response = context.data as LoginResponse;
        if (response.iResponseId == 0)
        {
            if (_RememberBtn.selected)
            {
                PlayerPrefs.SetString("LastName", response.name);
                PlayerPrefs.SetString("LastPassWord", response.password);
            }
            else
            {
                PlayerPrefs.SetString("LastName", "");
                PlayerPrefs.SetString("LastPassWord", "");
            }
            contentPane.GetController("change").SetSelectedIndex(0);
            _Account.text = response.name;
        }
        else
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(response.iResponseId));
            contentPane.GetController("change").SetSelectedIndex(1);
            _LoginAccount.text = "";
            _LoginPassWord.text = "";
        }
    }

    /*
     * 进入注册页面
     */
    private void OnRegister()
    {
        contentPane.GetController("change").SetSelectedIndex(2);
        _RegisterAccount.text = "";
        _RegisterPassWord.text = "";
        _RegisterPassWord0.text = "";
    }

    /*
     * 注册
     */
    private void OnRegisterOk()
    {
        string szName = _RegisterAccount.text;
        string szPassWord = _RegisterPassWord.text;
        string szPassWord0 = _RegisterPassWord0.text;
        if (szName == "" || szPassWord == "" || szPassWord0 == "")
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(ErrorConstant.ERROR_100003));
        }
        else if (szPassWord != szPassWord0)
        {
            GUIManager.Instance.OpenPopMessage(LanguageConfig.Instance.GetErrorText(ErrorConstant.ERROR_100004));
        }
        else
        {
            NetManager.Instance.CheckRegisterRequest(szName, szPassWord);
        }
    }

    /*
     * 注册结果
     */
    public void OnRegisterResponse(EventContext context)
    {
        RegisterResponse response = context.data as RegisterResponse;
        NetManager.Instance.CheckLoginRequest(response.name, response.password);
    }

    /*
     * 离开注册
     */
    private void OnRegisterCancel()
    {
        contentPane.GetController("change").SetSelectedIndex(1);
        _LoginAccount.text = "";
        _LoginPassWord.text = "";
    }
}