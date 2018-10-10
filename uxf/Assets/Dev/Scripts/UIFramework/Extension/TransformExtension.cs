using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：TransformExtension  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/10/9 15:42:18
// ================================
namespace Game
{
    public static class TransformExtension
    {
        public static void DestroyAllChild(this Transform _trans)
        {
            int count = _trans.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform o = _trans.GetChild(i);
                if (o != null)
                {
                    GameObject.Destroy(o.gameObject);
                    o = null;
                }
            }
        }

        public static RectTransform rectTransform(this Transform _trans)
        {
            return _trans as RectTransform;
        }
        public static RectTransform rectTransform(this GameObject _trans)
        {
            return _trans.GetComponent<RectTransform>();
        }

        /// <summary>
        /// 获取组件，如果没有则添加，拓展给lua调
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T cpt = obj.GetComponent<T>();
            if (null == cpt)
            {
                cpt = obj.AddComponent<T>();
            }
            return cpt;
        }
    }
}
