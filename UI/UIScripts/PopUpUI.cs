using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class PopUpUI : BaseUI
{
    private void Awake()
    {
        currentUIType.type = UIFormType.PopUp;
        currentUIType.mode = UIFormShowMode.Stack;

        RegisterBtnOnClick("Back", go =>
        {
            CloseUI();
        });

        RegisterBtnOnClick("Yes", go =>
        {
            ConfirmYesPop();
        });
    }
}
