using UnityEngine;
using System.Collections;
using Game;
using UnityEngine.Events;
using System;
using UObject = UnityEngine.Object;
using XLua;
using System.Collections.Generic;

namespace Game
{
    public class ResUtil
    {
        #region 角色模型预设路径访问接口.
        /// <summary>
        /// 角色模型预设所在目录，相对与Resources目录.
        /// </summary>
        private const string CHARACTER_PREFAB_PATH_RELATIVE_TO_RESOURCES = "Prefabs/Model/";

        /// <summary>
        /// 获取角色预设所在目录,相当于Resources目录.
        /// </summary>
        /// <param name="characterName">角色预设名.</param>
        /// <returns></returns>
        public static string GetCharacterPrefabPathRelativeToResources(string characterName)
        {
            return CHARACTER_PREFAB_PATH_RELATIVE_TO_RESOURCES + characterName;
        }
        #endregion

        #region UI界面预设访问接口.
        /// <summary>
        /// UI界面预设所在目录，相对与Resources目录.
        /// </summary>
        private const string UI_PREFAB_PATH_RELATIVE_TO_RESOURCES = "Prefabs/UI/";

        /// <summary>
        /// 获取UI界面预设所在目录,相当于Resources目录.
        /// </summary>
        /// <param name="uiPrefabName">UI界面预设名.</param>
        public static string GetUIPrefabPathRelativeToResources(string uiPrefabName)
        {
            return UI_PREFAB_PATH_RELATIVE_TO_RESOURCES + uiPrefabName;
        }
        #endregion

        //------------------

        /// <summary>
        /// 得到资源prefab
        /// </summary>
        /// <param name="_prefabName"></param>
        /// <param name="_prefabPath"></param>
        /// <param name="_callback"></param>
        public static void GetAssetsPrefab(string _abName,string _prefabPath, LuaFunction _callback)
        {
            int idx = _prefabPath.LastIndexOf('/');
            int len = _prefabPath.Length - idx - 1;
            string prefabName = _prefabPath.Substring(idx+1,len);

            if (AppConst.DebugMode)//调试模式
            {
                
                _callback.Call(new List<UObject> { Resources.Load(_prefabPath) });
            }
            else
            {
                if (string.IsNullOrEmpty(_abName))
                {
                    _callback.Call(new List<UObject> { Resources.Load(_prefabPath) });
                }
                else
                {
                    DebugUtil.Log("------------------------------------->_abName :" + _abName + " ;   _prefabPath:" + _prefabPath);
                    GameApp.resourceManager.LoadPrefab(_abName, prefabName, _callback);
                }
            }
        }



    }
}
