using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua;


/*----------------------------------------------------------------
* 项目名称 ：Assets.Dev.Scripts.Utils
* 项目描述 ：
* 类 名 称 ：LuaHelperUtil
* 类 描 述 ：
* 所在的域 ：CHENGHAIXIAO-PC
* 命名空间 ：Assets.Dev.Scripts.Utils
* 机器名称 ：CHENGHAIXIAO-PC 
* CLR 版本 ：4.0.30319.42000
* 作    者 ：ASUS
* 创建时间 ：2018/5/8 17:16:25
* 更新时间 ：2018/5/8 17:16:25
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ ASUS 2018. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/

namespace Game
{
    public class LuaHelperUtil
    {
        /// <summary>
        /// lua 调用 设置物件的层级
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_layer"></param>
        public static void SetGameObjectAndChildLayer(GameObject _go, string _layer)
        {
            Transform[] trans = _go.GetComponentsInChildren<Transform>();
            for (int i = 0; i < trans.Length; i++)
            {
                trans[i].gameObject.layer = LayerMask.NameToLayer(_layer);
            }
        }

        public static void AddComponent(GameObject _go, string _className)
        {
            Type t = Type.GetType(_className);

            var cpt = _go.GetComponent(t);
            if (null == cpt)
            {
                cpt = _go.AddComponent(t);
            }
        }

        public static void AddButtonListener(GameObject _go, LuaFunction _callback)
        {
            Button btn = _go.GetComponent<Button>();
            UnityAction action = delegate () {
                _callback.Call();
            };
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }


        public static void RemoveAllUnParentChild(GameObject obj)
        {
            while (obj.transform.childCount > 0)
            {
                GameObject sub = obj.transform.GetChild(0).gameObject;
                sub.SetActive(false);
                sub.transform.parent = null;
                GameObject.Destroy(sub.gameObject);
            }
        }
    }
}


