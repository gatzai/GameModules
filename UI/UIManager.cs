using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{

    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        /// <summary>
        /// 缓存所有UI窗体
        /// </summary>
        Dictionary<string, BaseUI> cacheUIs
            = new Dictionary<string, BaseUI>();

        /// <summary>
        /// 当前显示的UI窗体
        /// </summary>
        Dictionary<string, BaseUI> showUIs
            = new Dictionary<string, BaseUI>();
        /// <summary>
        /// UI栈，有逻辑层级的UI
        /// </summary>
        Stack<BaseUI> staUIs
            = new Stack<BaseUI>();

        /// <summary>
        /// UI根节点
        /// </summary>
        Transform nodeRoot;

        Transform nodePopUp;
        Transform nodeFixed;
        Transform nodeNormal;
        Transform nodeUIScript;

        Dictionary<string, string> UIPath = new Dictionary<string, string>();

        //一些常量
        public const string PopUpMenuKey = "PopUpUI";

        //委托
        public delegate void PopConfirm();
        public PopConfirm YesConfirm;

        public void Awake()
        {
            instance = this;
            InitRootCanvasLoading();
            //获取根节点，弹出节点，固定节点，全屏节点
            nodeRoot = GameObject.Find("Canvas").transform;

            nodePopUp = nodeRoot.Find("PopUp");
            nodeFixed = nodeRoot.Find("Fixed");
            nodeNormal = nodeRoot.Find("Normal");

            UIPath?.Add("MainUI", "UIPrefabs/MainMenu");
            UIPath?.Add("StartUI", "UIPrefabs/StartMenu");
            UIPath?.Add("SettingUI", "UIPrefabs/SettingMenu");
            UIPath?.Add("OtherUI", "UIPrefabs/OtherMenu");
            UIPath?.Add("GraphicsUI", "UIPrefabs/GraphicsMenu");
            UIPath?.Add("SaveUI", "UIPrefabs/SaveMenu");
            UIPath?.Add("SoundUI", "UIPrefabs/SoundMenu");

            UIPath?.Add(PopUpMenuKey, "UIPrefabs/PopMenu");

            MyUITest();
        }


        public bool ShowUI(string UIName)
        {
            
            BaseUI baseUI = null;
            if (string.IsNullOrEmpty(UIName))
                return false;
            baseUI = LoadUIToCacheUIs(UIName);

            if (baseUI == null)
                return false;

            switch(baseUI.currentUIType.mode)
            {
                case UIFormShowMode.Normal: 
                    LoadUIToShowUIs(UIName);
                    break;
                case UIFormShowMode.Stack: 
                    PushUIToStack(UIName);
                    break;
                case UIFormShowMode.HideOther: 

                    break;
                default: return false;
            }
            return true;
        }
        

        public void CloseUI(string UIName)
        {
            BaseUI baseUI = null;
            if (string.IsNullOrEmpty(UIName))
            {
                Debug.Log("UI名字为空");
                return;
            }

            cacheUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
            {
                Debug.Log("UI加载错误");
                return;
            }

            switch (baseUI.currentUIType.mode)
            {
                case UIFormShowMode.Normal:
                    ExitUI(UIName);
                    break;
                case UIFormShowMode.Stack:
                    PopUIFromStack();
                    break;
                case UIFormShowMode.HideOther:
                    
                    break;
                default:
                    Debug.Log("未知UI类型");
                    break;
            }

        }

        public void CloseAll()
        {
            foreach(string name in showUIs.Keys)
            {
                CloseUI(name);
            }
            staUIs.Peek().ActiveFalse();
            showUIs.Clear();
            ClearStack();
        }



        #region 内部测试

        #endregion

        #region 私有方法

        void InitRootCanvasLoading()
        {
        }

        void MyUITest()
        {
            UIManager.instance.ShowUI("MainUI");
        }

        /// <summary>
        /// 加载窗体到缓存里
        /// </summary>
        /// <param name="UIName"></param>
        /// <returns></returns>
        BaseUI LoadUIToCacheUIs(string UIName)
        {
            BaseUI baseUI = null;
            cacheUIs.TryGetValue(UIName, out baseUI);
            if(baseUI == null)
            {
                baseUI = LoadUI(UIName);
            }
            return baseUI;
        }

        BaseUI LoadUI(string UIName)
        {
            Debug.Log("从资源处加载UI");
            string uiPath;
            GameObject UIPrefab = null;
            BaseUI baseUI = null;

            UIPath.TryGetValue(UIName, out uiPath);

            Debug.Log($"路径:{uiPath}");

            if (!string.IsNullOrEmpty(uiPath))
            {
                UIPrefab = ResourcesMgr.GetInstance().LoadAsset(uiPath, false);
            }

            if (nodeRoot != null && UIPrefab != null)
            {
                baseUI = UIPrefab.GetComponent<BaseUI>();
                if (baseUI == null)
                {
                    Debug.Log($"baseUI==null,请确认窗口是否有BaseUI脚本，参数:{UIName}");
                    return null;
                }

                switch (baseUI.currentUIType.type)
                {
                    case UIFormType.Normal:
                        UIPrefab.transform.SetParent(nodeNormal, false);
                        break;
                    case UIFormType.Fixed:
                        UIPrefab.transform.SetParent(nodeFixed, false);
                        break;
                    case UIFormType.PopUp:
                        UIPrefab.transform.SetParent(nodePopUp, false);
                        break;
                }

                UIPrefab.SetActive(false);
                cacheUIs.Add(UIName, baseUI);
                return baseUI;
            }
            else
            {
                Debug.Log($"traRoot!=null Or UIPrefab!=null Please Check  参数{UIName}");
            }

            Debug.Log($"出现不可预估的错误 参数{UIName}");
            return null;
        }

        void LoadUIToShowUIs(string UIName)
        {
            BaseUI baseUI;
            BaseUI baseUIFormCache;

            showUIs.TryGetValue(UIName, out baseUI);

            if (baseUI != null)
                return;

            cacheUIs.TryGetValue(UIName, out baseUIFormCache);

            if (baseUIFormCache != null)
            {
                showUIs.Add(UIName, baseUIFormCache);
                baseUIFormCache.ActiveTrue();
            }

        }

        void PushUIToStack(string UIName)
        {
            BaseUI baseUI;
            //判断栈中是否有其他的窗体，有则冻结
            if (staUIs.Count > 0)
            {
                BaseUI topUI = staUIs.Peek();
                topUI.Freeze();
            }
            //判断UI所有窗体 是否有指定的UI窗体 有就处理
            cacheUIs.TryGetValue(UIName, out baseUI);
            if (baseUI != null)
            {
                baseUI.ActiveTrue();
                //把指定UI窗体，入栈
                staUIs.Push(baseUI);
            }
            else
                Debug.Log($"baseUI==null,请确认窗口是否有BaseUI脚本，参数:{UIName}");
        }

        void PopUIFromStack()
        {
            BaseUI baseUI = staUIs.Pop();
            baseUI.ActiveFalse();
            if (staUIs.Count >= 1)
            {
                //下一个窗体重新显示
                staUIs.Peek().ActiveTrue();
            }
        }

        void ExitUI(string UIName)
        {
            BaseUI baseUI;
            //"正在显示集合"如果没有记录 则直接返回
            showUIs.TryGetValue(UIName, out baseUI);
            if (baseUI == null)
                return;
            //指定窗体，标记为隐藏状态,从正在显示集合中移除
            baseUI.ActiveFalse();
            showUIs.Remove(UIName);
        }



        bool ClearStack()
        {
            if (staUIs != null && staUIs.Count >= 1)
            {
                staUIs.Clear();
                return true;
            }
            return false;
        }

        #endregion

    }
}

