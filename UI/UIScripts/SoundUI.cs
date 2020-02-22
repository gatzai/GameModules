using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class SoundUI : BaseUI
{
    private void Awake()
    {
        currentUIType.type = UIFormType.Normal;
        currentUIType.mode = UIFormShowMode.Stack;

        RegisterSlider("MainSoundSlider", go =>
        {
            Debug.Log("Drag Update");
        });

        RegisterBtnOnClick("Back", go =>
        {
            CloseUI();
        });
    }
}
