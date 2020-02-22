using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class SettingUI : BaseUI
{
    private void Awake()
    {
        currentUIType.type = UIFormType.Normal;
        currentUIType.mode = UIFormShowMode.Stack;
        RegisterBtnOnClick("Back", go =>
        {
            CloseUI();
        });

        RegisterBtnOnClick("Sound", go =>
        {
            OpenUI("SoundUI");
        });

        RegisterBtnOnClick("Graphics", go =>
        {
            OpenUI("GraphicsUI");
        });
    }
}
