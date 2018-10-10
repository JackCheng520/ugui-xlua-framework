/*******************************************
*项目名称 ：Assets.Dev.Scripts.Utils
*项目描述 :
*类 名 称 : GameObjectUtil
*作    者 : ASUS 
*创建时间 : 2018/3/14 11:24:18
*更新时间 : 2018/3/14 11:24:18
********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
using Game.UI;
using UnityEngine;
using System.IO;

namespace Game
{
    public class GameUtil
    {
        /// <summary>
        /// 检测是否点击到UI上
        /// </summary>
        /// <returns></returns>
        public static bool CheckRaycastGuiElements()
        {
            /*
            if (UIRoot2D.Instance == null || UIRoot2D.Instance.UIEventSystem == null)
                return false;
            PointerEventData eventData = new PointerEventData(UIRoot2D.Instance.UIEventSystem);
#if UNITY_EDITOR
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount <= 0)
                return false;
			eventData.pressPosition = Input.GetTouch(0).position;
			eventData.position = Input.GetTouch(0).position;
#endif

            List<RaycastResult> list = new List<RaycastResult>();
            for (int i = 0; i < UIRoot2D.Instance.UIGraphicRaycaster.Length; i++)
            {
                UIRoot2D.Instance.UIGraphicRaycaster[i].Raycast(eventData, list);
                if (list.Count > 0)
                    break;
            }
            return list.Count > 0;
            */
            return false;
        }

        /// <summary>
        /// 删除物件
        /// </summary>
        /// <param name="_obj"></param>
        public static void Destory(UnityEngine.Object _obj)
        {
            GameObject.Destroy(_obj);
        }

        /// <summary>
        /// 寻找子节点
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public static GameObject FindChildByName(GameObject _parent, string _name)
        {
            if (_parent == null) { return null; }

            Transform[] trans = _parent.GetComponentsInChildren<Transform>();
            int len = trans.Length;

            for (int i = 0; i < len; i++)
            {
                Transform t = trans[i];
                if (t.name == _name)
                {
                    return t.gameObject;
                }
            }

            return null;
        }


        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string Md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string Md5file(byte[] fileBytes)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fileBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }

    }
}