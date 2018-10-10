using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：DictionaryExtension  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/10/9 15:56:22
// ================================
namespace Game
{
    public static class DictionaryExtension
    {
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> _dic, TKey _key, TValue _value) {
            if (!_dic.ContainsKey(_key)) {
                _dic.Add(_key, _value);
            }

            return _dic;
        }
        public static Dictionary<TKey, TValue> AddOrReplace<TKey,TValue>(this Dictionary<TKey, TValue> _dic, TKey _key, TValue _value) 
        {
            _dic[_key] = _value;
            return _dic;
        }

    }
}
