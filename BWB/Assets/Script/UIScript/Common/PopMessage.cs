using FairyGUI;
using System.Collections.Generic;

class PopMessage : Window
{
    private GTextField _MessageText;

    private int _iCountDown;

    protected override void OnInit()
    {
        base.OnInit();
        contentPane = UIPackage.CreateObject("Common", "PopMessage").asCom;
        Center();
        modal = false;
        touchable = false;
    }

    protected override void OnShown()
    {
        base.OnShown();
        _iCountDown = 3;
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
        _MessageText.text = text;
        if (iPosX == 0 && iPosY == 0)
        {
            _MessageText.x = (Constant.WIDTH - _MessageText.width) / 2;
            _MessageText.y = (Constant.HEIGHT - _MessageText.height) / 2;
        }
        else
        {
            _MessageText.x = iPosX;
            _MessageText.y = iPosY;
        }
        if (isShowing)
        {
            _iCountDown = 3;
        }
        else
        {
            Show();
        }
    }
}