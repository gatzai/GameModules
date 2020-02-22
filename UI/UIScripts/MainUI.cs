using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class MainUI : BaseUI
{
    private void Awake()
    {
        currentUIType.type = UIFormType.Normal;
        currentUIType.mode = UIFormShowMode.Stack;

        RegisterBtnOnClick("Begin", go =>
        {
            OpenUI("StartUI");
        });

        RegisterBtnOnClick("Setting", go =>
        {
            OpenUI("SettingUI");
        });

        RegisterBtnOnClick("Other", go =>
        {
            OpenUI("OtherUI");
        });

        RegisterBtnOnClick("Exit", go =>
        {

        });
    }
}
