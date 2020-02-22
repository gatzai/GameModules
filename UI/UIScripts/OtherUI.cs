using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class OtherUI : BaseUI
{
    private void Awake()
    {
        currentUIType.type = UIFormType.Normal;
        currentUIType.mode = UIFormShowMode.Stack;


        RegisterBtnOnClick("Back", go =>
        {
            CloseUI();
        });
    }
}
