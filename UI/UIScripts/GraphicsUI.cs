using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class GraphicsUI : BaseUI
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
