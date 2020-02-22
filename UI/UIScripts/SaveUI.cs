using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class SaveUI : BaseUI
{
    
    private void Awake()
    {
        currentUIType.type = UIFormType.Normal;
        currentUIType.mode = UIFormShowMode.Stack;

        RegisterBtnOnClick("Load1", go =>
         {
             Debug.Log("存档1号位");
         });

        RegisterBtnOnClick("Load2", go =>
        {
            Debug.Log("存档2号位");
        });

        RegisterBtnOnClick("Load3", go =>
        {
            Debug.Log("存档3号位");
        });

        RegisterBtnOnClick("Back", go =>
        {
            CloseUI();

        });
    }
}
