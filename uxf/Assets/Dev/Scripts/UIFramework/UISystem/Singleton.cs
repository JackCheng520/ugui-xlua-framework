/*******************************************
*项目名称 ：Assets.zJCGame.Utils
*项目描述 :
*类 名 称 : Singleton
*作    者 : ASUS 
*创建时间 : 2017/4/21 11:34:13
*更新时间 : 2017/4/21 11:34:13
********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class Singleton<T> where T : class,new()
    {
        private static T ins;

        private static readonly object lockObj = new object();

        public static T Instance
        {
            get 
            {
                lock (lockObj)
                {
                    if (ins == null)
                    {
                        ins = new T();
                    }
                    return ins;
                }
            }
        }
    }
}