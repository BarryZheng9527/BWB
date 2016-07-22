using FairyGUI;
using System.Collections.Generic;

class PopMessage : Window
{
    private GTextField _MessageText;

    private int _iCountDown;
    private string _Message;
    private int _iPosX;
    private int _iPosY;

    protected override void OnInit()
    {
        base.OnInit();
        contentPane = UIPackage.CreateObject("Common", "PopMessage").asCom;
        Center();
        modal = false;
        touchable = false;
        _MessageText = contentPane.GetChild("_MessageText").asTextField;
    }

    protected override void OnShown()
    {
        base.OnShown();
        UpdateShow();
        _iCountDown = 2;
        Timers.inst.Add(1, 0, CountDown);
    }

    protected override void OnHide()
    {
        base.OnHide();
        Timers.inst.Remove(CountDown);
    }

    private void CountDown(object param)
    {
        _iCountDown--;
        if (_iCountDown <= 0)
        {
            Hide();
        }
    }

    public void setText(string text, int iPosX = 0, int iPosY = 0)
    {
        _Message = text;
        _iPosX = iPosX;
        _iPosY = iPosY;
        if (isShowing)
        {
            UpdateShow();
            _iCountDown = 2;
        }
        else
        {
            Show();
        }
    }

    public void UpdateShow()
    {
        _MessageText.text = _Message;
        if (_iPosX == 0 && _iPosY == 0)
        {
            _MessageText.x = (Constant.WIDTH - _MessageText.width) / 2;
            _MessageText.y = (Constant.HEIGHT - _MessageText.height) / 2;
        }
        else
        {
            _MessageText.x = _iPosX;
            _MessageText.y = _iPosY;
        }
    }
}