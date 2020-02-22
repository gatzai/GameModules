using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFrameWork
{
    public class UIType
    {
        public bool isClearStack = false;
        public UIFormType type = UIFormType.Normal;
        public UIFormShowMode mode = UIFormShowMode.Normal;

    }

    public class BaseUI : MonoBehaviour
    {
        public UIType currentUIType { get; set; } = new UIType();

        public virtual void ActiveTrue()
        {
            gameObject.SetActive(true);
        }

        public virtual void ActiveFalse()
        {
            gameObject.SetActive(false);
        }

        public virtual void Freeze()
        {
            gameObject.SetActive(false);
        }

        #region 封装子类常用方法
        protected void OpenUI(string UIName)
        {
            UIManager.instance.showUI(UIName);
        }

        protected void CloseUI()
        {
            string UIName;
            UIName = GetType().ToString();
            Debug.Log($"GetTypeName: {UIName}");
            UIManager.instance.CloseUI(UIName);
        }

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="del"></param>
        protected void RegisterBtnOnClick(string btnName, EventTriggerListener.VoidDelegate del)
        {
            Transform btn = UnityHelper.Find(gameObject.transform, btnName);
            EventTriggerListener.Get(btn?.gameObject).onClick = del;
        }
        protected void RegisterBtnOnClick(Transform btn, EventTriggerListener.VoidDelegate del)
        {
            EventTriggerListener.Get(btn?.gameObject).onClick = del;
        }

        protected void SendMsg(string msgType, string msgName, object msgContent)
        {
            KeyValuesUpdate kv = new KeyValuesUpdate(msgName, msgContent);
            MessageCenter.SendMessage(msgType, kv);
        }

        protected void ReceiveMsg(string msgType, MessageCenter.DelMessageDelivery hander)
        {
            MessageCenter.AddMsgListener(msgType, hander);
        }

        /// <summary>
        /// 注册滑动条
        /// </summary>
        /// <param name="sliderName"></param>
        /// <param name="del"></param>
        protected void RegisterSlider(string sliderName, EventTriggerListener.VoidDelegate del)
        {
            Transform slider = UnityHelper.Find(gameObject.transform, sliderName);
            EventTriggerListener.Get(slider?.gameObject).onUp = del;


        }
        #endregion
    }

}