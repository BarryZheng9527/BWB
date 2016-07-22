using UnityEngine;
using FairyGUI;
using System.Collections;

public class Login : Window
{
    private GTextField _Account;
    private GTextField _PassWord;
    private GButton _RememberBtn;
    private GButton _ChangeBtn;
    private GButton _LoginBtn;

    protected override void OnInit()
    {
        base.OnInit();
        contentPane = UIPackage.CreateObject("Login", "Login").asCom;
        Center();
        modal = true;
        _Account = contentPane.GetChild("_Account").asTextField;
        _PassWord = contentPane.GetChild("_PassWord").asTextField;
        _RememberBtn = contentPane.GetChild("_RememberBtn").asButton;
        _ChangeBtn = contentPane.GetChild("_ChangeBtn").asButton;
        _ChangeBtn.onClick.Add(OnChangeAccount);
        _LoginBtn = contentPane.GetChild("_LoginBtn").asButton;
        _LoginBtn.onClick.Add(OnLogin);
    }

    protected override void OnShown()
    {
        base.OnShown();
        GameEventHandler.Messenger.AddEventListener(EventConstant.Login, OnLoginResponse);
        InitNameAndPassWord();
    }

    protected override void OnHide()
    {
        base.OnHide();
        GameEventHandler.Messenger.RemoveEventListener(EventConstant.Login, OnLoginResponse);
    }

    private void InitNameAndPassWord()
    {
        string name = PlayerPrefs.GetString("LastName");
        string password = PlayerPrefs.GetString("LastPassWord");
        _Account.text = name;
        _PassWord.text = password;
    }

    private void OnChangeAccount()
    {
        if (0 == contentPane.GetController("change").selectedIndex)
        {
            contentPane.GetController("change").SetSelectedIndex(1);
        }
        else if (1 == contentPane.GetController("change").selectedIndex)
        {
            contentPane.GetController("change").SetSelectedIndex(0);
        }
    }

    private void OnLogin()
    {
        NetManager.Instance.LoginRequest(_Account.text, _PassWord.text);
    }

    public void OnLoginResponse()
    {
        if (_RememberBtn.selected)
        {
            PlayerPrefs.SetString("LastName", _Account.text);
            PlayerPrefs.SetString("LastPassWord", _PassWord.text);
        }
        else
        {
            PlayerPrefs.SetString("LastName", "");
            PlayerPrefs.SetString("LastPassWord", "");
        }
        Hide();
        GUIManager.Instance.OpenCreatRole();
    }
}