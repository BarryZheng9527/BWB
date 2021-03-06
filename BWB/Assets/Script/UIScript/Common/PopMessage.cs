﻿using FairyGUI;
using System.Collections.Generic;
using UnityEngine;

class PopMessage : Window
{
    private GTextField _MessageText;

    private float _iCountDown;
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
        _iCountDown = 1f;
        Timers.inst.Add(0.5f, 0, CountDown);
    }

    protected override void OnHide()
    {
        base.OnHide();
        Timers.inst.Remove(CountDown);
    }

    private void CountDown(object param)
    {
        _iCountDown -= 0.5f;
        if (_iCountDown <= 0)
        {
            Hide();
        }
    }

    /*
     * 设置弹出框文字
     */
    public void setText(string text, int iPosX = 0, int iPosY = 0)
    {
        _Message = text;
        _iPosX = iPosX;
        _iPosY = iPosY;
        if (isShowing)
        {
            UpdateShow();
            _iCountDown = 1f;
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
        _MessageText.TweenMove(new Vector2(_MessageText.x, _MessageText.y - 120), 0.8f);
    }
}