using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class AppConst
    {
        public const bool DebugMode = true;                       //调试模式(仅仅用于调试lua代码，true则表示lua代码读取的是LuaFramework文件下的，false的话读取的资源路径是persistentDataPath）
        
        public const bool UpdateMode = false;                       //更新模式-默认关闭(更新模式打开必须把DebugMode设置为false) 
        public const bool LuaByteMode = false;                       //Lua字节码模式-默认关闭 
        public const bool LuaBundleMode = false;                    //Lua代码AssetBundle模式

        public const int TimerInterval = 1;
        public const int GameFrameRate = 30;                        //游戏帧频

        public const string AppName = "LuaFramework";               //应用程序名称
        public const string LuaTempDir = "Lua/";                    //临时目录
        public const string AppPrefix = AppName + "_";              //应用程序前缀
        public const string ExtName = ".unity3d";                   //素材扩展名
        public const string AssetDir = "StreamingAssets";           //素材目录 
        public const string WebUrl = "http://101.132.24.127/resversion20180517/";      //测试更新地址
        
        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/" + AppName;
            }
        }
    }
}