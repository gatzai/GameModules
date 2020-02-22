using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    public class UnityHelper
    {
        /// <summary>
        /// 递归查找子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform Find(Transform parent, string childName)
        {
            Transform child = null;

            child = parent.Find(childName);
            if(child == null)
            {
                foreach (Transform node in parent)
                {
                    child = Find(node, childName);
                    if (child != null) return child;
                }
            }
            return child;
        }
    }

}
